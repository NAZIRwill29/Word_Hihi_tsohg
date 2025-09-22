using UnityEngine;

public class UIFollowTarget : MonoBehaviour
{
    public Transform target;                   // Target to follow (world position)
    public Vector3 offset = new Vector3(0, 2f, 0); // Offset above target (in world space)
    public Camera mainCamera;                  // Assign if using Screen Space - Camera
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // World position + offset
        Vector3 worldPosition = target.position + offset;

        // Convert world position to screen position
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

        // Hide if behind the camera
        if (screenPosition.z < 0)
        {
            gameObject.SetActive(false);
            return;
        }
        else
        {
            gameObject.SetActive(true);
        }

        // Set the UI element position
        rectTransform.position = screenPosition;
    }
}
