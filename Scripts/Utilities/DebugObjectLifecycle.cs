using UnityEngine;

public class DebugObjectLifecycle : MonoBehaviour
{
    private void OnDisable()
    {
        Debug.Log($"[{name}] OnDisable called");
    }

    private void OnEnable()
    {
        Debug.Log($"[{name}] OnEnable called");
    }

    private void OnDestroy()
    {
        Debug.LogWarning($"[{name}] OnDestroy called!");
    }
}
