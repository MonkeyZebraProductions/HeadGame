using UnityEngine;

public class HurryUpScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] float TimerInterval = 90f;
    [SerializeField] AudioManager audioManager;
    [SerializeField] GameStateManager gameStateManager;
    float currentTime;
    void Awake()
    {
        GameStateManager.OnGameStateChanged += ResetTimerOnStateChange;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStateManager == null || gameStateManager.gamestate != GameStateManager.GameStatePS.Intro
            || gameStateManager.gamestate != GameStateManager.GameStatePS.EndSequence)
        {
            return;
        }
        currentTime += Time.deltaTime;

        if (currentTime >= TimerInterval)
        {
            audioManager.PlayRandomHurryUp();
            currentTime = 0;
        }
    }

    void ResetTimerOnStateChange(GameStateManager.GameStatePS state)
    {
        currentTime = 0;
    }
}
