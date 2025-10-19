using System;
using UnityEngine;

public class GameStateManager : Singleton<GameStateManager>
{
    public static GameStateManager Instance => _instance;

    public GameStatePS gamestate;
    public static event Action<GameStatePS> OnGameStateChanged;

    void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
       
    }

    public void UpdateGameState(GameStatePS newState)
    {
        gamestate = newState;
        OnGameStateChanged?.Invoke(newState);
        Debug.Log("GameState changed to: " + newState);
    }

    public enum GameStatePS
    {
        Intro,
        BootSequence,
        KeypadPuzzle,
        HeadPuzzle,
        ThreeSwitches,
        NobAndCameraState,
        EmotionsState,
        EndSequence
    }
}
