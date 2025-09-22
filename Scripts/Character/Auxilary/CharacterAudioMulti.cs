using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudioMulti : ObjectAudioMulti
{
    [SerializeField, Optional] HealthSystem m_Health;

    private void OnEnable()
    {
        if (m_Health)
            m_Health.StatChanged.AddListener(PlayHealthChangeSound);
    }
    private void OnDisable()
    {
        if (m_Health)
            m_Health.StatChanged.RemoveListener(PlayHealthChangeSound);
    }

    void PlayHealthChangeSound(float percentage, MicrobarAnimType microbarAnimType)
    {
        //Debug.Log("PlayHealthChangeSound");
        //PlayRandomClip(healthChangeData.soundName);
    }
}
