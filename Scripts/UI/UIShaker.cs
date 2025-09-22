using UnityEngine;

public class UIShaker : MonoBehaviour
{
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float magnitude = 10f;

    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private Coroutine shakeCoroutine;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    public void ButtonShake(float customMagnitude = 1f)
    {
        Shake(1, customMagnitude);
    }

    public void Shake(float customDuration = -1f, float customMagnitude = -1f)
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        float finalDuration = customDuration > 0 ? customDuration : duration;
        float finalMagnitude = customMagnitude > 0 ? customMagnitude : magnitude;

        shakeCoroutine = StartCoroutine(ShakeRoutine(finalDuration, finalMagnitude));
    }

    private System.Collections.IEnumerator ShakeRoutine(float shakeDuration, float shakeMagnitude)
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;

            rectTransform.anchoredPosition = originalPosition + new Vector3(offsetX, offsetY, 0f);
            elapsed += Time.unscaledDeltaTime; // Use unscaled for UI (so pause menus can still shake)
            yield return null;
        }

        rectTransform.anchoredPosition = originalPosition;
        shakeCoroutine = null;
    }
}
