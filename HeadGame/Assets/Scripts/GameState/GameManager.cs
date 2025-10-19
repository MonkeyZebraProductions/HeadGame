using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        GameStateManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameStateManager.GameStatePS state)
    {
        switch (state)
        {
            case GameStateManager.GameStatePS.Intro:
            case GameStateManager.GameStatePS.BootSequence:
            case GameStateManager.GameStatePS.HeadPuzzle:
            case GameStateManager.GameStatePS.ThreeSwitches:
            case GameStateManager.GameStatePS.EmotionsState:
            case GameStateManager.GameStatePS.NobAndCameraState:
            case GameStateManager.GameStatePS.EndSequence:
                // Now handled by ArduinoManager
                break;
        }
    }

    private void OnDestroy()
    {
        GameStateManager.OnGameStateChanged -= OnGameStateChanged;
    }
}
