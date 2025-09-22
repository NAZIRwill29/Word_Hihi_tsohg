using UnityEngine;

public class YSort : MonoBehaviour
{
    [Tooltip("Optional offset to shift sorting order (e.g. align with feet)")]
    public float offsetY = 0f;

    [Tooltip("Multiplier for precision. Higher = finer sorting steps.")]
    public int precision = 100;

    protected virtual void Awake()
    {

    }

    protected virtual void LateUpdate()
    {
        float yPos = transform.position.y + offsetY;
        int order = Mathf.RoundToInt(yPos * -precision);
    }
}
