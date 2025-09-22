using UnityEngine;

public class MiniGameInteractable : MonoBehaviour
{
    public bool IsActive { get; set; }
    [SerializeField] protected MiniGameSO m_MiniGameSO;
    public Collider2D Collider2D { get; protected set; }

    protected virtual void StartMiniGame()
    {
        if (!IsActive) return;
        if (!GameManager.Instance.MiniGameManager.CanMiniGame) return;
        if (GameManager.Instance.MiniGameManager.CooldownTime >= 0) return;
        //do minigame
        GameManager.Instance.MiniGameManager.ChangeMiniGame(m_MiniGameSO, this);
        //Debug.Log("StartMiniGame");
    }
}
