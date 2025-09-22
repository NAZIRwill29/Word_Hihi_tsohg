using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Manager/InteractManagerSO", fileName = "InteractManagerSO")]
public class InteractManagerSO : ScriptableObject
{
    public UnityEvent<string, bool> OnCanInteract;
    public UnityEvent<string> OnInteract;
    //TODO() - use to notify need key
    public UnityEvent<string> OnInteractLocked;
    public UnityEvent<Item> OnShowItemPopUp;
    public UnityEvent<ActionControllerSO, ActionControllerSO> OnShowItemPopUpT;
    public UnityEvent OnClosePopUp;
    [SerializeField] private ActionControllerSO m_PositiveActionControllerSO, m_NegatvieActionControllerSO;

    public void ShowItemPopUp(Item item)
    {
        OnShowItemPopUp.Invoke(item);
        OnShowItemPopUpT.Invoke(m_PositiveActionControllerSO, m_NegatvieActionControllerSO);
    }
}
