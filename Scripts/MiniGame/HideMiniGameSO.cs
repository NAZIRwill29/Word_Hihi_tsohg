using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniGameSO/HideMiniGameSO", fileName = "HideMiniGameSO")]
public class HideMiniGameSO : MiniGameSO
{
    [SerializeField] private int m_LetterNum;
    [SerializeField] private List<char> m_Letters = new();
    private List<float> m_LetterExpireTimes = new();
    private List<bool> m_LetterActives = new();
    private float m_Max = 45;
    [SerializeField, Range(20, 35)] private float m_TimeDurationMin;
    [SerializeField, Tooltip("higher than TimeMin"), Range(25, 40)] private float m_TimeDurationMax;
    [SerializeField, Range(1, 10)] private float m_TimeSuccessMax;
    [SerializeField, Range(3, 15)] private float m_TimeSuccessMaxExt;
    [SerializeField] private int m_StartVisibilityMeter = 10;
    [SerializeField, Range(1, 5)] private int m_VisibilityMeterMinus = 1;
    [SerializeField, Range(3, 10)] private int m_VisibilityMeterGreatMinus = 3;
    [SerializeField] private int m_VisibilityMeter;
    private int m_LetterCount; //
    [SerializeField] private bool m_IsCanReset = true;
    [SerializeField] private float m_ZoomInOutAddNum = 0.25f, m_ZoomInOutInterval = 0.25f;

    public override void Initialize(WordSystem wordSystem)
    {
        base.Initialize(wordSystem);
        m_VisibilityMeter = m_StartVisibilityMeter;
        m_IsCanReset = false;
    }

    public override void StartMiniGame()
    {
        base.StartMiniGame();

        m_Letters.Clear();
        m_LetterExpireTimes.Clear();
        m_LetterActives.Clear();

        if (m_IsCanReset)
        {
            m_VisibilityMeter = m_StartVisibilityMeter;
            m_IsCanReset = false;
        }

        GameManager.Instance.MiniGameManager.MiniGameUI.ShowHideSection();

        m_LetterCount = Math.Min(m_LetterNum, GameManager.Instance.MiniGameManager.MiniGameUI.GetHideLetterBoxCount());
        float currentTime = Time.time;

        var newLetters = m_WordSystem.GetUnduplicatedRandomAlphabets(m_LetterCount);
        m_Letters.AddRange(newLetters);

        for (int i = 0; i < m_LetterCount; i++)
        {
            m_LetterActives.Add(true);
            GameManager.Instance.MiniGameManager.MiniGameUI.ShowHideLetter(i, true, m_Letters[i].ToString());
            GameManager.Instance.MiniGameManager.MiniGameUI.HideLetterCircleAnimation(i, true);

            float randomDuration = UnityEngine.Random.Range(m_TimeDurationMin, m_TimeDurationMax);
            m_LetterExpireTimes.Add(currentTime + randomDuration);

            // Added safety internally in MiniGameUI methods to avoid crash
            GameManager.Instance.MiniGameManager.MiniGameUI.ChangeSizeHideLetterCircleImageMinScaleDiff(i, randomDuration / m_Max);
            GameManager.Instance.MiniGameManager.MiniGameUI.SetLetterCircleIndicator(i, m_TimeSuccessMax / m_Max, m_TimeSuccessMaxExt / m_Max);
        }
    }

    public override void DoUpdate()
    {
        base.DoUpdate();
        if (!m_IsActive) return;

        float currentTime = Time.time;

        for (int i = 0; i < m_LetterCount; i++)
        {
            if (!m_LetterActives[i]) continue;

            float timeRemaining = m_LetterExpireTimes[i] - currentTime;
            if (timeRemaining <= 0f)
            {
                DoPunishment();
                GameManager.Instance.MiniGameManager.MiniGameUI.ShowHideLetter(i, false);
                GameManager.Instance.MiniGameManager.MiniGameUI.HideLetterCircleAnimation(i, false);
                m_LetterActives[i] = false;
                continue;
            }
            //Debug.Log("ChangeSizeHideLetterCircleImageMinScaleDiff");
            GameManager.Instance.MiniGameManager.MiniGameUI.ChangeSizeHideLetterCircleImageMinScaleDiff(i, timeRemaining / m_Max);
        }
    }

    public override void OnTyping(string word)
    {
        base.OnTyping(word);
        if (!m_IsActive) return;

        float currentTime = Time.time;

        for (int i = 0; i < m_LetterCount; i++)
        {
            if (word.Length > 0 && word[^1] == m_Letters[i])
            {
                float timeRemaining = m_LetterExpireTimes[i] - currentTime;

                if (timeRemaining <= m_TimeSuccessMax && timeRemaining > 0f)
                {
                    DoReward();
                }
                else if (timeRemaining <= m_TimeSuccessMaxExt && timeRemaining > 0f)
                {
                    DoPunishment();
                }
                else
                {
                    DoGreatPunishment();
                }
                GameManager.Instance.MiniGameManager.MiniGameUI.ShowHideLetter(i, false);
                GameManager.Instance.MiniGameManager.MiniGameUI.HideLetterCircleAnimation(i, false);
                m_LetterActives[i] = false;
            }
        }
    }

    protected override void DoReward()
    {
        base.DoReward();
        CameraManager.Instance.CameraZoom.ZoomInOut(m_ZoomInOutAddNum / 2, m_ZoomInOutInterval, 1);
    }

    protected override void DoPunishment()
    {
        base.DoPunishment();
        ChangeVisibilityMeter(-m_VisibilityMeterMinus);
        CameraManager.Instance.CameraZoom.ZoomInOut(m_ZoomInOutAddNum, m_ZoomInOutInterval, 2);
    }

    protected void DoGreatPunishment()
    {
        CameraManager.Instance.CameraShake.Shake(ShakeIntensity * 2);
        ChangeVisibilityMeter(-m_VisibilityMeterGreatMinus);
        CameraManager.Instance.CameraZoom.ZoomInOut(m_ZoomInOutAddNum * 2, m_ZoomInOutInterval, 4);
    }

    private void ChangeVisibilityMeter(int addNum)
    {
        m_VisibilityMeter += addNum;
        Debug.Log("m_VisibilityMeter = " + m_VisibilityMeter);
        GameManager.Instance.MiniGameManager.MiniGameUI.ChangeHideBar((float)m_VisibilityMeter / m_StartVisibilityMeter);

        if (m_VisibilityMeter <= 0)
        {
            Debug.Log("Visibility zero");
            GameManager.Instance.HidingMechanic.DiscoverableHide();
            Enemy2D enemy2D = GameManager.Instance.MiniGameManager.MiniGameInteractable.Collider2D.GetComponentInParent<Enemy2D>();
            if (enemy2D)
            {
                enemy2D.ForceEnemyAIStateChange("Chase");
            }
        }
    }

    public override void Exit(bool isByPlayer)
    {
        base.Exit(isByPlayer);
        m_IsCanReset = isByPlayer;
    }
}
