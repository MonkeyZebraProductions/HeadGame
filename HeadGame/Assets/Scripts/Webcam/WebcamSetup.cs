using UnityEngine;

public class WebcamSetup : MonoBehaviour
{

    WebCamTexture camTexture;
    [SerializeField] int CameraIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        WebCamDevice my_device = new WebCamDevice();
        WebCamDevice[] webCamDevices= WebCamTexture.devices;
        foreach (WebCamDevice currentDevice in webCamDevices)
        {
            Debug.Log(currentDevice.name);
        }
        my_device = webCamDevices[CameraIndex];
        camTexture = new WebCamTexture(my_device.name);
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = camTexture;
        camTexture.Play();
        Debug.Log(camTexture.width + ", " + camTexture.height);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
