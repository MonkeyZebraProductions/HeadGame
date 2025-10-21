using UnityEngine;
using UnityEngine.UI;

public class FakeLoadingScreen : MonoBehaviour
{
    private Canvas canvas;
    private Slider slider;
    [SerializeField] GameStateManager gameStateManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GetComponent<Canvas>();
        slider = GetComponentInChildren<Slider>();
        canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canvas == null || slider == null || gameStateManager == null || gameStateManager.gamestate != GameStateManager.GameStatePS.BootSequence)
        {
            return;
        }

        if (!canvas.enabled)
        {
            canvas.enabled = true;
        }

        if(slider.value<slider.maxValue)
        {
            slider.value += Time.deltaTime * Random.Range(0.0f, 3.0f);
        }
        else
        {
            gameObject.SetActive(false);
        }
        
    }
}
