using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "ActionControllerSO/GameObjectActionControllerSO", fileName = "GameObjectActionControllerSO")]
public class GameObjectActionControllerSO : ActionControllerSO
{
    public UnityEvent<GameObject> UnityEventGameObject;

    public void DoSomething(GameObject gameObject)
    {
        UnityEventGameObject.Invoke(gameObject);
    }
}
