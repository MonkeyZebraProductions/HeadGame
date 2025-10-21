using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SwitchPuzzle : MonoBehaviour
{
    [SerializeField] ArduinoManager arduinoManager;
    [SerializeField] AudioManager audioManager;
    [SerializeField] GameStateManager gameStateManager;
    [SerializeField] Image[] ButtonImages;
    [SerializeField] RawImage WebcamImage;

    bool[] SwitchState = new bool[3];
    bool[] targetState = new bool[3];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        for (int i = 0; i < 3; i++)
        {
            targetState[i] = Random.value>0.5f;
        }

        foreach (Image button in ButtonImages)
        {
            button.color = Color.red;
        }
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(1);
        gameStateManager.gamestate = GameStateManager.GameStatePS.ThreeSwitches;
    }

    // Update is called once per frame
    void Update()
    {
        if (arduinoManager == null || gameStateManager == null || gameStateManager.gamestate != GameStateManager.GameStatePS.ThreeSwitches)
        {
            Debug.Log(gameStateManager.gamestate);
            return;
        }

        if (CheckResult())
        {
            if (audioManager != null)
            {
                audioManager.PlayIfNotPlaying("Puzzle Succeeded");
                audioManager.Play("Camera Tuning Start");
            }
            if(WebcamImage != null)
            {
                WebcamImage.enabled = true;
            }
            gameStateManager.UpdateGameState(GameStateManager.GameStatePS.NobAndCameraState);
            return;
        }


        for (int i = 0; i < 3; i++)
        {
            Debug.Log(arduinoManager.SwitchStates[i]);
            SwitchState[i] = (arduinoManager.SwitchStates[i] == "ON" ? true:false);
            ButtonImages[i].color = (SwitchState[i] ? Color.green : Color.red);
        }
    }

    bool CheckResult()
    {
        for (int i = 0; i < 3; i++)
        {
            if (SwitchState[i] != targetState[i])
            {
                return false;
            }

        }
        return true;
    }
}
