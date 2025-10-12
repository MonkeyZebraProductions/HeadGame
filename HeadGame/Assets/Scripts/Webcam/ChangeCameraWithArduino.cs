using Unity.Mathematics;
using UnityEngine;

public class ChangeCameraWithArduino : MonoBehaviour
{

    public SerialController serialController;
    [SerializeField] RenderTexture WebCamTexture;
    [SerializeField] Camera WebCam;
    [SerializeField] int MaxCameraWidth = 576;
    [SerializeField] int MaxCameraHeight = 324;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
    }

    // Update is called once per frame
    void Update()
    {
        string message = serialController.ReadSerialMessage();

        if (message == null)
        {
            return;
        }

        // Check if the message is plain data or a connect/disconnect event.
        if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
        {
            Debug.Log("Connection established");
        }
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
        {
            Debug.Log("Connection attempt failed or disconnection detected");
        }
        else
        {
            Debug.Log("Message arrived: Counter: " + message);
            int currentCount = int.Parse(message);
            int camWidth = (int)math.remap(0, 19, 16, MaxCameraWidth, (float)currentCount);
            int camHeight = (int)math.remap(0,19,8,MaxCameraHeight,(float)currentCount);
            ResizeRenderTexture(WebCamTexture, WebCam, Mathf.Abs(camWidth), Mathf.Abs(camHeight));
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
