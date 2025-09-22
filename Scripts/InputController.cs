using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] protected InputManagerSO m_InputManagerSO;

    protected void BindInput(NameDataFlyweight inputData, UnityEngine.Events.UnityAction action)
    {
        if (inputData == null || m_InputManagerSO == null)
            return;

        if (m_InputManagerSO.InputDetails.TryGetValue(x => x.Name == inputData.Name, out InputDetail inputDetail))
        {
            inputDetail.Action.AddListener(action);
        }
    }

    protected void UnBindInput(NameDataFlyweight inputData, UnityEngine.Events.UnityAction action)
    {
        if (inputData == null || m_InputManagerSO == null)
            return;

        if (m_InputManagerSO.InputDetails.TryGetValue(x => x.Name == inputData.Name, out InputDetail inputDetail))
        {
            inputDetail.Action.RemoveListener(action);
        }
    }
}
