using System.Collections;
using UnityEngine;

public class SummoningCircle : MonoBehaviour
{
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    // Call this method to start the prewave effect
    public void StartPrewave(float prewaveDuration)
    {
        StartCoroutine(Prewave(prewaveDuration));
    }

    private IEnumerator Prewave(float duration)
    {
        float elapsedTime = 0f;
        float scaleMultiplier = 1.5f; // How much larger the circle grows

        while (elapsedTime < duration)
        {
            float scale = Mathf.PingPong(elapsedTime * 2, scaleMultiplier - 1) + 1; // Oscillate between 1 and 1.5
            transform.localScale = originalScale * scale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale; // Reset to original scale after prewave
    }
}

