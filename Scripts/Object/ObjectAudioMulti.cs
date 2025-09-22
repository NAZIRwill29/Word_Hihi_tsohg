using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAudioMulti : ObjectAudio
{
    [Header("Shared Data")]
    [Tooltip("Reference to shared AudioData ScriptableObject")]
    [SerializeField] protected AudioDataFlyweight m_SharedData;

    protected Dictionary<string, float> m_LastTimePlayedMusic = new();

    // Used when the player makes a sound - higher value = more sound
    [HideInInspector] public float soundVolProd = 2;

    protected virtual void Start()
    {
        if (m_SharedData == null || m_SharedData.SoundClips == null || m_SharedData.SoundClips.Length == 0)
            return;

        foreach (var soundClip in m_SharedData.SoundClips)
        {
            m_LastTimePlayedMusic[soundClip.Name] = -soundClip.m_CooldownTime;
        }

        if (m_ObjectT != null)  //  Fix: Ensure `m_ObjectT` is not null before subscribing
            m_ObjectT.OnThingHappened += ThingHappen;

        soundVolProd = 2;
    }

    private void OnDisable()
    {
        if (m_ObjectT != null)  //  Fix: Prevent null reference errors
            m_ObjectT.OnThingHappened -= ThingHappen;
    }

    protected virtual void ThingHappen(ThingHappenData thingHappenData)
    {
        //Debug.Log("Play sound " + thingHappenData.SoundName);
        if (!string.IsNullOrEmpty(thingHappenData.SoundName))
            PlayRandomClip(thingHappenData.SoundName);
    }

    protected void PlayRandomClip(string audioName)
    {
        SoundClip soundClip = GetSoundClip(audioName);
        if (soundClip == null)
            return;

        if (!m_LastTimePlayedMusic.ContainsKey(soundClip.Name))
            return;

        //Debug.Log($"PlayRandomClip");
        float timeToNextPlay = soundClip.m_CooldownTime + m_LastTimePlayedMusic[soundClip.Name];

        if (Time.time > timeToNextPlay)
        {
            //Debug.Log($"PlayRandomClip " + soundClip.m_name);
            m_LastTimePlayedMusic[soundClip.Name] = Time.time;

            AudioClip selectedClip = GetRandomClip(soundClip);
            if (selectedClip != null)  //  Fix: Ensure a valid clip is selected
            {
                m_AudioSource.clip = selectedClip;
                m_AudioSource.Play();
                //Debug.Log($"Playing sound: {soundClip.m_name}");
            }
        }
    }

    public void MuteSound(bool isTrue)
    {
        m_AudioSource.mute = isTrue;
    }

    public void StopPlaySound(string audioName)
    {
        if (string.IsNullOrEmpty(audioName))
            return;
        SoundClip soundClip = GetSoundClip(audioName);
        if (soundClip == null)
            return;

        AudioClip foundClip;
        if (VariableFinder.TryGetVariableFromArray(soundClip.m_Clips, m_AudioSource.clip, out foundClip))
        {
            if (m_AudioSource.isPlaying)
            {
                m_AudioSource.Stop();
            }
        }
    }

    protected AudioClip GetRandomClip(SoundClip soundClip)
    {
        if (soundClip.m_Clips == null || soundClip.m_Clips.Length == 0)
            return null;
        //Debug.Log($"GetRandomClip");
        int randomIndex = RandomGenerator.GenerateRandomNumber(0, soundClip.m_Clips.Length);  //  Fix: Use `Random.Range` correctly
        return soundClip.m_Clips[randomIndex];
    }

    protected SoundClip GetSoundClip(string audioName)
    {
        if (m_SharedData == null || m_SharedData.SoundClips == null || string.IsNullOrEmpty(audioName))
            return null;
        //Debug.Log($"GetSoundClip");
        if (VariableFinder.TryGetVariableContainNameFromArray(m_SharedData.SoundClips, audioName, out SoundClip clip))
            return clip;
        else
            return null;
        // return m_SharedData.SoundClips.FirstOrDefault(
        //     clip => string.Equals(clip.Name, audioName, System.StringComparison.OrdinalIgnoreCase)
        // );
    }
}
