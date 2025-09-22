using UnityEngine;

public class UIFollow2D : MonoBehaviour
{
    public Transform target;                   // The 2D target to follow
    public Vector3 offset = new Vector3(0, 1f, 0); // Offset above target (in world space)
    public Camera mainCamera;                  // Camera rendering the 2D scene

    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (target == null || !target.gameObject.activeInHierarchy) return;

        // Get the world position + offset
        Vector3 worldPos = target.position + offset;
        // Convert to screen position
        Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);

        // If screenPos is invalid, skip
        if (float.IsNaN(screenPos.x) || float.IsNaN(screenPos.y)) return;

        // Set UI position (canvas in Screen Space - Overlay or Camera)
        rectTransform.position = screenPos;
    }
}
