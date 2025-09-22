using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource impulseSource;

    /// <summary>
    /// Call this to trigger camera shake.
    /// </summary>
    /// <param name="intensity">Amplitude of the shake.</param>
    public void Shake(float intensity = 1f)
    {
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse(intensity);
        }
    }
}
