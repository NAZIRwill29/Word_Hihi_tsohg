using System.Collections.Generic;
using UnityEngine;
using Microlight.MicroBar;

public enum MicrobarAnimType
{
    simple, delay, disappear, impact, punch, shake
}

public class MicrobarGraphic : MonoBehaviour
{
    [Header("MicroBar")]
    //  0           1           2          3          4       5
    //simple    delayed     disappear   impact      punch   shake
    [SerializeField] protected List<MicroBar> microbars;
    protected const float k_MAX_HP = 100f;
    protected float m_hp;
    protected int m_currentType = 0;
    [SerializeField] protected MircobarSystem m_MircobarSystem;

    void OnEnable()
    {
        if (m_MircobarSystem)
        {
            //Debug.Log("OnEnable");
            m_MircobarSystem.StatInit.AddListener(Initialize);
            m_MircobarSystem.StatChanged.AddListener(UpdateHealthBar);
        }
    }

    void OnDisable()
    {
        if (m_MircobarSystem)
        {
            m_MircobarSystem.StatInit.RemoveListener(Initialize);
            m_MircobarSystem.StatChanged.RemoveListener(UpdateHealthBar);
        }
    }

    /// <summary>
    /// Hook into the system and initialize immediately.
    /// </summary>
    public void Activate(MircobarSystem system)
    {
        // Unhook if we were already hooked
        if (m_MircobarSystem != null)
            Deactivate();

        m_MircobarSystem = system;
        m_MircobarSystem.StatInit.AddListener(Initialize);
        m_MircobarSystem.StatChanged.AddListener(UpdateHealthBar);

        // Kick things off
        m_MircobarSystem.Initialize();
    }

    /// <summary>
    /// Unhook all listeners and clear the reference.
    /// </summary>
    public void Deactivate()
    {
        if (m_MircobarSystem == null) return;

        m_MircobarSystem.StatInit.RemoveListener(Initialize);
        m_MircobarSystem.StatChanged.RemoveListener(UpdateHealthBar);
        m_MircobarSystem = null;
    }

    /// <summary>
    /// Called by the system when stats are first initialized.
    /// </summary>
    public void Initialize(float healthValue)
    {
        m_hp = healthValue;
        foreach (var bar in microbars)
        {
            bar.Initialize(k_MAX_HP);
            bar.UpdateBar(m_hp, true, UpdateAnim.Heal);
        }
        ShowBar(m_currentType);
    }

    #region Damage / Heal

    private void UpdateHealthBar(float delta, MicrobarAnimType animType)
    {
        if (delta >= 0f) Heal(delta, animType);
        else Damage(-delta, animType);
    }

    public void Damage(float amount, MicrobarAnimType animType)
    {
        ChangeBars(animType);
        m_hp = Mathf.Max(0f, m_hp - amount);

        foreach (var bar in microbars)
            bar.UpdateBar(m_hp, false, UpdateAnim.Damage);
    }

    public void Heal(float amount, MicrobarAnimType animType)
    {
        ChangeBars(animType);
        m_hp = Mathf.Min(k_MAX_HP, m_hp + amount);

        foreach (var bar in microbars)
            bar.UpdateBar(m_hp, false, UpdateAnim.Heal);
    }

    #endregion

    /// <summary>
    /// Switch which visual variant is showing.
    /// </summary>
    private void ChangeBars(MicrobarAnimType type)
    {
        int idx = type switch
        {
            MicrobarAnimType.delay => 1,
            MicrobarAnimType.disappear => 2,
            MicrobarAnimType.impact => 3,
            MicrobarAnimType.punch => 4,
            MicrobarAnimType.shake => 5,
            _ => 0,   // simple
        };
        ShowBar(idx);
    }

    /// <summary>
    /// Override in subclasses to actually show/hide the correct bar(s).
    /// </summary>
    protected virtual void ShowBar(int index)
    {
        // e.g. for (int i=0; i<microbars.Count; i++) microbars[i].gameObject.SetActive(i == index);
    }
}
