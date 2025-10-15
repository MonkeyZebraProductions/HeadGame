using UnityEngine;
using UnityEngine.UI;

public class WirePuzzle : MonoBehaviour
{

    [SerializeField] SerialController serialController;
    [SerializeField] Image[] ButtonImages;
    bool[] WireConnected = new bool[3];
    string[] MessagePeices = new string[3];
    bool puzzleCompleted;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(Image button in ButtonImages)
        {
            button.color = Color.red;
        }
    }

    // Update is called once per frame
    void Update()
    {
        string message = serialController.ReadSerialMessage();

        if (message == null || puzzleCompleted)
        {
            return;
        }
        if (message == "Puzzle Complete!")
        {
            for (int i = 0; i < WireConnected.Length; i++)
            {
                ButtonImages[i].color = Color.green;
            }
            puzzleCompleted = true;
        }
        else
        {
            MessagePeices = message.Split(":");
            for (int i = 0; i < WireConnected.Length; i++)
            {
                int currentMessage = int.Parse(MessagePeices[i]);
                WireConnected[i] = (currentMessage ==1 ? true:false);
                ButtonImages[i].color = (WireConnected[i] ? Color.green : Color.red);
            }
            Debug.Log(WireConnected[0] +", " + WireConnected[1] + ", " + WireConnected[2]);

        }


    }
}
