using UnityEngine;
using TMPro;

public class TextFlicker : MonoBehaviour
{
    public TextMeshProUGUI text;  // Assign in Inspector
    public float minInterval = 0.05f; // shortest time light can stay on/off
    public float maxInterval = 0.3f;  // longest time light can stay on/off

    private void Start()
    {
        if (text == null)
            text = GetComponent<TextMeshProUGUI>();

        StartCoroutine(Flicker());
    }

    private System.Collections.IEnumerator Flicker()
    {
        while (true)
        {
            // Random on/off
            text.enabled = !text.enabled;

            // Wait a random time before switching again
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
