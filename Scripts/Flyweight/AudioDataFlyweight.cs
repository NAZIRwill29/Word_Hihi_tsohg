using UnityEngine;

[System.Serializable]
public class SoundClip : INameable
{
    [SerializeField] private string m_Name;
    public NameDataFlyweight NameDataFlyweight;
    public string Name
    {
        get => NameDataFlyweight != null ? NameDataFlyweight.Name : m_Name;
        set
        {
            if (NameDataFlyweight != null)
            {
                NameDataFlyweight.Name = value;
            }
            m_Name = value;  // Store locally in case NameDataFlyweight is null
        }
    }
    public AudioClip[] m_Clips;
    public float m_CooldownTime = 0.5f;
    //[HideInInspector] public float m_LastTimePlayed;
}

[CreateAssetMenu(fileName = "AudioData", menuName = "Flyweight/AudioData", order = 1)]
public class AudioDataFlyweight : ScriptableObject
{
    [Header("Shared Data")]
    [Tooltip("Shared string data across all instances of the audio data")]
    //walk  damage    heal   attack  revive   die  talk  
    public SoundClip[] SoundClips;
}
