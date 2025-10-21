using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextFlicker : MonoBehaviour
{
   // public TextMeshProUGUI text;  // Assign in Inspector
    public Image FlickerImage;
    public float minInterval = 0.05f; // shortest time light can stay on/off
    public float maxInterval = 0.3f;  // longest time light can stay on/off

    private void Start()
    {
        if (FlickerImage == null)
            FlickerImage = GetComponent<Image>();

        StartCoroutine(Flicker());
    }

    private System.Collections.IEnumerator Flicker()
    {
        while (true)
        {
            // Random on/off
            FlickerImage.enabled = !FlickerImage.enabled;

            // Wait a random time before switching again
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
