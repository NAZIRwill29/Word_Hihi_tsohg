using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatUIElement : MonoBehaviour
{
    public Animator WordContainerAnim, WordContainerEffectImageAnim, WordContainerEffectImage2Anim;
    public Animator EscapeBtnAnim;
    public Animator PlayerStabilityAnim, PlayerHealthAnim, GhostStabilityAnim, GhostHealthAnim;
    public Image BackgroundImage;

    [Header("Ghost Combat")]
    public Image GhostCharImage;
    public Animator GhostCharAnim;

    [Header("TextUI")]
    public List<TextMeshProUGUI> TopCenterTexts;
    public List<TextMeshProUGUI> GoodScrambleTexts;
    public List<CanvasGroup> GoodScrambleTextCGs { get; set; } = new();
    public List<TextMeshProUGUI> BadScrambleTexts;
    public List<CanvasGroup> BadScrambleTextCGs { get; set; } = new();

    [Header("Typing")]
    public Animator TypingContainerAnim;
    public RectTransform TypingContainerRect;
    public TextMeshProUGUI TypingText;
    public Animator TypingPromptContAnim;
    public Image TypingPromptContImage;
    public TextMeshProUGUI TypingPromptContText;
    public List<GameObject> ExorcismWordObjs;
    public List<TextMeshProUGUI> ExorcismWordTexts { get; set; } = new();

    [Header("ExorcismLetter")]
    public List<GameObject> ExorcismLetterObjs;
    public List<TextMeshProUGUI> ExorcismLetterTexts { get; set; } = new();
    public Image ExorcismLetterCoverImage;
    public CanvasGroup ExorcismLetterCoverCG { get; set; }

    [Header("WeaknessWord")]
    public List<GameObject> WeaknessWordObjs;
    public List<TextMeshProUGUI> WeaknessWordTexts { get; set; } = new();
    public List<Animator> WeaknessWordAnims { get; set; } = new();
    public List<GameObject> CurseWordObjs;
    public List<TextMeshProUGUI> CurseWordTexts { get; set; } = new();
    public List<Animator> CurseWordAnims { get; set; } = new();

    [Header("Rule")]
    public Image RuleTimeImage;
    public TextMeshProUGUI LetterPlacementText;
    public TextMeshProUGUI LetterRuleText;
    public Image PassiveRuleTimeImage;
    public TextMeshProUGUI PassiveRuleText;

    [Header("Escape")]
    public TextMeshProUGUI EscapeWordText;
    public Image[] EscapeIcons;

    [Header("Profile")]
    public RectTransform GhostBoxRect;
    public RectTransform PlayerBoxRect;
    public Image GhostProfImage;
    public Image PlayerProfImage;
    // public Rect GhostProfRec, GhostHealthRec, GhostStabilityRec, GhostManaRec;
    // public Rect PlayerProfRec, PlayerHealthRec, PlayerStabilityRec, PlayerManaRec;
    public MicrobarImageAnim GhostHealthMicrobarImageAnim, GhostStabilityMicrobarImageAnim, GhostManaMicrobarAnim;
    public Animator ManaBorderAnim;
    public TextMeshProUGUI GhostEmotionText, PlayerEmotionText;
    public ListWrapper<SpriteWithName> GhostEmotionSprites = new();// { get; set; } = new();
    public ListWrapper<SpriteWithName> PlayerEmotionSprites = new();// { get; set; } = new();

    [Header("GhostPrompt")]
    public Animator GhostPromptUIAnim;
    public Image GhostPromptUIImage, GhostPromptTextContImage, GhostPromptCenterImage;
    public CanvasGroup GhostPromptTextContCG { get; set; }
    public TextMeshProUGUI GhostPromptText;

    [Header("AbilityPrompt")]
    public Animator AbilityPromptUIAnim;
    public Image AbilityPromptUIImage, AbilityPromptTextContImage, AbilityPromptCenterImage;
    public CanvasGroup AbilityPromptTextContCG { get; set; }
    public TextMeshProUGUI AbilityPromptText;

    [Header("AbilityPrompt2")]
    public Animator AbilityPrompt2UIAnim;
    public Image AbilityPrompt2UIImage, AbilityPrompt2TextContImage;
    public CanvasGroup AbilityPrompt2TextContCG { get; set; }
    public TextMeshProUGUI AbilityPrompt2Text;

    [Header("effect")]
    public Animator EffectAnim;
    public TextMeshProUGUI ExorciseWordEffectText;

    [Header("StruggleMode")]
    public Image StruggleModeBackgroundImage;
    public Image StruggleModeTextContImage;
    public Image StruggleModeCenterImage;
    public CanvasGroup StruggelModeUICG, StruggleModeTextContCG;
    public CanvasGroup StruggleModeCenterCG { get; set; }
    //normal
    public List<Image> StruggleModeTextBoxes, StruggleModeTextCircleImages;
    public List<TextMeshProUGUI> StruggleModeTexts;
    public List<CanvasGroup> StruggleModeTextCGs { get; set; } = new();
    public List<Animator> StruggleModeTextCircleAnims { get; set; } = new();
    public List<Animator> StruggleModeTextBlockAnims;
    public List<CanvasGroup> StruggleModeTextBlockCGs { get; set; } = new();
    //big
    public Image StruggleModeBigTextBox, StruggleModeBigTextCircleImage;
    public CanvasGroup StruggleModeBigTextBoxCG { get; set; }
    public Animator StruggleModeBigTextCircleAnim { get; set; }
    public List<CanvasGroup> StruggleModeBigTextContCGs = new();
    public List<Image> StruggleModeBigTextBindImages = new();
    public List<CanvasGroup> StruggleModeBigTextBindImageCGs { get; set; } = new();
    public List<TextMeshProUGUI> StruggleModeBigTexts = new();
    public List<CanvasGroup> StruggleModeBigTextCGs { get; set; } = new();
    public List<Animator> StruggleModeBigTextBlockAnims;
    public List<CanvasGroup> StruggleModeBigTextBlockCGs { get; set; } = new();
    //fight meter
    public CanvasGroup FightMeterCG;
    public Image FightMeterGhostProf, FightMeterPlayerProf;
    public Image FightMeterGhostMeter, FightMeterPlayerMeter;
    //other
    public Image StruggleModeRuleTimeImage;
    public TextMeshProUGUI StruggleModeRuleText;
    public float StruggleModeTextCircleImageScaleDiff { get; set; }
    public float StruggleModeTextCircleImageScaleStop = 0.7f;

    [Header("DisableKey")]
    public CanvasGroup DisableKeyUICG;
    public List<Image> DisableKeyTextContImages;
    public List<TextMeshProUGUI> DisableKeyTexts { get; set; } = new();

    [Header("DisableKey2")]
    public CanvasGroup DisableKey2UICG;
    public List<Image> DisableKey2TextContImages;
    public List<TextMeshProUGUI> DisableKey2Texts { get; set; } = new();

    [Header("Other")]
    public bool IsCheckWordNotExist = true;

    private void Start()
    {
        //Exorcism letter
        foreach (var item in ExorcismLetterObjs)
        {
            if (item != null && item.GetComponentInChildren<TextMeshProUGUI>() != null)
                ExorcismLetterTexts.Add(item.GetComponentInChildren<TextMeshProUGUI>());
        }

        //weakness word
        foreach (var item in WeaknessWordObjs)
        {
            if (item != null && item.GetComponentInChildren<TextMeshProUGUI>() != null)
                WeaknessWordTexts.Add(item.GetComponentInChildren<TextMeshProUGUI>());
            if (item != null && item.GetComponentInChildren<Animator>() != null)
                WeaknessWordAnims.Add(item.GetComponentInChildren<Animator>());
        }
        foreach (var item in CurseWordObjs)
        {
            if (item != null && item.GetComponentInChildren<TextMeshProUGUI>() != null)
                CurseWordTexts.Add(item.GetComponentInChildren<TextMeshProUGUI>());
            if (item != null && item.GetComponentInChildren<Animator>() != null)
                CurseWordAnims.Add(item.GetComponentInChildren<Animator>());
        }

        //typing
        foreach (var item in ExorcismWordObjs)
        {
            if (item != null && item.GetComponentInChildren<TextMeshProUGUI>() != null)
                ExorcismWordTexts.Add(item.GetComponentInChildren<TextMeshProUGUI>());
        }

        //StruggleModeText
        foreach (var item in StruggleModeTextCircleImages)
        {
            if (item != null && item.GetComponent<Animator>() != null)
                StruggleModeTextCircleAnims.Add(item.GetComponent<Animator>());
        }
        foreach (var item in StruggleModeTexts)
        {
            if (item != null && item.GetComponent<CanvasGroup>() != null)
                StruggleModeTextCGs.Add(item.GetComponent<CanvasGroup>());
        }
        foreach (var item in StruggleModeTextBlockAnims)
        {
            if (item != null && item.GetComponent<CanvasGroup>() != null)
                StruggleModeTextBlockCGs.Add(item.GetComponent<CanvasGroup>());
        }

        //StruggleModeBigText
        // foreach (var item in StruggleModeBigTextContCGs)
        // {
        //     if (item != null && item.GetComponentInChildren<TextMeshProUGUI>() != null)
        //         StruggleModeBigTexts.Add(item.GetComponentInChildren<TextMeshProUGUI>());
        //     if (item != null && item.GetComponentInChildren<Image>() != null)
        //         StruggleModeBigTextBindImages.Add(item.GetComponentInChildren<Image>());
        // }
        foreach (var item in StruggleModeBigTexts)
        {
            if (item != null && item.GetComponent<CanvasGroup>() != null)
                StruggleModeBigTextCGs.Add(item.GetComponent<CanvasGroup>());
        }
        foreach (var item in StruggleModeBigTextBindImages)
        {
            if (item != null && item.GetComponent<CanvasGroup>() != null)
                StruggleModeBigTextBindImageCGs.Add(item.GetComponent<CanvasGroup>());
        }
        foreach (var item in StruggleModeBigTextBlockAnims)
        {
            if (item != null && item.GetComponent<CanvasGroup>() != null)
                StruggleModeBigTextBlockCGs.Add(item.GetComponent<CanvasGroup>());
        }

        //DisableKeyText
        foreach (var item in DisableKeyTextContImages)
        {
            if (item != null && item.GetComponentInChildren<TextMeshProUGUI>() != null)
                DisableKeyTexts.Add(item.GetComponentInChildren<TextMeshProUGUI>());
        }
        foreach (var item in DisableKey2TextContImages)
        {
            if (item != null && item.GetComponentInChildren<TextMeshProUGUI>() != null)
                DisableKey2Texts.Add(item.GetComponentInChildren<TextMeshProUGUI>());
        }

        //TEXT UI
        foreach (var item in GoodScrambleTexts)
        {
            if (item != null && item.GetComponentInChildren<CanvasGroup>() != null)
                GoodScrambleTextCGs.Add(item.GetComponentInChildren<CanvasGroup>());
        }
        foreach (var item in BadScrambleTexts)
        {
            if (item != null && item.GetComponentInChildren<CanvasGroup>() != null)
                BadScrambleTextCGs.Add(item.GetComponentInChildren<CanvasGroup>());
        }

        // if (TypingContainerAnim != null)
        //     TypingContainerRect = ExorcismLetterCoverImage.GetComponent<RectTransform>();
        if (ExorcismLetterCoverImage != null)
            ExorcismLetterCoverCG = ExorcismLetterCoverImage.GetComponent<CanvasGroup>();
        if (AbilityPromptTextContImage != null)
            AbilityPromptTextContCG = AbilityPromptTextContImage.GetComponent<CanvasGroup>();
        if (AbilityPrompt2TextContImage != null)
            AbilityPrompt2TextContCG = AbilityPrompt2TextContImage.GetComponent<CanvasGroup>();
        if (StruggleModeTextContImage != null)
            StruggleModeTextContCG = StruggleModeTextContImage.GetComponent<CanvasGroup>();
        if (StruggleModeBigTextCircleImage != null)
            StruggleModeBigTextCircleAnim = StruggleModeBigTextCircleImage.GetComponent<Animator>();
        if (StruggleModeBigTextBox != null)
            StruggleModeBigTextBoxCG = StruggleModeBigTextBox.GetComponent<CanvasGroup>();

        StruggleModeTextCircleImageScaleDiff = 1 - StruggleModeTextCircleImageScaleStop;
    }

    #region GhostPrompt
    public void OnStartGhostPrompt(GhostTemplate ghostTemplate)
    {
        Debug.Log("OnStartGhostPrompt");
        SetDetailGhostPrompt(ghostTemplate.Appearance, ghostTemplate.PromptDescription, ghostTemplate.PromptBoxSprite, ghostTemplate.PromptUISprite);
        GhostPromptUIAnim.SetTrigger("Start");
    }
    public void OnShowGhostPrompt(GhostTemplate ghostTemplate)
    {
        SetDetailGhostPrompt(ghostTemplate.Appearance, ghostTemplate.PromptDescription, ghostTemplate.PromptBoxSprite, ghostTemplate.PromptUISprite);
        GhostPromptUIAnim.SetTrigger("Show");
    }
    public void OnShowGhostPrompt(Sprite appearance, string promptDescription, Sprite promptBoxSprite = null, Sprite promptUISprite = null)
    {
        SetDetailGhostPrompt(appearance, promptDescription, promptBoxSprite, promptUISprite);
        GhostPromptUIAnim.SetTrigger("Show");
    }
    public void OnHideGhostPrompt()
    {
        GhostPromptUIAnim.SetTrigger("Hide");
    }
    public void OnStopGhostPrompt()
    {
        GhostPromptUIAnim.SetTrigger("Stop");
    }

    private void SetDetailGhostPrompt(Sprite appearance, string promptDescription, Sprite promptBoxSprite = null, Sprite promptUISprite = null)
    {
        GhostPromptCenterImage.sprite = appearance;
        GhostPromptText.text = promptDescription;

        if (promptBoxSprite)
            GhostPromptTextContImage.sprite = promptBoxSprite;

        GhostPromptUIImage.sprite = promptUISprite;
    }
    #endregion

    #region OnAction
    public void OnChangeWordBar(int num)
    {
        if (WordContainerAnim != null)
            WordContainerAnim.SetInteger("Index", num);
    }

    public void OnChangeMode(bool isTrue)
    {
        if (TypingContainerAnim != null)
            TypingContainerAnim.SetBool("TypingMode", isTrue);
    }

    public void OnWordNotExist(string word)
    {
        if (TypingContainerAnim != null && IsCheckWordNotExist)
            TypingContainerAnim.SetTrigger("WordFail");
    }

    public void OnFailedWordNotDuplicate(string word)
    {
        TypingPromptContText.text = "Too many " + word;
        TypingPromptContAnim.SetTrigger("FlickerTrig");
    }

    // public void OnGhostMovePlus(float num)
    // {
    //     if (num >= 0) return;
    //     if (EffectAnim != null)
    //         EffectAnim.SetTrigger("BlinkTrig");
    // }

    // public void OnGhostSpeedIncrease(float num)
    // {
    //     GhostCharAnim.SetTrigger(num < 0 ? "IncSpeed" : "DecSpeed");
    // }

    public void OnGhostManaRecoveryPlus(float num)
    {
        GhostCharAnim.SetTrigger(num > 0 ? "Inc" : "Dec");
        // if (EffectAnim == null) return;
        // if (num >= 0)
        //     EffectAnim.SetTrigger("BlinkTrig");
    }

    public void OnGhostManaRecoverySpeedIncrease(bool isIncrease)
    {
        GhostCharAnim.SetTrigger(isIncrease ? "IncSpeed" : "DecSpeed");
        ManaBorderAnim.SetBool("Show", isIncrease);
    }

    public void OnWeaknessWordChange(List<string> words)
    {
        foreach (GameObject weaknessWordObj in WeaknessWordObjs)
        {
            weaknessWordObj.SetActive(false);
        }
        for (int i = 0; i < words.Count; i++)
        {
            WeaknessWordObjs[i].SetActive(true);
            WeaknessWordTexts[i].text = words[i];
        }
    }
    public void OnCurseWordChange(List<string> words)
    {
        foreach (GameObject curseWordObj in CurseWordObjs)
        {
            curseWordObj.SetActive(false);
        }
        for (int i = 0; i < words.Count; i++)
        {
            CurseWordObjs[i].SetActive(true);
            CurseWordTexts[i].text = words[i];
        }
    }
    public void OnExorciseWeaknessWord(string word)
    {
        ExorciseWordEffectText.text = word;
        EffectAnim.SetTrigger("ExorciseWeaknessWord");
    }
    public void OnExorciseOrdinaryWord(string word)
    {
        ExorciseWordEffectText.text = word;
        EffectAnim.SetTrigger("ExorciseOrdinaryWord");
    }
    public void OnExorciseCurseWord(string word)
    {
        ExorciseWordEffectText.text = word;
        EffectAnim.SetTrigger("ExorciseCurseWord");
    }

    public void OnGhostLowStability(string name)
    {
        ChangeGhostStatus(name);
    }
    public void OnGhostHighStability(string name)
    {
        ChangeGhostStatus(name);
    }
    public void ChangeGhostStatus(string name)
    {
        Debug.Log("ChangeGhostStatus");
        GhostEmotionText.text = name;
        if (GhostEmotionSprites.TryGetValue(x => x.NameData.Name == name, out SpriteWithName spriteWithName))
            GhostProfImage.sprite = spriteWithName.Sprite;
    }

    public void OnPlayerLowStability(string name)
    {
        ChangePlayerStatus(name);
    }
    public void OnPlayerHighStability(string name)
    {
        ChangePlayerStatus(name);
    }
    public void ChangePlayerStatus(string name)
    {
        PlayerEmotionText.text = name;
        if (PlayerEmotionSprites.TryGetValue(x => x.NameData.Name == name, out SpriteWithName spriteWithName))
            PlayerProfImage.sprite = spriteWithName.Sprite;
    }

    public void OnPlayerLowHealth()
    {
        PlayerStabilityAnim.SetBool("Flicker", true);
    }
    public void OnPlayerHighHealth()
    {
        PlayerStabilityAnim.SetBool("Flicker", false);
    }
    public void OnGhostLowHealth()
    {
        GhostStabilityAnim.SetBool("Flicker", true);
    }
    public void OnGhostHighHealth()
    {
        GhostStabilityAnim.SetBool("Flicker", false);
    }
    #endregion

    public void ChangeTypingText(string word)
    {
        if (TypingText != null)
            TypingText.text = word;
    }

    public void ChangeExorcismTypingText(string word)
    {
        int count = Mathf.Min(ExorcismWordObjs.Count, ExorcismWordTexts.Count);
        for (int i = 0; i < count; i++)
        {
            bool shouldShow = i < word.Length;
            if (ExorcismWordObjs[i] != null)
                ExorcismWordObjs[i].SetActive(shouldShow);
            if (shouldShow && ExorcismWordTexts[i] != null)
                ExorcismWordTexts[i].text = word[i].ToString();
        }
    }

    public void ChangeLetterPlacement(string letter)
    {
        if (LetterPlacementText != null)
            LetterPlacementText.text = $"Exorcism Letter : {letter}";
    }

    public void SetExorcismLetter(List<char> letters)
    {
        int count = Mathf.Min(ExorcismLetterObjs.Count, ExorcismLetterTexts.Count);
        for (int i = 0; i < count; i++)
        {
            bool shouldShow = i < letters.Count;
            if (ExorcismLetterObjs[i] != null)
                ExorcismLetterObjs[i].SetActive(shouldShow);
            if (shouldShow && ExorcismLetterTexts[i] != null)
                ExorcismLetterTexts[i].text = letters[i].ToString();
        }
    }

    public void SetLetterRule(string index, string letter)
    {
        if (LetterRuleText != null)
            LetterRuleText.text = $"Letter Rule : {letter} in {index}";
    }

    public void SetEscapeWord(string word)
    {
        if (EscapeWordText != null)
            EscapeWordText.text = word;
    }

    public void EscapeWordSuccess()
    {
        if (EscapeBtnAnim != null)
            EscapeBtnAnim.SetBool("Enable", true);
    }

    #region Struggle Mode
    //normal    
    public void ChangeStruggleModeText(string word, Color color = default)
    {
        if (color == default) color = Color.black;

        for (int i = 0; i < StruggleModeTexts.Count; i++)
        {
            StruggleModeTextBoxes[i].gameObject.SetActive(false);
        }
        if (String.IsNullOrEmpty(word)) return;

        StruggleModeTextBoxes[0].gameObject.SetActive(true);
        StruggleModeTexts[0].text = word;
        StruggleModeTexts[0].color = color;
    }

    public void ChangeStruggleModeTexts(List<string> words, Color color = default)
    {
        if (color == default) color = Color.black;

        for (int i = 0; i < StruggleModeTexts.Count; i++)
        {
            StruggleModeTextBoxes[i].gameObject.SetActive(false);
        }

        int count = Math.Min(StruggleModeTexts.Count, words.Count);

        for (int i = 0; i < count; i++)
        {
            StruggleModeTextBoxes[i].gameObject.SetActive(true);
            StruggleModeTexts[i].text = words[i];
            StruggleModeTexts[i].color = color;
        }
    }

    //big
    public void ChangeStruggleModeBigText(string word, Color color = default)
    {
        if (color == default) color = Color.black;

        if (String.IsNullOrEmpty(word))
        {
            StruggleModeBigTextBoxCG.alpha = 0;
            return;
        }
        StruggleModeBigTextBoxCG.alpha = 1;

        foreach (CanvasGroup struggleModeBigTextContCG in StruggleModeBigTextContCGs)
        {
            struggleModeBigTextContCG.alpha = 0;
        }

        int count = Math.Min(StruggleModeBigTexts.Count, word.Length);

        for (int i = 0; i < count; i++)
        {
            StruggleModeBigTextContCGs[i].alpha = 1;
            StruggleModeBigTexts[i].text = word[i].ToString();
            StruggleModeBigTexts[i].color = color;
        }
    }

    //fight meter
    public void ShowFightMeter(bool isTrue)
    {
        if (isTrue)
        {
            FightMeterGhostProf.sprite = GhostProfImage.sprite;
            FightMeterPlayerProf.sprite = PlayerProfImage.sprite;
            FightMeterGhostMeter.fillAmount = 0.5f;
            FightMeterPlayerMeter.fillAmount = 0.5f;
        }
        FightMeterCG.alpha = isTrue ? 1 : 0;
    }
    public void ChangeFightMeter(float playerMeter)
    {
        FightMeterPlayerMeter.fillAmount = playerMeter;
        FightMeterGhostMeter.fillAmount = 1 - playerMeter;
    }
    #endregion

    public void ChangeDisableKey(List<string> texts)
    {
        if (texts.Count == 0)
        {
            DisableKeyUICG.alpha = 0;
            DisableKey2UICG.alpha = 0;
        }
        else if (texts.Count <= 3)
        {
            DisableKeyUICG.alpha = 1;
            DisableKey2UICG.alpha = 0;

            foreach (var item in DisableKeyTextContImages)
            {
                item.gameObject.SetActive(false);
            }
            for (int i = 0; i < texts.Count; i++)
            {
                DisableKeyTextContImages[i].gameObject.SetActive(true);
                DisableKeyTexts[i].text = texts[i];
            }
        }
        else
        {
            DisableKeyUICG.alpha = 0;
            DisableKey2UICG.alpha = 1;
            int count = Math.Min(texts.Count, 6);

            foreach (var item in DisableKey2TextContImages)
            {
                item.gameObject.SetActive(false);
            }
            for (int i = 0; i < count; i++)
            {
                DisableKey2TextContImages[i].gameObject.SetActive(true);
                DisableKey2Texts[i].text = texts[i];
            }
        }
    }

    #region TextUi
    public void ChangeTopCenterTexts(List<string> words, bool isShow, Color color)
    {
        if (words == null || words.Count == 0 || !isShow)
        {
            foreach (var topCenterText in TopCenterTexts)
                topCenterText.gameObject.SetActive(false);
            return;
        }

        foreach (var topCenterText in TopCenterTexts)
            topCenterText.gameObject.SetActive(false);

        int count = Mathf.Min(words.Count, TopCenterTexts.Count);

        for (int i = 0; i < count; i++)
        {
            TopCenterTexts[i].gameObject.SetActive(isShow);
            TopCenterTexts[i].text = words[i];
            TopCenterTexts[i].color = color;
        }
    }

    public void ChangeBadScrambleTexts(List<char> chars, float alpha, Color color)
    {
        if (chars == null || chars.Count == 0)
        {
            foreach (CanvasGroup scrambleTextCG in BadScrambleTextCGs)
                scrambleTextCG.alpha = 0;
            return;
        }

        foreach (CanvasGroup scrambleTextCG in BadScrambleTextCGs)
            scrambleTextCG.alpha = 0;

        int count = Mathf.Min(chars.Count, BadScrambleTexts.Count);

        for (int i = 0; i < count; i++)
        {
            BadScrambleTexts[i].text = chars[i].ToString();
            BadScrambleTexts[i].color = color;
            BadScrambleTextCGs[i].alpha = alpha;
        }
    }

    public void ChangeGoodScrambleTexts(List<char> chars, float alpha, Color color)
    {
        if (chars == null || chars.Count == 0)
        {
            foreach (CanvasGroup scrambleTextCG in GoodScrambleTextCGs)
                scrambleTextCG.alpha = 0;
            return;
        }

        foreach (CanvasGroup scrambleTextCG in GoodScrambleTextCGs)
            scrambleTextCG.alpha = 0;

        int count = Mathf.Min(chars.Count, GoodScrambleTexts.Count);

        for (int i = 0; i < count; i++)
        {
            GoodScrambleTexts[i].text = chars[i].ToString();
            GoodScrambleTexts[i].color = color;
            GoodScrambleTextCGs[i].alpha = alpha;
        }
    }
    #endregion

    #region OnAbility
    public void OnActiveAbility(ActiveAbilityData data)
    {
        if (data == null) return;

        if (StruggelModeUICG != null)
            StruggelModeUICG.alpha = 1;

        if (StruggleModeRuleText != null)
            StruggleModeRuleText.text = data.RuleText;

        if (StruggleModeCenterImage != null)
            StruggleModeCenterImage.sprite = data.GhostProfSprite;

        if (StruggleModeBackgroundImage != null)
            StruggleModeBackgroundImage.sprite = data.BackgroundSprite;

        if (StruggleModeTexts.Count > 0)
        {
            ChangeStruggleModeTexts(data.Texts);
        }

        PlayerHealthAnim.SetBool("Flicker", true);

        if (!string.IsNullOrEmpty(data.EffectImageAnimationConstant))
        {
            // You can trigger animations here if needed
        }

        ManageAbilityPromptUI(data);
    }

    public void OnPassiveAbility(PassiveAbilityData data)
    {
        PlayerHealthAnim.SetBool("Flicker", false);
        if (data == null) return;

        if (StruggelModeUICG != null)
            StruggelModeUICG.alpha = 0;

        if (PassiveRuleText != null)
            PassiveRuleText.text = data.RuleText;

        if (GhostProfImage != null && data.GhostProfSprite != null)
            GhostProfImage.sprite = data.GhostProfSprite;

        if (GhostCharImage != null && data.GhostCharSprite != null)
            GhostCharImage.sprite = data.GhostCharSprite;

        if (ExorcismLetterCoverImage != null)
        {
            if (data.ExorcismLetterCoverSprites.Count > 0)
            {
                ExorcismLetterCoverImage.sprite = data.ExorcismLetterCoverSprites[0];
                ExorcismLetterCoverCG.alpha = data.ExorcismLetterCoverSprites[0] != null ? 1 : 0;
            }
            else
                ExorcismLetterCoverCG.alpha = 0;
        }

        if (!string.IsNullOrEmpty(data.EffectImageAnimationConstant))
        {
            // You can trigger animations here if needed
        }

        ManageAbilityPromptUI(data);
    }

    private void ManageAbilityPromptUI(AbilityPassData data)
    {
        if (data.GhostProfSprite != null)
        {
            if (AbilityPromptCenterImage != null)
                AbilityPromptCenterImage.sprite = data.GhostProfSprite;

            if (AbilityPromptText != null)
                AbilityPromptText.text = data.RuleText;

            if (data.AbilityPromptTextContSprite != null && AbilityPromptTextContImage)
                AbilityPromptTextContImage.sprite = data.AbilityPromptTextContSprite;

            if (data.AbilityPromptUISprite != null && AbilityPromptUIImage)
                AbilityPromptUIImage.sprite = data.AbilityPromptUISprite;

            if (AbilityPromptUIAnim != null)
                AbilityPromptUIAnim.SetTrigger("Show");
        }
        else
        {
            if (AbilityPrompt2Text != null)
                AbilityPrompt2Text.text = data.RuleText;

            if (data.AbilityPromptTextContSprite != null && AbilityPrompt2TextContImage)
                AbilityPrompt2TextContImage.sprite = data.AbilityPromptTextContSprite;

            if (data.AbilityPromptUISprite != null && AbilityPrompt2UIImage)
                AbilityPrompt2UIImage.sprite = data.AbilityPromptUISprite;

            if (AbilityPrompt2UIAnim != null)
                AbilityPrompt2UIAnim.SetTrigger("Show");
        }
    }
    #endregion
}
