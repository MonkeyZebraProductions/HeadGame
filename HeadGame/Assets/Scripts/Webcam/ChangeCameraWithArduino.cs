using Unity.Mathematics;
using UnityEngine;

public class ChangeCameraWithArduino : MonoBehaviour
{

    //public SerialController serialController;
    [SerializeField] ArduinoManager arduinoManager;
    [SerializeField] AudioManager audioManager;
    [SerializeField] GameStateManager gameStateManager;
    [SerializeField] RenderTexture WebCamTexture;
    [SerializeField] Camera WebCam;
    [SerializeField] int MaxCameraWidth = 576;
    [SerializeField] int MaxCameraHeight = 324;
    [SerializeField] int FinalClick = 19;
    bool PuzzleComplete = false;
    [SerializeField] bool DebugFullResolution;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
        ResizeRenderTexture(WebCamTexture, WebCam, 16, 9);
        gameStateManager.gamestate = GameStateManager.GameStatePS.NobAndCameraState;
    }

    // Update is called once per frame
    void Update()
    {

        if (DebugFullResolution)
        {
            ResizeRenderTexture(WebCamTexture, WebCam, MaxCameraWidth, MaxCameraHeight);
        }
        //string message = serialController.ReadSerialMessage();

        if (arduinoManager == null || gameStateManager == null || gameStateManager.gamestate != GameStateManager.GameStatePS.NobAndCameraState)
        {
            return;
        }

        
        //Debug.Log("Message arrived: Counter: " + message);
        int currentCount = (int)arduinoManager.Pos;
        int camWidth = (int)math.remap(0, 19, 16, MaxCameraWidth, (float)currentCount);
        int camHeight = (int)math.remap(0,19,9,MaxCameraHeight,(float)currentCount);
        ResizeRenderTexture(WebCamTexture, WebCam, Mathf.Abs(camWidth), Mathf.Abs(camHeight));
        if (Mathf.Abs(currentCount) == FinalClick)
        {
            Debug.Log("Camera Configured");
            gameStateManager.UpdateGameState(GameStateManager.GameStatePS.EmotionsState);
            if(audioManager != null)
            {
                audioManager.Play("Puzzle Succeeded");
            }
            //Play Audio File
        }
        
    }

    void ResizeRenderTexture(RenderTexture renderTexture, Camera camera, int width, int height)
    {
        renderTexture.Release();
        renderTexture.width = width;
        renderTexture.height = height;
        camera.ResetAspect();  //retain the correct aspect ratio this will change zoom levels based on aspect ratio
    }
}
