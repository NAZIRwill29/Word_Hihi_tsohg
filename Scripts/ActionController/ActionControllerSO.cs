using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "ActionControllerSO/ActionControllerSO", fileName = "ActionControllerSO")]
public class ActionControllerSO : ScriptableObject
{
    public NameDataFlyweight NameData;
    public UnityEvent UnityEvent;

    public void DoSomething()
    {
        UnityEvent.Invoke();
    }
}
