using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public NameDataFlyweight AbilityNameDataFlyweight;
    //[SerializeField] protected string m_AbilityName;
    public string AbilityName { get => AbilityNameDataFlyweight.Name; }
    [ExpandableTextArea] public string Descriptions;
    [ExpandableTextArea] public string ShortDescriptions;
    [Tooltip("Image texture for UI button")]
    [SerializeField] protected Sprite m_ButtonIcon;
    [Header("Visuals")]
    [SerializeField] protected NameDataFlyweight m_FXNameDataFlyweight;
    [SerializeField] protected NameDataFlyweight m_SoundNameDataFlyweight;
    public bool IsPassiveAbility;
    public float Cooldown = 0.65f;
    public float Delay = 0f;
    public Sprite ButtonIcon => m_ButtonIcon;
    public float Cost = 0;
    //public float TimeToNextPlay { get; set; }
    [Tooltip("for effect ObjectT only"), SerializeField] private NatureElement m_NatureElement;
    [Tooltip("for effect ObjectT only"), SerializeField] private List<NatureElementEffectCharacterSO> m_NatureElementEffectCharacterSOs;

    //ABILITY() - 7(LAST)
    // Each Strategy can use custom logic. Implement the Use method in the subclasses
    public virtual void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        // Use method logs name, plays sound, and particle effect
        Debug.Log($"Using ability: {AbilityName}");
        ThingHappen(objectT);
    }

    protected virtual void ThingHappen(ObjectT objectT)
    {
        string soundName = m_SoundNameDataFlyweight != null ? m_SoundNameDataFlyweight.Name : "";
        string fxName = m_FXNameDataFlyweight != null ? m_FXNameDataFlyweight.Name : "";
        objectT.ThingHappen(new()
        {
            SoundName = soundName,
            FXName = fxName
        });
        if (m_NatureElement)
        {
            if (objectT is Character character)
            {
                character.NatureElement = m_NatureElement;
                if (m_NatureElementEffectCharacterSOs.Count > 0)
                    character.NatureElementControllerCharacter.NatureElementEffectCharacterSOs = m_NatureElementEffectCharacterSOs;
            }
        }
    }

    // public virtual void OnBenefit(){}
    // public virtual void OnSupport(){}
    // public virtual void OnStrength(){}
    // public virtual void OnWeakness(){}
}