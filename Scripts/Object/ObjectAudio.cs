using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ObjectAudio : MonoBehaviour
{
    protected ObjectT m_ObjectT;
    [SerializeField] protected AudioSource m_AudioSource;

    protected virtual void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_ObjectT = GetComponent<ObjectT>();
    }
}
