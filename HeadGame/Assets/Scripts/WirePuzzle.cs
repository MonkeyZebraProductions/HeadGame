using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class WirePuzzle : MonoBehaviour
{

    //[SerializeField] SerialController serialController;
    [SerializeField] ArduinoManager arduinoManager;
    [SerializeField] AudioManager audioManager;
    [SerializeField] GameStateManager gameStateManager;
    [SerializeField] Image[] ButtonImages;
    bool[] WireConnected = new bool[3];
    //string[] MessagePeices = new string[3];
    bool puzzleCompleted;

    [Header ("Voltage Values")]
    [SerializeField] int LowValue = 20;
    [SerializeField] int MidValue = 450;
    [SerializeField] int HighValue = 900;

    enum VoltageValue { SMALL, MED, LARGE, NONE};
    VoltageValue A0VoltageValue, A1VoltageValue, A2VoltageValue = VoltageValue.NONE;

    VoltageValue[] VoltageArray = {VoltageValue.MED, VoltageValue.LARGE, VoltageValue.SMALL };
    List<VoltageValue> StartingArray = new List<VoltageValue> { VoltageValue.SMALL, VoltageValue.MED, VoltageValue.LARGE };
    VoltageValue[] CurrentVoltage = new VoltageValue[3];
    VoltageValue[] PrevVoltage = new VoltageValue[3];
    bool[] targetCorrect = { true, true, true };

    bool playSound = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(Image button in ButtonImages)
        {
            button.color = Color.red;
        }

        //Randomise Starting Voltages
        for (int i = 0; i < 3; i++)
        {
            VoltageValue v = StartingArray[Random.Range(0, 3 - i)];
            VoltageArray[i] = v;
            StartingArray.Remove(v);
        }
        Debug.Log(VoltageArray[0] + ", " + VoltageArray[1] + ", " + VoltageArray[2]);
    }

    // Update is called once per frame
    void Update()
    {

        //string message = serialController.ReadSerialMessage();

        if (arduinoManager == null || gameStateManager == null || gameStateManager.gamestate != GameStateManager.GameStatePS.HeadPuzzle 
            || puzzleCompleted)
        {
            return;
        }

        if(WireConnected == targetCorrect)
        { 
            gameStateManager.UpdateGameState(GameStateManager.GameStatePS.ThreeSwitches);
            return; 
        }

        AnalogReads();

        CurrentVoltage[0] = A0VoltageValue;
        CurrentVoltage[1] = A1VoltageValue;
        CurrentVoltage[2] = A2VoltageValue;

        CheckVoltages();
        if(playSound)
        {
            if (audioManager != null)
            {
                audioManager.Play("Click");
            }
            playSound = false;
        }
    }

    void CheckVoltages()
    {
        for (int i = 0; i < 3; i++)
        {
            if (CurrentVoltage[i] == VoltageArray[i])
            {
                WireConnected[i] = true;
            }
            else
            {
                WireConnected[i] = false;
            }
            ButtonImages[i].color = (WireConnected[i] ? Color.green : Color.red);
            if (PrevVoltage[i] != CurrentVoltage[i])
            {
                playSound = true;
            }
            PrevVoltage[i] = CurrentVoltage[i];
        }
    }

    void AnalogReads()
    {
        VoltageValue[] voltageValues = new VoltageValue[3]; 
        for(int i = 0;i < 3;i++)
        {
            if(arduinoManager.WireValues[i] < LowValue)
            {
                voltageValues[i] = VoltageValue.SMALL;
            }
            else if (arduinoManager.WireValues[i] < MidValue)
            {
                voltageValues[i] = VoltageValue.MED;
            }
            else if (arduinoManager.WireValues[i] < HighValue)
            {
                voltageValues[i] = VoltageValue.LARGE;
            }
            else
            {
                voltageValues[i] = VoltageValue.NONE;
            }
        }

        A0VoltageValue = voltageValues[0];
        A1VoltageValue = voltageValues[1];
        A2VoltageValue = voltageValues[2];
    }
}
