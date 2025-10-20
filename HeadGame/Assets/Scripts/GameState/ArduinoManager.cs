using System;
using System.IO.Ports;
using UnityEngine;

public class ArduinoManager : MonoBehaviour
{
    [Header("Arduino Serial Settings")]
    public string portName = "dev/cu.usbmodem201912341"; // replace with your Arduino port
    public int baudRate = 9600;

    //private int keyCount = 0;

    private SerialPort port;

    [Header("Keypad Puzzle")]
    public string Key;
    public int KeyCount;

    [Header("Head Puzzle")]
    public int[] WireValues = new int[3];

    [Header("Three Switches Puzzle")]
    public string[] SwitchStates = new string[3];

    [Header("Nob and Camera Puzzle")]
    public long Pos;


    [Header("Emotions Puzzle")]
    public int[] RGBValues = new int[3];

    private void Awake()
    {
       GameStateManager.OnGameStateChanged += OnGameStateChanged;

    }

    private void Start()
    {
        port = new SerialPort(portName, baudRate);
        port.ReadTimeout = 50;

        try
        {
            port.Open();
            System.Threading.Thread.Sleep(2000);
            Debug.Log("Arduino connected on " + portName);
            GameStateManager.Instance.UpdateGameState(GameStateManager.GameStatePS.Intro);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to open Arduino port: " + e.Message);
        }

    }


    private void Update()
    {
        if (!port.IsOpen) return;

        try
        {
            string msg = port.ReadLine().Trim();
            if (!string.IsNullOrEmpty(msg))
            {
                Debug.Log("Arduino: " + msg);
                HandleArduinoMessage(msg);
            }
        }
        catch { /* ignore timeout exceptions */ }
    }

    // ---------------- Send Commands to Arduino ----------------
    private void SendCommand(string command)
    {
        if (port.IsOpen)
        {
            port.Write(command);
            Debug.Log("Sent to Arduino: " + command);
        }
    }

    private void UpdateLCD(string msg)
    {
        SendCommand("LCD:" + msg + "\n");
    }

    private void ActivateSensor(string sensor)
    {
        SendCommand("ACTIVATE:" + sensor + "\n");
    }

    // ---------------- Handle Arduino Messages ----------------
    private void HandleArduinoMessage(string msg)
    {
        if (msg == "TOUCH_DETECTED")
        {
            GameStateManager.Instance.UpdateGameState(GameStateManager.GameStatePS.BootSequence);
        }
        else if (msg.StartsWith("KEYPAD_PRESSED:"))
        {
            Key = msg.Substring(15);
            Debug.Log("Keypad pressed: " + Key);
            KeyCount++;

            if (KeyCount == 4)
            {
                KeyCount = 0;
                GameStateManager.Instance.UpdateGameState(GameStateManager.GameStatePS.HeadPuzzle);

            }

            // Handle code entry in Unity
        }
        else if (msg.StartsWith("ANALOG:"))
        {
            string[] parts = msg.Substring(7).Split(',');
            int val1 = int.Parse(parts[0]);
            int val2 = int.Parse(parts[1]);
            int val3 = int.Parse(parts[2]);
            WireValues[0]= val1; 
            WireValues[1]=val2; 
            WireValues[2]=val3;
            Debug.Log($"Analog values: {val1}, {val2}, {val3}");

            // Example: check puzzle solved
            if (val1 > 500 && val2 < 400 && val3 > 600)
            {
                GameStateManager.Instance.UpdateGameState(GameStateManager.GameStatePS.ThreeSwitches);
            }


        }
        else if (msg.StartsWith("ENCODER:"))
        {
            Pos = long.Parse(msg.Substring(8));
            Debug.Log("Rotary Encoder: " + Pos);

            //Sync up on Moday About Rotary Puzzle

            //if (pos > 10)
            //{
            //    GameStateManager.Instance.UpdateGameState(GameStateManager.GameStatePS.EmotionsState);
            //}
        }
        else if (msg.StartsWith("SWITCHES:"))
        {
            SwitchStates = msg.Substring(9).Split(',');
            if (SwitchStates.Length == 3 && SwitchStates[0] == "ON" && SwitchStates[1] == "ON" && SwitchStates[2] == "ON")
            {
                GameStateManager.Instance.UpdateGameState(GameStateManager.GameStatePS.NobAndCameraState);
            }
        }
        else if (msg.StartsWith("RGB:"))
        {
            string[] rgb = msg.Substring(4).Split(',');
            int r = int.Parse(rgb[0]);
            int g = int.Parse(rgb[1]);
            int b = int.Parse(rgb[2]);
            RGBValues[0] = r; 
            RGBValues[1] = g; 
            RGBValues[2] = b;
            Debug.Log($"RGB Sensor: {r},{g},{b}");

            // Example: detect colors
            if (r > g + 100 && r > b + 100) {
                Debug.Log("Red detected");
                GameStateManager.Instance.UpdateGameState(GameStateManager.GameStatePS.EndSequence);

            }
            // You can add green, blue, yellow detection logic here
        }
    }

    // ---------------- Game State Changes ----------------
    private void OnGameStateChanged(GameStateManager.GameStatePS state)
    {
        switch (state)
        {
            case GameStateManager.GameStatePS.Intro:
                ActivateSensor("TOUCH");
                UpdateLCD("Touch to Start");
                break;

            case GameStateManager.GameStatePS.BootSequence:
                ActivateSensor("KEYPAD");
                UpdateLCD("Enter Code");
                break;

            case GameStateManager.GameStatePS.HeadPuzzle:
                ActivateSensor("ANALOG");
                ActivateSensor("ENCODER"); // optional: read rotary simultaneously
                UpdateLCD("Head Puzzle...");
                break;

            case GameStateManager.GameStatePS.ThreeSwitches:
                ActivateSensor("SWITCHES");
                UpdateLCD("Toggle Switches");
                break;

            case GameStateManager.GameStatePS.NobAndCameraState:
                UpdateLCD("Unity Camera...");
                break;

            case GameStateManager.GameStatePS.EmotionsState:
                ActivateSensor("RGB");
                UpdateLCD("Emotion Puzzle");
                break;

            case GameStateManager.GameStatePS.EndSequence:
                UpdateLCD("Game Complete!");
                break;
        }
    }




    private void OnDestroy()
    {
        GameStateManager.OnGameStateChanged -= OnGameStateChanged;

        if (port != null && port.IsOpen)
            port.Close();
    }
}
