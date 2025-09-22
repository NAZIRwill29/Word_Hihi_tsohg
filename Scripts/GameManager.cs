using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameOverData
{
    public NameDataFlyweight NameData;
    public string Text;
    public Sprite Image;
    public Sprite Background;
}

public class GameManager : Singleton<GameManager>
{
    public Player2D player2D;
    public AbilityManager[] AbilityManagers;
    public ObjectPoolScriptable ObjectPool;
    public FloatingTextManager FloatingTextManager;
    [SerializeField] private GameObject m_PoolContainer;
    public List<GameObjectWithName> TextContainers;
    public WordSystem WordSystem;
    public WordCheck WordCheck;
    public WordPoolManager WordPoolManager;
    public PlayModeManager PlayModeManager;
    public MenuUI MenuUI;
    [SerializeField] private InputSystem m_InputSystem;
    public bool IsPause, IsNeedGameOver;
    public GameOverData GameOverData { get; set; }
    public HidingMechanic HidingMechanic;
    public LockingMechanic LockingMechanic;
    public LightManager LightManager;
    public EnemyManager EnemyManager;
    public MiniGameManager MiniGameManager;
    public NormalSystem NormalSystem;
    public Inventory Inventory;
    public StabilityStateManager PlayerStabilityStateManager;
    public HealthStateManager PlayerHealthStateManager;

    protected override void Awake()
    {
        base.Awake();

        DOTween.SetTweensCapacity(200, 100); // or higher if you need

        foreach (var item in AbilityManagers)
        {
            item.Initialize();
        }
        if (ObjectPool && m_PoolContainer)
            ObjectPool.Initialize(m_PoolContainer);
        if (FloatingTextManager)
            FloatingTextManager.Initialize(TextContainers[0].GameObject, PlayModeManager);
        if (WordCheck)
        {
            WordCheck.SetupWordList();
            WordCheck.SetupLetterSets();
        }

        if (WordSystem)
            WordSystem.Initialize();
        if (WordPoolManager)
            WordPoolManager.Initialize();
    }

    void Start()
    {
        //FRAME RATE()
        Application.targetFrameRate = 30;
        ChangePlayMode(PlayModeManager.NormalListenerNameData.Name);
        PlayerStabilityStateManager.Activate(player2D);
        PlayerHealthStateManager.Activate(player2D);
    }

    void Update()
    {
    }

    public void Pause(bool isTrue)
    {
        IsPause = isTrue;
        //Time.timeScale = isTrue ? 0f : 1f;
    }

    public void ChangePlayMode(string name)
    {
        Debug.Log("ChangePlayMode " + name);
        PlayModeManager.ChangePlayMode(name);
        m_InputSystem.Change(name);
        if (IsNeedGameOver)
        {
            GameOver();
            IsNeedGameOver = false;
        }
    }

    public void StartGame()
    {
        Pause(false);
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
        MenuUI.ShowGameOver(true, GameOverData.Image, GameOverData.Text, GameOverData.Background);
    }
}
