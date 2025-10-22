using UnityEngine;
using UnityEngine.UI;

public class EmotionsPuzzle : MonoBehaviour
{
    [SerializeField] ArduinoManager arduinoManager;
    [SerializeField] AudioManager audioManager;
    [SerializeField] GameStateManager gameStateManager;

    [SerializeField] Image[] EmotionImages;
    [SerializeField] Image[] ResponceImages;
    enum Colour {RED, BLUE, YELLOW, NONE}

    Colour CurrentColour = Colour.NONE;
    Colour TargetColor = Colour.RED;

    bool puzzleStarted = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < EmotionImages.Length; i++)
        {
            if (EmotionImages[i] != null && ResponceImages[i] != null)
            {
                EmotionImages[i].enabled = false;
                ResponceImages[i].enabled = false;
            }
        }

        EmotionImages[0].enabled = true;
    }

    private void ShowResponce(int index)
    {
        foreach (Image image in ResponceImages)
        {
            if(image != null)
            {
                image.enabled = false;
            }
        }
        ResponceImages[index].enabled = true;
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

        //if (!puzzleStarted)
        //{
        //    if(audioManager != null)
        //    {
        //        audioManager.Play("Angry");
        //    }
        //    puzzleStarted = true;
        //}

        int r = arduinoManager.RGBValues[0];
        int g = arduinoManager.RGBValues[1];
        int b = arduinoManager.RGBValues[2];

        //Debug.Log($"RGB Sensor: {r},{g},{b}");
        if (!audioManager.IsPlaying("Angry") && !audioManager.IsPlaying("Sad") && !audioManager.IsPlaying("Happy"))
        {
            if (r > g + 80 && r > b + 80)
            {
                CurrentColour = Colour.RED;
                audioManager.PlayIfNotPlaying("Angry Responce");
                ShowResponce(0);
                Debug.Log("Red detected");
            }
            else if (r <50 && b > r + 50)
            {
                CurrentColour = Colour.BLUE;
                audioManager.PlayIfNotPlaying("Sad Responce");
                ShowResponce(1);
                Debug.Log("Blue detected");
            }
            else if (b < 50 && r > b + 50)
            {
                CurrentColour = Colour.YELLOW;
                audioManager.PlayIfNotPlaying("Happy Responce");
                ShowResponce(2);
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
                    EmotionImages[0].enabled = false;
                    EmotionImages[1].enabled = true;
                    break;
                case Colour.BLUE:
                    audioManager.Play("Happy");
                    TargetColor = Colour.YELLOW;
                    EmotionImages[1].enabled = false;
                    EmotionImages[2].enabled = true;
                    break;
                case Colour.YELLOW:
                    TargetColor = Colour.NONE;
                    EmotionImages[2].enabled = false;
                    //EmotionImages[1].enabled = true;
                    audioManager.Play("Final Speech");
                    gameStateManager.UpdateGameState(GameStateManager.GameStatePS.EndSequence);
                    break;
            }
        }


    }
}
