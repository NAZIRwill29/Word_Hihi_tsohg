using System.Collections.Generic;
using UnityEngine;
//TODO() - IN INSPECTOR -  FILL IN SUBCHAPTER
public class GhostManipulator : MonoBehaviour
{
    [SerializeField] private List<Ghost> m_Ghosts;
    [SerializeField] private GhostTemplateContainerSO m_GhostTemplateContainerSO;

    void Start()
    {
        foreach (var ghost in m_Ghosts)
        {
            ChangeGhostTemplate(ghost, m_GhostTemplateContainerSO.GetRandomGhostTemplate());
            ghost.GhostCombatDataFlyweight = m_GhostTemplateContainerSO.GetGhostCombatDataFlyweight();
        }
    }

    public void ChangeGhostTemplate(Ghost ghost, GhostTemplate ghostTemplate)
    {
        ghost.GhostTemplate = ghostTemplate;
        ghost.Activate(true);
    }
}
