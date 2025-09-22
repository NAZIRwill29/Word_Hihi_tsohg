using UnityEngine;

[CreateAssetMenu(fileName = "HoppingAdvanceGhostAbility", menuName = "Abilities/Ghost/HoppingAdvanceGhostAbility")]
public class HoppingAdvanceGhostAbility : PassiveGhostAbility
{
    // [Tooltip("How many 'hops' the ghost advances per typo")]
    // [SerializeField] private int m_LetterNum = 2;
    [SerializeField] private MagicData healMagicData;

    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT, data);
        // If additional use logic is needed, add here
    }

    public override void OnFailedLetterRule()
    {
        DoPunishment();
    }

    public override void DoPunishment()
    {
        GhostCombatSystem.Instance.CombatSystem.CombatDataManager.Ghost.ObjectMagic.Heal(healMagicData);

        // get the normal pull amount from your GhostTemplate (private field in CombatDistanceSystem)
        // float basePull = GhostCombatSystem.Instance.GhostTemplate.DistancePercentChangePlusPull;
        // // hop forward m_LetterNum times
        // float totalPull = basePull * m_LetterNum;
        // GhostCombatSystem.Instance.CombatSystem.CombatDistanceSystem.AdvanceNow(totalPull);
    }
}
