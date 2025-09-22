using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    private MiniGameSO m_MiniGameSO;
    public MiniGameUI MiniGameUI;
    [SerializeField] private TypingSystem m_TypingSystem;
    [SerializeField] private WordSystem m_WordSystem;
    [SerializeField] private bool m_IsInMiniGame;
    [SerializeField] private float m_Duration;
    public MiniGameInteractable MiniGameInteractable { get; private set; }
    public bool CanMiniGame = true;
    [SerializeField] private MiniGameSO[] m_MiniGameSOs;
    [SerializeField] private float m_CooldownDuration = 20;
    public float CooldownTime { get; private set; }
    void OnEnable()
    {
        if (m_TypingSystem)
        {
            m_TypingSystem.OnTyping.AddListener(OnTyping);
            m_TypingSystem.OnCompleteWord.AddListener(OnCompleteWord);
        }

        if (m_WordSystem)
        {
            m_WordSystem.OnSuccessWord.AddListener(OnSuccessWord);
            m_WordSystem.OnFailedWord.AddListener(OnFailedWord);
            m_WordSystem.OnSuccessWordNotDuplicate.AddListener(OnSuccessWordNotDuplicate);
            m_WordSystem.OnFailedWordNotDuplicate.AddListener(OnFailedWordNotDuplicate);
        }
    }

    void OnDisable()
    {
        if (m_TypingSystem)
        {
            m_TypingSystem.OnTyping.RemoveListener(OnTyping);
            m_TypingSystem.OnCompleteWord.RemoveListener(OnCompleteWord);
        }

        if (m_WordSystem)
        {
            m_WordSystem.OnSuccessWord.RemoveListener(OnSuccessWord);
            m_WordSystem.OnFailedWord.RemoveListener(OnFailedWord);
            m_WordSystem.OnSuccessWordNotDuplicate.RemoveListener(OnSuccessWordNotDuplicate);
            m_WordSystem.OnFailedWordNotDuplicate.RemoveListener(OnFailedWordNotDuplicate);
        }
    }

    void OnDestroy()
    {
        OnDisable(); // ensure proper cleanup
    }

    void Start()
    {
        for (int i = 0; i < m_MiniGameSOs.Length; i++)
        {
            m_MiniGameSOs[i].Initialize(m_WordSystem);
        }
        // foreach (MiniGameSO item in m_MiniGameSOs)
        // {
        //     item.Initialize(m_WordSystem);
        // }
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;
        if (CooldownTime >= 0)
        {
            CooldownTime -= deltaTime;
            return;
        }
        if (m_MiniGameSO != null && m_IsInMiniGame)
        {
            if (m_MiniGameSO.HasDuration)
            {
                m_Duration -= deltaTime;
                if (m_Duration <= 0)
                    ExitMiniGame(false);
            }
            m_MiniGameSO.DoUpdate();
        }
    }

    public void ChangeMiniGame(MiniGameSO miniGameSO, MiniGameInteractable miniGameInteractable = null)
    {
        if (miniGameSO == null || m_IsInMiniGame) return;
        m_IsInMiniGame = true;

        m_MiniGameSO = miniGameSO;
        m_Duration = m_MiniGameSO.Duration;
        m_TypingSystem.StartSystem(true);
        m_MiniGameSO.StartMiniGame();
        MiniGameUI.ShowMiniGame(true, miniGameSO.NameData.Name);

        MiniGameInteractable = miniGameInteractable;
    }

    public void ExitMiniGame(bool isByPlayer)
    {
        if (!m_IsInMiniGame) return;

        m_IsInMiniGame = false;
        if (m_MiniGameSO != null)
            m_MiniGameSO.Exit(isByPlayer);
        m_TypingSystem.StartSystem(false);
        MiniGameUI.ShowMiniGame(false);
        Debug.Log("ExitMiniGame");
    }

    private void OnTyping(string word)
    {
        m_MiniGameSO?.OnTyping(word);
    }

    private void OnCompleteWord(string word)
    {
        m_MiniGameSO?.OnCompleteWord(word);
    }

    private void OnSuccessWord(string word)
    {
        m_MiniGameSO?.OnSuccessWord(word);
    }

    private void OnFailedWord(string word)
    {
        m_MiniGameSO?.OnFailedWord(word);
    }

    private void OnSuccessWordNotDuplicate(string word)
    {
        m_MiniGameSO?.OnSuccessWordNotDuplicate(word);
    }

    private void OnFailedWordNotDuplicate(string word)
    {
        m_MiniGameSO?.OnFailedWordNotDuplicate(word);
    }

    public void StartCooldown()
    {
        CooldownTime = m_CooldownDuration;
    }
}
