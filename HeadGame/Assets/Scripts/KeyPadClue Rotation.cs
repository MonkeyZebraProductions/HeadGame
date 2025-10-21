using TMPro;
using UnityEngine;

public class KeyPadClueRotation : MonoBehaviour
{
    [SerializeField] string[] Clues;
    [SerializeField] float TimerInterval = 7;
    [SerializeField] GameStateManager gameStateManager;
    [SerializeField] TextMeshProUGUI ClueText;
    float currentTime;
    int index = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ClueText.text = Clues[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStateManager == null || gameStateManager.gamestate != GameStateManager.GameStatePS.BootSequence)
        {
            return;
        }

        currentTime += Time.deltaTime;
        if(currentTime > TimerInterval)
        {
            currentTime = 0;
            index++;
            if(index >= Clues.Length)
            {
                index = 0;
            }
            ClueText.text = Clues[index];
        }
    }
}
