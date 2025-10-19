using UnityEngine;

public class TouchScript : MonoBehaviour
{
    [SerializeField] GameStateManager gameStateManager;
    [SerializeField] AudioManager audioManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        gameStateManager = FindAnyObjectByType<GameStateManager>();
    }

    // Update is called once per frame
    void Update()
    { 
    }

    public void JustTouched()
    {

        if (gameStateManager == null || gameStateManager.gamestate != GameStateManager.GameStatePS.Intro
            || gameStateManager.gamestate != GameStateManager.GameStatePS.EndSequence)
        {
            return;
        }
        audioManager.PlayRandomDontTouch();

    }
}
