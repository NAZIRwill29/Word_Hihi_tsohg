using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GhostTemplateContainerSO", menuName = "Ghost System/GhostTemplate Container")]
public class GhostTemplateContainerSO : ScriptableObject
{
    [SerializeField] private List<GhostTemplate> m_GhostTemplates;
    [SerializeField] private GhostCombatDataFlyweight m_GhostCombatDataFlyweight;

    public GhostTemplate GetRandomGhostTemplate()
    {
        return m_GhostTemplates[Random.Range(0, m_GhostTemplates.Count)];
    }

    public GhostCombatDataFlyweight GetGhostCombatDataFlyweight()
    {
        return m_GhostCombatDataFlyweight;
    }
}
