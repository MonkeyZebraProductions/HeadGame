using UnityEngine;

public class EmotionsPuzzle : MonoBehaviour
{
    [SerializeField] ArduinoManager arduinoManager;
    [SerializeField] AudioManager audioManager;
    [SerializeField] GameStateManager gameStateManager;
    enum Colour {RED, BLUE, YELLOW, NONE}

    Colour CurrentColour = Colour.NONE;
    Colour TargetColor = Colour.RED;

    bool puzzleStarted = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameStateManager.gamestate = GameStateManager.GameStatePS.EmotionsState;
    }

    // Update is called once per frame
    void Update()
    {
        if (arduinoManager == null || gameStateManager == null || audioManager == null || 
            gameStateManager.gamestate != GameStateManager.GameStatePS.EmotionsState)
        {
            //Debug.Log("RETURN");
            return;
        }

        if (!puzzleStarted)
        {
            if(audioManager != null)
            {
                audioManager.Play("Angry");
            }
            puzzleStarted = true;
        }

        int r = arduinoManager.RGBValues[0];
        int g = arduinoManager.RGBValues[1];
        int b = arduinoManager.RGBValues[2];

        //Debug.Log($"RGB Sensor: {r},{g},{b}");
        if (!audioManager.IsPlaying("Angry") && !audioManager.IsPlaying("Sad") && !audioManager.IsPlaying("Happy"))
        {
            if (r > g + 100 && r > b + 100)
            {
                CurrentColour = Colour.RED;
                audioManager.PlayIfNotPlaying("Angry Responce");
                Debug.Log("Red detected");
            }
            else if (r <50 && b > r + 50)
            {
                CurrentColour = Colour.BLUE;
                audioManager.PlayIfNotPlaying("Sad Responce");
                Debug.Log("Blue detected");
            }
            else if (b < 50 && r > b + 50)
            {
                CurrentColour = Colour.YELLOW;
                audioManager.PlayIfNotPlaying("Happy Responce");
                Debug.Log("Yellow detected");
            }
            else
            {
                CurrentColour = Colour.NONE;
            }

        }

        if(CurrentColour == TargetColor)
        {
            switch (TargetColor)
            {
                case Colour.RED:
                    TargetColor = Colour.BLUE;
                    audioManager.Play("Sad");
                    break;
                case Colour.BLUE:
                    audioManager.Play("Happy");
                    TargetColor = Colour.YELLOW;
                    break;
                case Colour.YELLOW:
                    TargetColor = Colour.NONE; 
                    gameStateManager.UpdateGameState(GameStateManager.GameStatePS.EndSequence);
                    break;
            }
        }


    }
}
