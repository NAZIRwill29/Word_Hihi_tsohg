using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public List<GhostTemplate> GhostTemplates;
    [SerializeField] private WordPoolManager wordPoolManager;

    void Start()
    {
        foreach (var ghostTemplate in GhostTemplates)
        {
            ghostTemplate.Initialize(wordPoolManager);
        }
    }
}
