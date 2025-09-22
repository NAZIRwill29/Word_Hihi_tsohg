using UnityEngine;

public class CombatController : InputController
{
    [SerializeField] private CombatSystem m_CombatSystem;
    [SerializeField] private NameDataFlyweight m_ConfirmWordInput;
    [SerializeField] private NameDataFlyweight m_ChangeModeInput;
    [SerializeField] private NameDataFlyweight m_EscapeWordBarInput;
    [SerializeField] private NameDataFlyweight m_ExorcismLetterBarInput;
    [SerializeField] private NameDataFlyweight m_WeaknessWordBarInput;
    [SerializeField] private NameDataFlyweight m_InventoryBarInput;
    [SerializeField] private NameDataFlyweight m_WordBarChangeInput;
    [SerializeField] private NameDataFlyweight m_EscapeInput;
    [SerializeField] private NameDataFlyweight m_SpaceInput;

    private void OnEnable()
    {
        if (!m_InputManagerSO) return;

        BindInput(m_ConfirmWordInput, m_CombatSystem.InitConfirmWord);
        BindInput(m_ChangeModeInput, m_CombatSystem.ChangeMode);
        BindInput(m_EscapeWordBarInput, m_CombatSystem.EscapeWordBar);
        BindInput(m_ExorcismLetterBarInput, m_CombatSystem.ExorcismLetterBar);
        BindInput(m_WeaknessWordBarInput, m_CombatSystem.WeaknessWordBar);
        BindInput(m_InventoryBarInput, m_CombatSystem.InventoryBar);
        BindInput(m_WordBarChangeInput, m_CombatSystem.ShiftChangeWordBar);
        BindInput(m_EscapeInput, m_CombatSystem.EscapeCombat);
        BindInput(m_SpaceInput, m_CombatSystem.StartCombat);
        BindInput(m_SpaceInput, m_CombatSystem.ExitCombat);
    }

    private void OnDisable()
    {
        if (!m_InputManagerSO) return;

        UnBindInput(m_ConfirmWordInput, m_CombatSystem.InitConfirmWord);
        UnBindInput(m_ChangeModeInput, m_CombatSystem.ChangeMode);
        UnBindInput(m_EscapeWordBarInput, m_CombatSystem.EscapeWordBar);
        UnBindInput(m_ExorcismLetterBarInput, m_CombatSystem.ExorcismLetterBar);
        UnBindInput(m_WeaknessWordBarInput, m_CombatSystem.WeaknessWordBar);
        UnBindInput(m_InventoryBarInput, m_CombatSystem.InventoryBar);
        UnBindInput(m_WordBarChangeInput, m_CombatSystem.ShiftChangeWordBar);
        UnBindInput(m_EscapeInput, m_CombatSystem.EscapeCombat);
        UnBindInput(m_SpaceInput, m_CombatSystem.StartCombat);
        UnBindInput(m_SpaceInput, m_CombatSystem.ExitCombat);
    }
}
