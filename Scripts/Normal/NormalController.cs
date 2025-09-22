using UnityEngine;

public class NormalController : InputController
{
    [SerializeField] private NormalSystem m_NormalSystem;
    [SerializeField] private NameDataFlyweight m_ThrowInput;
    [SerializeField] private NameDataFlyweight m_MainMenuInput;
    [SerializeField] private NameDataFlyweight m_InteractInput;
    [SerializeField] protected NameDataFlyweight m_GameMenuInput;
    [SerializeField] protected NameDataFlyweight m_GameMenuBarInput;
    [SerializeField] protected NameDataFlyweight m_InventoryBarInput;
    [SerializeField] protected NameDataFlyweight m_WordBarInput;
    [SerializeField] protected NameDataFlyweight m_MapBarInput;
    [SerializeField] protected NameDataFlyweight m_DictionaryBarInput;
    [SerializeField] private NameDataFlyweight m_PositiveInput;
    [SerializeField] private NameDataFlyweight m_NegativeInput;
    [SerializeField] private NameDataFlyweight m_InInput;
    [SerializeField] private NameDataFlyweight m_OutInput;
    [SerializeField] private NameDataFlyweight m_FlashLIghtInput;

    private void OnEnable()
    {
        if (!m_InputManagerSO) return;

        BindInput(m_ThrowInput, m_NormalSystem.InitThrow);
        BindInput(m_MainMenuInput, m_NormalSystem.InitMainMenu);
        BindInput(m_InteractInput, m_NormalSystem.InitInteract);
        BindInput(m_GameMenuInput, m_NormalSystem.InitGameMenu);
        BindInput(m_GameMenuBarInput, m_NormalSystem.InitGameMenuBar);
        BindInput(m_InventoryBarInput, m_NormalSystem.InitInventoryBar);
        BindInput(m_WordBarInput, m_NormalSystem.InitWordBar);
        BindInput(m_MapBarInput, m_NormalSystem.InitMapBar);
        BindInput(m_DictionaryBarInput, m_NormalSystem.InitDictionaryBar);
        BindInput(m_PositiveInput, m_NormalSystem.InitPositive);
        BindInput(m_NegativeInput, m_NormalSystem.InitNegative);
        BindInput(m_InInput, m_NormalSystem.InitIn);
        BindInput(m_OutInput, m_NormalSystem.InitOut);
        BindInput(m_FlashLIghtInput, m_NormalSystem.InitFlashLight);
    }

    private void OnDisable()
    {
        if (!m_InputManagerSO) return;

        UnBindInput(m_ThrowInput, m_NormalSystem.InitThrow);
        UnBindInput(m_MainMenuInput, m_NormalSystem.InitMainMenu);
        UnBindInput(m_InteractInput, m_NormalSystem.InitInteract);
        UnBindInput(m_GameMenuInput, m_NormalSystem.InitGameMenu);
        UnBindInput(m_GameMenuBarInput, m_NormalSystem.InitGameMenuBar);
        UnBindInput(m_InventoryBarInput, m_NormalSystem.InitInventoryBar);
        UnBindInput(m_WordBarInput, m_NormalSystem.InitWordBar);
        UnBindInput(m_MapBarInput, m_NormalSystem.InitMapBar);
        UnBindInput(m_DictionaryBarInput, m_NormalSystem.InitDictionaryBar);
        UnBindInput(m_PositiveInput, m_NormalSystem.InitPositive);
        UnBindInput(m_NegativeInput, m_NormalSystem.InitNegative);
        UnBindInput(m_InInput, m_NormalSystem.InitIn);
        UnBindInput(m_OutInput, m_NormalSystem.InitOut);
        UnBindInput(m_FlashLIghtInput, m_NormalSystem.InitFlashLight);
    }
}
