using UnityEngine;

public class CharacterAudio : ObjectAudio
{
    [SerializeField]
    float m_CooldownTime = 2f;

    [SerializeField]
    AudioClip[] m_BounceClips;

    float m_LastTimePlayed;

    private void Start()
    {
        m_LastTimePlayed = -m_CooldownTime;
    }

    public void PlayRandomClip()
    {
        // Calculate the time to play the next clip.            
        float timeToNextPlay = m_CooldownTime + m_LastTimePlayed;

        // Check if the cooldown time has passed.
        if (Time.time > timeToNextPlay)
        {
            m_LastTimePlayed = Time.time;
            m_AudioSource.clip = GetRandomClip();
            m_AudioSource.Play();
            Debug.Log("play sound");
        }
    }

    private AudioClip GetRandomClip()
    {
        // Get a random clip from the array based on the number of clips in it.
        int randomIndex = UnityEngine.Random.Range(0, m_BounceClips.Length);
        return m_BounceClips[randomIndex];
    }
}
