using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public EnemyRuntimeSetSO EnemyRuntimeSetSO;

    public void SetAllAnimation(string name, bool isTrue)
    {
        foreach (var item in EnemyRuntimeSetSO.Items)
        {
            if (item is Enemy2D enemy2D && enemy2D.Animator != null)
                enemy2D.Animator.SetBool(name, isTrue);
        }
    }
    public void SetAllAnimation(string name)
    {
        foreach (var item in EnemyRuntimeSetSO.Items)
        {
            if (item is Enemy2D enemy2D && enemy2D.Animator != null)
                enemy2D.Animator.SetTrigger(name);
        }
    }
    public void SetAllAnimation(string name, int num)
    {
        foreach (var item in EnemyRuntimeSetSO.Items)
        {
            if (item is Enemy2D enemy2D && enemy2D.Animator != null)
                enemy2D.Animator.SetInteger(name, num);
        }
    }
    public void SetAllAnimation(string name, float num)
    {
        foreach (var item in EnemyRuntimeSetSO.Items)
        {
            if (item is Enemy2D enemy2D && enemy2D.Animator != null)
                enemy2D.Animator.SetFloat(name, num);
        }
    }
}
