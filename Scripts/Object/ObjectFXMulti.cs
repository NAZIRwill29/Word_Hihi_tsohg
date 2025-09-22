using System.Collections.Generic;
using UnityEngine;

public class ObjectFXMulti : MonoBehaviour
{
    protected ObjectT m_ObjectT;
    //[SerializeField] protected ObjectPool m_ObjectPool;
    private Dictionary<string, float> m_ObjParticleSystemTimeCooldownDict = new();
    [SerializeField] private GameObject m_CenterFX;
    private static readonly List<string> _keyBuffer = new();

    void Awake()
    {
        m_ObjectT = GetComponent<ObjectT>();
        m_ObjectT.OnThingHappened += ThingHappen;
    }
    void OnDisable()
    {
        m_ObjectT.OnThingHappened -= ThingHappen;
    }

    void Update()
    {
        if (m_ObjParticleSystemTimeCooldownDict.Count == 0) return;

        _keyBuffer.Clear();
        _keyBuffer.AddRange(m_ObjParticleSystemTimeCooldownDict.Keys); // Avoids per-frame allocation

        foreach (var key in _keyBuffer)
        {
            m_ObjParticleSystemTimeCooldownDict[key] = Mathf.Max(0, m_ObjParticleSystemTimeCooldownDict[key] - Time.deltaTime);
        }
    }

    private void ThingHappen(ThingHappenData thingHappenData)
    {
        if (!string.IsNullOrEmpty(thingHappenData.FXName))
            PlayEffect(thingHappenData.FXName, m_CenterFX.transform.position);
    }

    public void PlayEffectB(string name)
    {
        PlayEffect(name, m_CenterFX.transform.position);
    }

    public void PlayEffect(string name, Vector2 position)
    {
        var pooledObject = GameManager.Instance.ObjectPool.GetPooledObject(name);
        if (pooledObject == null)
        {
            Debug.LogWarning($"No pooled object found for {name}");
            return;
        }

        if (pooledObject is ObjParticleSystem objParticleSystem)
        {
            if (objParticleSystem.m_ParticleSystem != null)
            {
                if (objParticleSystem.IsOneAtATime)
                {
                    if (m_ObjParticleSystemTimeCooldownDict.TryGetValue(name, out float cooldown))
                    {
                        if (cooldown > 0)
                        {
                            objParticleSystem.Deactivate();
                            return;
                        }

                        m_ObjParticleSystemTimeCooldownDict[name] = objParticleSystem.Cooldown;
                        //Debug.Log($"ObjParticleSystem {name} cooldown {cooldown}");
                        PlayEffect(objParticleSystem, position);
                    }
                    else
                    {
                        m_ObjParticleSystemTimeCooldownDict[name] = objParticleSystem.Cooldown;
                        //Debug.Log($"Add ObjParticleSystem {name}");
                        PlayEffect(objParticleSystem, position);
                    }
                }
                else
                {
                    //Debug.Log($"Play ObjParticleSystem {name}");
                    PlayEffect(objParticleSystem, position);
                }
            }
        }
    }

    private void PlayEffect(ObjParticleSystem objParticleSystem, Vector2 position)
    {
        if (objParticleSystem?.m_ParticleSystem == null) return;

        objParticleSystem.m_ParticleSystem.transform.position = position;
        objParticleSystem.m_ParticleSystem.Stop();
        objParticleSystem.m_ParticleSystem.Play();
    }
}
