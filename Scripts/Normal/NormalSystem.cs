using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NormalSystem : MonoBehaviour
{
    public StabilityStateManager GhostStabilityStateManager;
    public StabilityStateManager PlayerStabilityStateManager;
    public NormalUI NormalUI;
    public InputManagerSO InputManagerSO;
    public Torchlight Torchlight;
    [SerializeField] private NameDataFlyweight m_PositiveInput, m_NegativeInput;
    public InputDetail PositiveInputDetail { get; set; }
    public InputDetail NegativeInputDetail { get; set; }
    private bool m_IsPause, m_IsGameMenu;
    public string CurrentState { get; set; }
    [SerializeField] private GameObject m_DropParent;
    [SerializeField] private List<ActionControllerSO> m_ActionControllerSOs;
    [HideInInspector] public ActionControllerSO PositiveActionControllerSO, NegativeActionControllerSO;
    [SerializeField] private InteractManagerSO m_InteractManagerSO;

    void OnEnable()
    {
        if (GhostStabilityStateManager)
        {
            GhostStabilityStateManager.OnLowStability.AddListener(OnGhostLowStability);
            GhostStabilityStateManager.OnHighStability.AddListener(OnGhostHighStability);
            GhostStabilityStateManager.OnZeroStability.AddListener(OnGhostLowStability);
        }

        if (PlayerStabilityStateManager)
        {
            PlayerStabilityStateManager.OnLowStability.AddListener(OnPlayerLowStability);
            PlayerStabilityStateManager.OnHighStability.AddListener(OnPlayerHighStability);
        }

        if (m_InteractManagerSO)
        {
            m_InteractManagerSO.OnShowItemPopUpT.AddListener(OnShowItemPopUpT);
        }
    }

    void OnDisable()
    {
        if (GhostStabilityStateManager)
        {
            GhostStabilityStateManager.OnLowStability.RemoveListener(OnGhostLowStability);
            GhostStabilityStateManager.OnHighStability.RemoveListener(OnGhostHighStability);
            GhostStabilityStateManager.OnZeroStability.RemoveListener(OnGhostLowStability);
        }

        if (PlayerStabilityStateManager)
        {
            PlayerStabilityStateManager.OnLowStability.RemoveListener(OnPlayerLowStability);
            PlayerStabilityStateManager.OnHighStability.RemoveListener(OnPlayerHighStability);
        }

        if (m_InteractManagerSO)
        {
            m_InteractManagerSO.OnShowItemPopUpT.RemoveListener(OnShowItemPopUpT);
        }
    }

    void Start()
    {
        if (InputManagerSO.InputDetails.TryGetValue(x => x.Name == m_PositiveInput.Name, out InputDetail positiveInputDetail))
            PositiveInputDetail = positiveInputDetail;
        if (InputManagerSO.InputDetails.TryGetValue(x => x.Name == m_NegativeInput.Name, out InputDetail negativeInputDetail))
            NegativeInputDetail = negativeInputDetail;
    }

    private void OnShowItemPopUpT(ActionControllerSO actionControllerSO1, ActionControllerSO actionControllerSO2)
    {
        PositiveActionControllerSO = actionControllerSO1;
        NegativeActionControllerSO = actionControllerSO2;
    }

    #region Controller
    public void InitThrow()
    {

    }
    public void InitMainMenu()
    {
        CurrentState = "MainMenu";
        m_IsPause = !m_IsPause;
        GameManager.Instance.Pause(m_IsPause);
        //TODO()
    }
    public void InitInteract()
    {

    }
    public void InitGameMenu()
    {
        m_IsPause = !m_IsPause;
        m_IsGameMenu = m_IsPause;
        CurrentState = m_IsPause ? "GameMenu" : string.Empty;
        GameManager.Instance.Pause(m_IsPause);
        NormalUI.OnShowMenu();
        //Debug.Log("InitGameMenu");
    }
    public void InitGameMenuBar()
    {
        if (!m_IsGameMenu) return;
        NormalUI.OnMenuBarTab();
        //Debug.Log("InitGameMenuBar");
    }
    public void InitInventoryBar()
    {
        if (!m_IsGameMenu) return;
        NormalUI.InitInventoryBar();
    }
    public void InitWordBar()
    {
        if (!m_IsGameMenu) return;
        NormalUI.InitWordBar();
    }
    public void InitMapBar()
    {
        if (!m_IsGameMenu) return;
        NormalUI.InitMapBar();
    }
    public void InitDictionaryBar()
    {
        if (!m_IsGameMenu) return;
        NormalUI.InitDictionaryBar();
    }
    public void InitPositive()
    {
        //NormalUI.UseItem();
        PositiveActionControllerSO.DoSomething();
    }
    public void InitNegative()
    {
        //NormalUI.DropItem(m_DropParent);
        if (NegativeActionControllerSO is GameObjectActionControllerSO gameObjectActionControllerSO)
            gameObjectActionControllerSO.DoSomething(m_DropParent);
        else
            NegativeActionControllerSO.DoSomething();
    }
    public void InitIn()
    {

    }
    public void InitOut()
    {
        NormalUI.CloseInspectItem();
    }
    public void InitFlashLight()
    {
        Torchlight.TurnTorchLight();
    }
    #endregion

    #region Stability
    private void OnGhostHighStability(string name)
    {
        NormalUI.OverlayAnimation(false, 0);
    }
    private void OnGhostLowStability(string name)
    {
        NormalUI.OverlayAnimation(true, 1);
    }
    private void OnPlayerLowStability(string name)
    {

    }
    private void OnPlayerHighStability(string name)
    {

    }
    #endregion

    public void PositiveBtn()
    {
        PositiveInputDetail?.Action.Invoke();
    }
    public void NegativeBtn()
    {
        NegativeInputDetail?.Action.Invoke();
    }

    public void TestButton()
    {
        OnGhostLowStability("angry");
    }
    public void TestButton2()
    {
        OnGhostLowStability("");
    }
}
