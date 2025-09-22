using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
    [SerializeField] private WordSystem m_WordSystem;
    [SerializeField] private CombatSystem m_CombatSystem;
    [SerializeField] private CombatUIElement m_CombatUIElement;
    [SerializeField] private ScrambleUIManager m_ScrambleUIManager;
    [SerializeField] private MoverUIManager m_MoverUIManager;
    [SerializeField] private FlickerUIManager m_FlickerUIManager;
    [SerializeField] private UIShakeGroupDOTween m_UIShakeGroupDOTweenUI;
    [SerializeField] private UIShakerDOTween m_UIShakerDOTween;
    public UnityEvent<int> OnFlickerStartBright;

    // private float m_PlayerPosX;
    // private Transform m_GhostPos;
    // private Vector3 m_GhostPosOri;
    // private RectTransform m_ghostRect;
    // private Vector2 m_ghostAnchoredOrig;
    public int StruggleModeTextBlockMax { get; set; }
    public int StruggleModeBigTextBlockMax { get; set; }
    public int StruggleModeBigTextContMax { get; set; }
    [SerializeField] private float m_ShakeDuration = 0.75f;
    [SerializeField] private float m_ShakeStrength = 7f;


    private void OnEnable()
    {
        if (!m_CombatUIElement) return;
        if (m_WordSystem != null)
        {
            m_WordSystem.OnFailedWord.AddListener(m_CombatUIElement.OnWordNotExist);
            m_WordSystem.OnFailedWordNotDuplicate.AddListener(m_CombatUIElement.OnFailedWordNotDuplicate);
        }

        if (m_CombatSystem == null) return;

        m_CombatSystem.OnStartGhostPrompt.AddListener(m_CombatUIElement.OnStartGhostPrompt);
        m_CombatSystem.OnHideGhostPrompt.AddListener(m_CombatUIElement.OnHideGhostPrompt);
        m_CombatSystem.OnShowGhostPrompt.AddListener(m_CombatUIElement.OnShowGhostPrompt);
        m_CombatSystem.OnStopGhostPrompt.AddListener(m_CombatUIElement.OnStopGhostPrompt);
        m_CombatSystem.OnChangeWordBar.AddListener(m_CombatUIElement.OnChangeWordBar);
        m_CombatSystem.OnChangeMode.AddListener(m_CombatUIElement.OnChangeMode);
        m_CombatSystem.OnStartCombat.AddListener(StartCombat);
        m_CombatSystem.OnActiveAbility.AddListener(m_CombatUIElement.OnActiveAbility);
        m_CombatSystem.OnPassiveAbility.AddListener(m_CombatUIElement.OnPassiveAbility);
        m_CombatSystem.OnExitCombat.AddListener(OnExitCombat);

        if (m_CombatSystem.CombatTypingSystem != null)
        {
            m_CombatSystem.CombatTypingSystem.OnTyping.AddListener(m_CombatUIElement.ChangeTypingText);
            m_CombatSystem.CombatTypingSystem.OnExorcismTyping.AddListener(m_CombatUIElement.ChangeExorcismTypingText);
        }

        if (m_CombatSystem.LetterPlacementSystem != null)
        {
            m_CombatSystem.LetterPlacementSystem.OnSetLetterPlacement.AddListener(m_CombatUIElement.ChangeLetterPlacement);
        }

        if (m_CombatSystem.ExorcismLetterSystem != null)
        {
            m_CombatSystem.ExorcismLetterSystem.OnChangeExorcismLetter.AddListener(m_CombatUIElement.SetExorcismLetter);
        }

        if (m_CombatSystem.LetterRuleSystem != null)
        {
            m_CombatSystem.LetterRuleSystem.OnSetLetterRule.AddListener(m_CombatUIElement.SetLetterRule);
        }

        if (m_CombatSystem.EscapeWordSystem != null)
        {
            m_CombatSystem.EscapeWordSystem.OnSetEscapeWord.AddListener(m_CombatUIElement.SetEscapeWord);
            m_CombatSystem.EscapeWordSystem.OnEscapeWordSuccess.AddListener(m_CombatUIElement.EscapeWordSuccess);
        }

        // if (m_CombatSystem.CombatDataManager.Ghost.ObjectMagic != null)
        // {
        //     m_CombatSystem.CombatDataManager.Ghost.ObjectMagic.OnRecoveryPlus.AddListener(m_CombatUIElement.OnGhostManaRecoveryPlus);
        //     m_CombatSystem.CombatDataManager.Ghost.ObjectMagic.OnRecoverySpeedInc.AddListener(m_CombatUIElement.OnGhostManaRecoverySpeedIncrease);
        // }

        // if (m_CombatSystem.CombatDistanceSystem != null)
        // {
        //     m_CombatSystem.CombatDistanceSystem.OnGhostMove.AddListener(OnGhostMove);
        //     m_CombatSystem.CombatDistanceSystem.OnGhostMovePlus.AddListener(m_CombatUIElement.OnGhostMovePlus);
        //     m_CombatSystem.CombatDistanceSystem.OnGhostSpeedIncrease.AddListener(m_CombatUIElement.OnGhostSpeedIncrease);
        // }

        if (m_CombatSystem.WeaknessWordSystem != null)
        {
            m_CombatSystem.WeaknessWordSystem.OnWeaknessWordChange.AddListener(m_CombatUIElement.OnWeaknessWordChange);
            m_CombatSystem.WeaknessWordSystem.OnCurseWordChange.AddListener(m_CombatUIElement.OnCurseWordChange);
        }

        if (m_CombatSystem.ExorcismWordSystem != null)
        {
            m_CombatSystem.ExorcismWordSystem.OnExorciseWeaknessWord.AddListener(m_CombatUIElement.OnExorciseWeaknessWord);
            m_CombatSystem.ExorcismWordSystem.OnExorciseOrdinaryWord.AddListener(m_CombatUIElement.OnExorciseOrdinaryWord);
            m_CombatSystem.ExorcismWordSystem.OnExorciseCurseWord.AddListener(m_CombatUIElement.OnExorciseCurseWord);
        }

        if (m_CombatSystem.GhostStabilityStateManager)
        {
            m_CombatSystem.GhostStabilityStateManager.OnLowStability.AddListener(m_CombatUIElement.OnGhostLowStability);
            m_CombatSystem.GhostStabilityStateManager.OnHighStability.AddListener(m_CombatUIElement.OnGhostHighStability);
        }

        if (m_CombatSystem.PlayerStabilityStateManager)
        {
            m_CombatSystem.PlayerStabilityStateManager.OnLowStability.AddListener(m_CombatUIElement.OnPlayerLowStability);
            m_CombatSystem.PlayerStabilityStateManager.OnHighStability.AddListener(m_CombatUIElement.OnPlayerHighStability);
        }

        if (m_CombatSystem.GhostHealthStateManager)
        {
            m_CombatSystem.GhostHealthStateManager.OnLowHealth.AddListener(m_CombatUIElement.OnGhostLowHealth);
            m_CombatSystem.GhostHealthStateManager.OnHighHealth.AddListener(m_CombatUIElement.OnGhostHighHealth);
        }

        if (m_CombatSystem.PlayerHealthStateManager)
        {
            m_CombatSystem.PlayerHealthStateManager.OnLowHealth.AddListener(m_CombatUIElement.OnPlayerLowHealth);
            m_CombatSystem.PlayerHealthStateManager.OnHighHealth.AddListener(m_CombatUIElement.OnPlayerHighHealth);
        }
    }

    private void OnDisable()
    {
        if (!m_CombatUIElement) return;
        if (m_WordSystem != null)
        {
            m_WordSystem.OnFailedWord.RemoveListener(m_CombatUIElement.OnWordNotExist);
            m_WordSystem.OnFailedWordNotDuplicate.RemoveListener(m_CombatUIElement.OnFailedWordNotDuplicate);
        }

        if (m_CombatSystem == null) return;

        m_CombatSystem.OnStartGhostPrompt.RemoveListener(m_CombatUIElement.OnStartGhostPrompt);
        m_CombatSystem.OnHideGhostPrompt.RemoveListener(m_CombatUIElement.OnHideGhostPrompt);
        m_CombatSystem.OnShowGhostPrompt.RemoveListener(m_CombatUIElement.OnShowGhostPrompt);
        m_CombatSystem.OnStopGhostPrompt.RemoveListener(m_CombatUIElement.OnStopGhostPrompt);
        m_CombatSystem.OnChangeWordBar.RemoveListener(m_CombatUIElement.OnChangeWordBar);
        m_CombatSystem.OnChangeMode.RemoveListener(m_CombatUIElement.OnChangeMode);
        m_CombatSystem.OnStartCombat.RemoveListener(StartCombat);
        m_CombatSystem.OnActiveAbility.RemoveListener(m_CombatUIElement.OnActiveAbility);
        m_CombatSystem.OnPassiveAbility.RemoveListener(m_CombatUIElement.OnPassiveAbility);
        m_CombatSystem.OnExitCombat.RemoveListener(OnExitCombat);

        if (m_CombatSystem.CombatTypingSystem != null)
        {
            m_CombatSystem.CombatTypingSystem.OnTyping.RemoveListener(m_CombatUIElement.ChangeTypingText);
            m_CombatSystem.CombatTypingSystem.OnExorcismTyping.RemoveListener(m_CombatUIElement.ChangeExorcismTypingText);
        }

        if (m_CombatSystem.LetterPlacementSystem != null)
        {
            m_CombatSystem.LetterPlacementSystem.OnSetLetterPlacement.RemoveListener(m_CombatUIElement.ChangeLetterPlacement);
        }

        if (m_CombatSystem.ExorcismLetterSystem != null)
        {
            m_CombatSystem.ExorcismLetterSystem.OnChangeExorcismLetter.RemoveListener(m_CombatUIElement.SetExorcismLetter);
        }

        if (m_CombatSystem.LetterRuleSystem != null)
        {
            m_CombatSystem.LetterRuleSystem.OnSetLetterRule.RemoveListener(m_CombatUIElement.SetLetterRule);
        }

        if (m_CombatSystem.EscapeWordSystem != null)
        {
            m_CombatSystem.EscapeWordSystem.OnSetEscapeWord.RemoveListener(m_CombatUIElement.SetEscapeWord);
            m_CombatSystem.EscapeWordSystem.OnEscapeWordSuccess.RemoveListener(m_CombatUIElement.EscapeWordSuccess);
        }

        if (m_CombatSystem.CombatDataManager.Ghost && m_CombatSystem.CombatDataManager.Ghost.ObjectMagic)
        {
            m_CombatSystem.CombatDataManager.Ghost.ObjectMagic.OnRecoveryPlus.RemoveListener(m_CombatUIElement.OnGhostManaRecoveryPlus);
            m_CombatSystem.CombatDataManager.Ghost.ObjectMagic.OnRecoverySpeedInc.RemoveListener(m_CombatUIElement.OnGhostManaRecoverySpeedIncrease);
        }

        // if (m_CombatSystem.CombatDistanceSystem != null)
        // {
        //     m_CombatSystem.CombatDistanceSystem.OnGhostMove.RemoveListener(OnGhostMove);
        //     m_CombatSystem.CombatDistanceSystem.OnGhostMovePlus.RemoveListener(m_CombatUIElement.OnGhostMovePlus);
        //     m_CombatSystem.CombatDistanceSystem.OnGhostSpeedIncrease.RemoveListener(m_CombatUIElement.OnGhostSpeedIncrease);
        // }

        if (m_CombatSystem.WeaknessWordSystem != null)
        {
            m_CombatSystem.WeaknessWordSystem.OnWeaknessWordChange.RemoveListener(m_CombatUIElement.OnWeaknessWordChange);
            m_CombatSystem.WeaknessWordSystem.OnCurseWordChange.RemoveListener(m_CombatUIElement.OnCurseWordChange);
        }

        if (m_CombatSystem.ExorcismWordSystem != null)
        {
            m_CombatSystem.ExorcismWordSystem.OnExorciseWeaknessWord.RemoveListener(m_CombatUIElement.OnExorciseWeaknessWord);
            m_CombatSystem.ExorcismWordSystem.OnExorciseOrdinaryWord.RemoveListener(m_CombatUIElement.OnExorciseOrdinaryWord);
            m_CombatSystem.ExorcismWordSystem.OnExorciseCurseWord.RemoveListener(m_CombatUIElement.OnExorciseCurseWord);
        }

        if (m_CombatSystem.GhostStabilityStateManager)
        {
            m_CombatSystem.GhostStabilityStateManager.OnLowStability.RemoveListener(m_CombatUIElement.OnGhostLowStability);
            m_CombatSystem.GhostStabilityStateManager.OnHighStability.RemoveListener(m_CombatUIElement.OnGhostHighStability);
        }

        if (m_CombatSystem.PlayerStabilityStateManager)
        {
            m_CombatSystem.PlayerStabilityStateManager.OnLowStability.RemoveListener(m_CombatUIElement.OnPlayerLowStability);
            m_CombatSystem.PlayerStabilityStateManager.OnHighStability.RemoveListener(m_CombatUIElement.OnPlayerHighStability);
        }

        if (m_CombatSystem.GhostHealthStateManager)
        {
            m_CombatSystem.GhostHealthStateManager.OnLowHealth.RemoveListener(m_CombatUIElement.OnGhostLowHealth);
            m_CombatSystem.GhostHealthStateManager.OnHighHealth.RemoveListener(m_CombatUIElement.OnGhostHighHealth);
        }

        if (m_CombatSystem.PlayerHealthStateManager)
        {
            m_CombatSystem.PlayerHealthStateManager.OnLowHealth.RemoveListener(m_CombatUIElement.OnPlayerLowHealth);
            m_CombatSystem.PlayerHealthStateManager.OnHighHealth.RemoveListener(m_CombatUIElement.OnPlayerHighHealth);
        }
    }

    // void Awake()
    // {
    //     if (m_CombatUIElement.GhostCharImage != null)
    //     {
    //         m_ghostRect = m_CombatUIElement.GhostCharImage.GetComponent<RectTransform>();
    //         m_ghostAnchoredOrig = m_ghostRect.anchoredPosition;
    //     }
    // }

    // void Start()
    // {
    //     if (m_CombatUIElement.GhostCharImage != null)
    //     {
    //         m_GhostPos = m_CombatUIElement.GhostCharImage.transform;
    //         m_GhostPosOri = m_GhostPos.position;
    //     }
    // }

    private void Update()
    {
        if (GameManager.Instance.IsPause) return;
        if (m_CombatSystem != null)
            ChangeRuleTime();
        if (GhostCombatSystem.Instance != null)
        {
            ChangePassiveRuleTime();
            //ChangeStruggleRuleTime();
        }
    }

    private void StartCombat()
    {
        //Debug.Log("StartCombat");
        m_CombatUIElement.GhostHealthMicrobarImageAnim.Activate(m_CombatSystem.CombatDataManager.Ghost.HealthMircobarSystem);
        m_CombatUIElement.GhostStabilityMicrobarImageAnim.Activate(m_CombatSystem.CombatDataManager.Ghost.StabilityMircobarSystem);
        m_CombatUIElement.GhostManaMicrobarAnim.Activate(m_CombatSystem.CombatDataManager.Ghost.ManaMircobarSystem);

        if (m_CombatSystem.CombatDataManager.Ghost && m_CombatSystem.CombatDataManager.Ghost.ObjectMagic)
        {
            m_CombatSystem.CombatDataManager.Ghost.ObjectMagic.OnRecoveryPlus.AddListener(m_CombatUIElement.OnGhostManaRecoveryPlus);
            m_CombatSystem.CombatDataManager.Ghost.ObjectMagic.OnRecoverySpeedInc.AddListener(m_CombatUIElement.OnGhostManaRecoverySpeedIncrease);
        }

        foreach (var item in m_CombatUIElement.ExorcismLetterObjs)
        {
            if (item != null)
                item.SetActive(false);
        }
        foreach (var item in m_CombatUIElement.ExorcismWordObjs)
        {
            if (item != null)
                item.SetActive(false);
        }

        if (m_CombatUIElement.EscapeBtnAnim != null)
            m_CombatUIElement.EscapeBtnAnim.SetBool("Enable", false);

        m_CombatUIElement.OnChangeMode(true);

        // if (m_CombatUIElement.PlayerCharImage != null)
        //     m_PlayerPosX = m_CombatUIElement.PlayerCharImage.transform.position.x;
        //ResetGhostPosition();

        StruggleModeTextBlockMax = m_CombatUIElement.StruggleModeTextBlockAnims.Count;
        StruggleModeBigTextBlockMax = m_CombatUIElement.StruggleModeBigTextBlockAnims.Count;
        StruggleModeBigTextContMax = m_CombatUIElement.StruggleModeBigTextContCGs.Count;

        //ghost prof
        m_CombatUIElement.GhostEmotionSprites.Clear();
        foreach (SpriteWithName item in m_CombatSystem.CombatDataManager.GhostTemplate.SpriteEmotions)
        {
            m_CombatUIElement.GhostEmotionSprites.Add(item);
        }
        m_CombatUIElement.ChangeGhostStatus("Calm");
        //player prof
        m_CombatUIElement.PlayerEmotionSprites.Clear();
        foreach (SpriteWithName item in m_CombatSystem.CombatDataManager.Player2D.SpriteEmotions)
        {
            m_CombatUIElement.PlayerEmotionSprites.Add(item);
        }
        m_CombatUIElement.ChangePlayerStatus("Calm");

        //ghost char
        m_CombatUIElement.GhostCharImage.sprite = m_CombatSystem.CombatDataManager.GhostTemplate.GhostCharSprite;
        //player char
        //m_CombatUIElement.PlayerCharImage.sprite = m_CombatSystem.CombatDataManager.Player2D.PlayerCharSprite;

        ChangeEscapeIcon(m_CombatSystem.CombatDataManager.GhostTemplate.EscapeNum);
    }

    private void OnExitCombat()
    {
        m_CombatUIElement.GhostHealthMicrobarImageAnim.Deactivate();
        m_CombatUIElement.GhostStabilityMicrobarImageAnim.Deactivate();
    }

    public void OnShowGhostPrompt(Sprite appearance, string promptDescription, Sprite promptBoxSprite = null, Sprite promptUISprite = null)
    {
        m_CombatUIElement.OnShowGhostPrompt(appearance, promptDescription, promptBoxSprite, promptUISprite);
    }

    public void ShowAbilityPrompt2(bool isShort, string abilityPrompt2Text, Sprite abilityPrompt2UIImage = null)
    {
        m_CombatUIElement.AbilityPrompt2UIImage.sprite = abilityPrompt2UIImage;
        m_CombatUIElement.AbilityPrompt2Text.text = abilityPrompt2Text;
        m_CombatUIElement.AbilityPrompt2UIAnim.SetTrigger(isShort ? "ShortShow" : "Show");
    }

    public void ChangeCombatUIElement(string name)
    {
        //TODO()
    }

    public void ChangeEscapeIcon(int num)
    {
        foreach (var item in m_CombatUIElement.EscapeIcons)
        {
            item.enabled = false;
        }
        for (int i = 0; i < num; i++)
        {
            m_CombatUIElement.EscapeIcons[i].enabled = true;
        }
    }

    public void ShakeUI()
    {
        Debug.Log("ShakeUI");
        m_UIShakeGroupDOTweenUI.ShakeAll(m_ShakeDuration, m_ShakeStrength);
    }

    public void ShakePlayer(float? customDuration = null, float? customStrength = null)
    {
        Debug.Log("ShakePlayer");
        m_UIShakerDOTween.Shake(m_CombatUIElement.PlayerBoxRect, customDuration, customStrength);
    }

    public void ShakeGhost(float? customDuration = null, float? customStrength = null)
    {
        Debug.Log("ShakeGhost");
        m_UIShakerDOTween.Shake(m_CombatUIElement.GhostBoxRect, customDuration, customStrength);
    }

    public void ShakeTypingUI(float? customDuration = null, float? customStrength = null)
    {
        Debug.Log("ShakeTypingUI");
        m_UIShakerDOTween.Shake(m_CombatUIElement.TypingContainerRect, customDuration, customStrength);
    }

    #region Rule Time
    private void ChangeRuleTime()
    {
        if (m_CombatUIElement.RuleTimeImage != null && m_CombatSystem.ChangeRuleTime > 0)
        {
            m_CombatUIElement.RuleTimeImage.fillAmount = 1f - (m_CombatSystem.ChangeRuleCooldown / m_CombatSystem.ChangeRuleTime);
        }
    }

    private void ChangePassiveRuleTime()
    {
        if (m_CombatUIElement.PassiveRuleTimeImage != null && GhostCombatSystem.Instance.PassiveChangeAbilityTime > 0)
        {
            m_CombatUIElement.PassiveRuleTimeImage.fillAmount = 1f - (GhostCombatSystem.Instance.PassiveChangeAbilityCooldown / GhostCombatSystem.Instance.PassiveChangeAbilityTime);
        }
    }
    #endregion

    #region Struggle Mode
    // private void ChangeStruggleRuleTime()
    // {
    //     if (m_CombatUIElement.StruggleModeRuleTimeImage != null && GhostCombatSystem.Instance.ChangeAbilityTime > 0)
    //     {
    //         m_CombatUIElement.StruggleModeRuleTimeImage.fillAmount = 1f - (GhostCombatSystem.Instance.ChangeAbilityCooldown / GhostCombatSystem.Instance.ChangeAbilityTime);
    //     }
    // }
    public void ShowStruggleMode(bool isTrue)
    {
        m_CombatUIElement.StruggelModeUICG.alpha = isTrue ? 1 : 0;
    }
    #region Normal
    public void ChangeSizeStruggleModeTextCircleImageMinScaleDiff(float num)
    {
        float clampedNum = Mathf.Clamp01(num);  // makes sure num stays between 0 and 1
        float scale = m_CombatUIElement.StruggleModeTextCircleImageScaleStop + clampedNum * m_CombatUIElement.StruggleModeTextCircleImageScaleDiff;
        ChangeSizeStruggleModeTextCircleImage(scale);
    }

    public void ChangeSizeStruggleModeTextCircleImage(float num)
    {
        for (int i = 0; i < m_CombatUIElement.StruggleModeTextBoxes.Count; i++)
        {
            if (m_CombatUIElement.StruggleModeTextBoxes[i].gameObject.activeInHierarchy)
            {
                // var img = m_CombatUIElement.StruggleModeTextCircleImages[i];
                // Debug.Log($"Before scale: {img.rectTransform.localScale}");
                // img.rectTransform.localScale = new Vector3(0.5f, 0.5f, 1f);  // hardcode a small scale
                // Debug.Log($"After scale: {img.rectTransform.localScale}");
                //Debug.Log("scale " + num);
                m_CombatUIElement.StruggleModeTextCircleImages[i].rectTransform.localScale = new Vector3(num, num, 1f);
            }
        }
    }

    public void ChangeStruggleModeText(string word, Color color = default)
    {
        if (color == default) color = Color.black;
        m_CombatUIElement.ChangeStruggleModeText(word, color);
    }
    public void ChangeStruggleModeTexts(List<string> words, Color color = default)
    {
        if (color == default) color = Color.black;
        m_CombatUIElement.ChangeStruggleModeTexts(words, color);
    }
    public void ShowStruggleModeTextBlocks(int num, bool isTrue)
    {
        int count = Math.Min(num, StruggleModeTextBlockMax);
        for (int i = 0; i < count; i++)
        {
            m_CombatUIElement.StruggleModeTextBlockCGs[i].alpha = isTrue ? 1 : 0;
        }
    }
    public void ShowStruggleModeTextBlock(int index, bool isTrue)
    {
        if (index <= StruggleModeBigTextBlockMax)
            m_CombatUIElement.StruggleModeTextBlockCGs[index].alpha = isTrue ? 1 : 0;
    }
    public void SetStruggleModeFlicker(int uIIndex, int flickerNum)
    {
        m_FlickerUIManager.SetFlickerUI(flickerNum, m_CombatUIElement.StruggleModeTextCGs[uIIndex]);
    }

    #endregion
    #region Big

    public void ChangeSizeStruggleModeBigTextCircleImageMinScaleDiff(float num)
    {
        float clampedNum = Mathf.Clamp01(num);  // makes sure num stays between 0 and 1
        float scale = m_CombatUIElement.StruggleModeTextCircleImageScaleStop + clampedNum * m_CombatUIElement.StruggleModeTextCircleImageScaleDiff;
        //Change Size StruggleModeBigTextCircleImage
        m_CombatUIElement.StruggleModeBigTextCircleImage.rectTransform.localScale = new Vector3(scale, scale, 1f);
    }

    public void ChangeStruggleModeBigText(string word, Color color = default)
    {
        if (color == default) color = Color.black;
        m_CombatUIElement.ChangeStruggleModeBigText(word, color);
    }

    public void SetStruggleModeBigFlicker(int flickerNum)
    {
        foreach (CanvasGroup struggleModeBigTextCG in m_CombatUIElement.StruggleModeBigTextCGs)
        {
            m_FlickerUIManager.SetFlickerUI(flickerNum, struggleModeBigTextCG);
        }
    }
    public void ShowStruggleModeBigTextBlocks(bool isTrue)
    {
        foreach (CanvasGroup struggleModeBigTextBlockCG in m_CombatUIElement.StruggleModeBigTextBlockCGs)
        {
            struggleModeBigTextBlockCG.alpha = isTrue ? 1 : 0;
        }
    }
    public void ShowStruggleModeBigTextBlock(int index, bool isTrue)
    {
        if (index <= StruggleModeBigTextBlockMax)
            m_CombatUIElement.StruggleModeBigTextBlockCGs[index].alpha = isTrue ? 1 : 0;
    }
    public void SetStruggleModeBigTextVisibility(int textNum, float alpha)
    {
        m_CombatUIElement.StruggleModeBigTextCGs[textNum].alpha = alpha;
    }
    public void ChangeStruggleModeBigTextBindImages(Sprite sprite)
    {
        foreach (Image struggleModeBigTextBindImage in m_CombatUIElement.StruggleModeBigTextBindImages)
        {
            struggleModeBigTextBindImage.sprite = sprite;
        }
    }
    public void ShowStruggleModeBigTextBindImages(bool isTrue)
    {
        foreach (CanvasGroup struggleModeBigTextBindImageCG in m_CombatUIElement.StruggleModeBigTextBindImageCGs)
        {
            struggleModeBigTextBindImageCG.alpha = isTrue ? 1 : 0;
        }
    }
    public void ShowStruggleModeBigTextBindImage(int index, bool isTrue)
    {
        if (index <= StruggleModeBigTextContMax)
            m_CombatUIElement.StruggleModeBigTextBindImageCGs[index].alpha = isTrue ? 1 : 0;
    }
    #endregion

    #region FightMeter

    public void ShowFightMeter(bool isTrue)
    {
        m_CombatUIElement.ShowFightMeter(isTrue);
    }
    public void ChangeFightMeter(float playerMeter)
    {
        m_CombatUIElement.ChangeFightMeter(playerMeter);
    }
    #endregion

    #endregion

    public void ShowExorcismLetterCoverImage(Sprite sprite)
    {
        m_CombatUIElement.ExorcismLetterCoverImage.sprite = sprite;
        m_CombatUIElement.ExorcismLetterCoverCG.alpha = sprite != null ? 1 : 0;
    }

    public void ChangeDisableKey(List<string> texts)
    {
        m_CombatUIElement.ChangeDisableKey(texts);
    }

    #region Text UI    
    #region Flicker
    public void SetSrambleTextFlickerTime(float num, float maxAlphaTime = 0, float minAlphaTime = 0)
    {
        m_FlickerUIManager.FlickerTime = num;
        m_FlickerUIManager.MaxAlphaTime = maxAlphaTime;
        m_FlickerUIManager.MinAlphaTime = minAlphaTime;
    }
    public void ReseFlickerUI()
    {
        m_FlickerUIManager.ReseFlickerUI();
    }
    public bool CheckFlickerIsBright(int index)
    {
        return m_FlickerUIManager.CheckFlickerIsBright(index);
    }

    public void SetGoodScrambleTextFlicker(int textIndex, int flickerNum)
    {
        m_FlickerUIManager.SetFlickerUI(flickerNum, m_CombatUIElement.GoodScrambleTextCGs[textIndex]);
    }
    public void SetBadScrambleTextFlicker(int textIndex, int flickerNum)
    {
        m_FlickerUIManager.SetFlickerUI(flickerNum, m_CombatUIElement.BadScrambleTextCGs[textIndex]);
    }
    #endregion
    #region Text 
    public void ChangeTopCenterTexts(List<string> words, bool isShow, Color color = default)
    {
        if (color == default) color = Color.white;
        m_CombatUIElement.ChangeTopCenterTexts(words, isShow, color);
    }

    public void ChangeGoodScrambleTexts(List<char> chars, float alpha, Color color = default)
    {
        if (color == default) color = Color.white;
        m_CombatUIElement.ChangeGoodScrambleTexts(chars, alpha, color);
    }

    public void ChangeBadScrambleTexts(List<char> chars, float alpha, Color color = default)
    {
        if (color == default) color = Color.red;
        m_CombatUIElement.ChangeBadScrambleTexts(chars, alpha, color);
    }
    #endregion
    #region Scramble
    public void SetRandomPositionGoodScrambleTexts(int num)
    {
        int count = Mathf.Min(num, m_CombatUIElement.GoodScrambleTexts.Count);
        m_ScrambleUIManager.SetRandomPositionUIs(m_CombatUIElement.GoodScrambleTexts, count);
    }
    public void SetRandomPositionBadScrambleTexts(int num)
    {
        int count = Mathf.Min(num, m_CombatUIElement.BadScrambleTexts.Count);
        m_ScrambleUIManager.SetRandomPositionUIs(m_CombatUIElement.BadScrambleTexts, count);
    }
    #endregion
    #region Mover
    public void InitializeAllMovingTexts(int goodNum, int badNum, float minSpeed, float maxSpeed)
    {
        List<TextMeshProUGUI> scrambleTexts = new();

        if (goodNum > 0 && m_CombatUIElement.GoodScrambleTexts.Count > 0)
        {
            int safeGoodCount = Mathf.Min(goodNum, m_CombatUIElement.GoodScrambleTexts.Count);
            scrambleTexts.AddRange(m_CombatUIElement.GoodScrambleTexts.GetRange(0, safeGoodCount));
        }

        if (badNum > 0 && m_CombatUIElement.BadScrambleTexts.Count > 0)
        {
            int safeBadCount = Mathf.Min(badNum, m_CombatUIElement.BadScrambleTexts.Count);
            scrambleTexts.AddRange(m_CombatUIElement.BadScrambleTexts.GetRange(0, safeBadCount));
        }

        List<RectTransform> scrambleTextRects = new();
        for (int i = 0; i < scrambleTexts.Count; i++)
        {
            scrambleTextRects.Add(scrambleTexts[i].GetComponent<RectTransform>());
        }
        m_MoverUIManager.InitializeMovingUIS(scrambleTextRects, minSpeed, maxSpeed);
    }
    public void InitializeGoodMovingTexts(int num, float minSpeed, float maxSpeed)
    {
        int count = Mathf.Min(num, m_CombatUIElement.GoodScrambleTexts.Count);
        List<RectTransform> scrambleTextRects = new();
        for (int i = 0; i < count; i++)
        {
            scrambleTextRects.Add(m_CombatUIElement.GoodScrambleTexts[i].GetComponent<RectTransform>());
        }
        m_MoverUIManager.InitializeMovingUIS(scrambleTextRects, minSpeed, maxSpeed);
    }
    public void InitializeBadMovingTexts(int num, float minSpeed, float maxSpeed)
    {
        int count = Mathf.Min(num, m_CombatUIElement.BadScrambleTexts.Count);
        List<RectTransform> scrambleTextRects = new();
        for (int i = 0; i < count; i++)
        {
            scrambleTextRects.Add(m_CombatUIElement.BadScrambleTexts[i].GetComponent<RectTransform>());
        }
        m_MoverUIManager.InitializeMovingUIS(scrambleTextRects, minSpeed, maxSpeed);
    }
    #endregion
    #endregion

    // #region Ghost Distance
    // public void OnGhostMove(float percent)
    // {
    //     if (m_ghostRect == null) return;

    //     // percent [0..100] → t [1..0]
    //     float t = 1f - Mathf.Clamp01(percent / 100f);
    //     // compute new anchored X between its original and player X
    //     float targetX = Mathf.Lerp(m_ghostAnchoredOrig.x, m_PlayerPosX, t);

    //     // only touch anchoredPosition (UI space), not world position
    //     var ap = m_ghostRect.anchoredPosition;
    //     ap.x = targetX;
    //     m_ghostRect.anchoredPosition = ap;
    // }
    // // public void OnGhostMove(float percent)
    // // {
    // //     if (m_GhostPos == null) return;

    // //     percent = Mathf.Clamp(percent, 0f, 100f);
    // //     float t = 1f - (percent / 100f);
    // //     float newX = Mathf.Lerp(m_GhostPosOri.x, m_PlayerPosX, t);
    // //     m_GhostPos.position = new Vector3(newX, m_GhostPos.position.y, m_GhostPos.position.z);
    // // }

    // public void ResetGhostPosition()
    // {
    //     if (m_GhostPos != null)
    //         m_GhostPos.position = m_GhostPosOri;
    // }
    // #endregion

    #region Animation

    public void StruggleModeTextCircleAnimation(string name, bool isTrue)
    {
        //Debug.Log("StruggleModeTextCircleAnimation");
        foreach (var item in m_CombatUIElement.StruggleModeTextCircleAnims)
        {
            if (item.gameObject.activeInHierarchy)
                item.SetBool(name, isTrue);
        }
    }

    public void StruggleModeTextBlockAnimation(string name, bool isTrue)
    {
        foreach (Animator animator in m_CombatUIElement.StruggleModeTextBlockAnims)
        {
            animator.SetBool(name, isTrue);
        }
    }

    public void StruggleModeBigTextCircleAnimation(string name, bool isTrue)
    {
        m_CombatUIElement.StruggleModeBigTextCircleAnim.SetBool(name, isTrue);
    }

    public void StruggleModeBigTextBlockAnimation(string name, bool isTrue)
    {
        foreach (Animator animator in m_CombatUIElement.StruggleModeBigTextBlockAnims)
        {
            animator.SetBool(name, isTrue);
        }
    }

    public void TypingPromptContShow(string name, bool isTrue, string text)
    {
        m_CombatUIElement.TypingPromptContText.text = text;
        if (m_CombatUIElement.TypingPromptContAnim != null)
            m_CombatUIElement.TypingPromptContAnim.SetBool(name, isTrue);
    }
    public void TypingPromptContShow(string name, string text)
    {
        m_CombatUIElement.TypingPromptContText.text = text;
        if (m_CombatUIElement.TypingPromptContAnim != null)
            m_CombatUIElement.TypingPromptContAnim.SetTrigger(name);
    }

    public void TypingContainerAnimation(string name)
    {
        if (m_CombatUIElement.TypingContainerAnim != null)
            m_CombatUIElement.TypingContainerAnim.SetTrigger(name);
    }

    public void EffectImageAnimation(string name, bool isTrue)
    {
        if (m_CombatUIElement.EffectAnim != null)
            m_CombatUIElement.EffectAnim.SetBool(name, isTrue);
    }
    public void EffectImageAnimation(string name)
    {
        if (m_CombatUIElement.EffectAnim != null)
            m_CombatUIElement.EffectAnim.SetTrigger(name);
    }
    public void EffectImageAnimation(string name, int num)
    {
        if (m_CombatUIElement.EffectAnim != null)
            m_CombatUIElement.EffectAnim.SetInteger(name, num);
    }
    public void EffectImageAnimation(string name, float num)
    {
        if (m_CombatUIElement.EffectAnim != null)
            m_CombatUIElement.EffectAnim.SetFloat(name, num);
    }

    public void WordContainerEffectImageAnimation(string name)
    {
        if (m_CombatUIElement.WordContainerEffectImageAnim != null)
            m_CombatUIElement.WordContainerEffectImageAnim.SetTrigger(name);
    }
    public void WordContainerEffectImage2Animation(string name)
    {
        if (m_CombatUIElement.WordContainerEffectImage2Anim != null)
            m_CombatUIElement.WordContainerEffectImage2Anim.SetTrigger(name);
    }

    public void WeaknessWordAnimations(string name)
    {
        if (m_CombatUIElement.WeaknessWordAnims == null || m_CombatUIElement.WeaknessWordAnims.Count == 0)
            return;

        foreach (Animator animator in m_CombatUIElement.WeaknessWordAnims)
        {
            if (animator != null && animator.gameObject.activeInHierarchy)
            {
                animator.SetTrigger(name);
            }
        }
    }
    public void WeaknessWordAnimation(string word, string name)
    {
        if (m_CombatUIElement.WeaknessWordAnims == null || m_CombatUIElement.WeaknessWordAnims.Count == 0)
            return;

        for (int i = 0; i < m_CombatUIElement.WeaknessWordObjs.Count; i++)
        {
            if (!m_CombatUIElement.WeaknessWordObjs[i].activeInHierarchy) continue;
            if (m_CombatUIElement.WeaknessWordTexts[i].text == word)
            {
                m_CombatUIElement.WeaknessWordAnims[i].SetTrigger("BlinkTrig");
            }
        }
    }
    public void CurseWordAnimations(string name)
    {
        if (m_CombatUIElement.CurseWordAnims == null || m_CombatUIElement.CurseWordAnims.Count == 0)
            return;

        foreach (Animator animator in m_CombatUIElement.CurseWordAnims)
        {
            if (animator != null && animator.gameObject.activeInHierarchy)
            {
                animator.SetTrigger(name);
            }
        }
    }
    public void CurseWordAnimations(string name, bool isTrue)
    {
        if (m_CombatUIElement.CurseWordAnims == null || m_CombatUIElement.CurseWordAnims.Count == 0)
            return;

        foreach (Animator animator in m_CombatUIElement.CurseWordAnims)
        {
            if (animator != null && animator.gameObject.activeInHierarchy)
            {
                animator.SetBool(name, isTrue);
            }
        }
    }
    public void CurseWordAnimation(string word, string name)
    {
        if (m_CombatUIElement.CurseWordAnims == null || m_CombatUIElement.CurseWordAnims.Count == 0)
            return;

        for (int i = 0; i < m_CombatUIElement.CurseWordObjs.Count; i++)
        {
            if (!m_CombatUIElement.CurseWordObjs[i].activeInHierarchy) continue;
            if (m_CombatUIElement.CurseWordTexts[i].text == word)
            {
                m_CombatUIElement.CurseWordAnims[i].SetTrigger(name);
            }
        }
    }

    public void GhostCharAnimation(string word)
    {
        if (m_CombatUIElement.GhostCharAnim == null) return;
        m_CombatUIElement.GhostCharAnim.SetTrigger(word);
    }
    #endregion

    #region Bool
    public void ChangeIsCheckWordNotExist(bool isTrue)
    {
        m_CombatUIElement.IsCheckWordNotExist = isTrue;
    }
    #endregion
}
