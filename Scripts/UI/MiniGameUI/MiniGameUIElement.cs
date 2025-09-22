using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameUIElement : MonoBehaviour
{
    public CanvasGroup MiniGameUICG;
    public List<CanvasGroupWithName> SectionUICGs;

    [Header("Hide")]
    public Image HideBarImage;
    public List<GameObject> HideLetterBoxObjs;
    public List<Image> HideLetterCircleImages;
    public List<Image> HideLetterCircleMinImages;
    public List<Image> HideLetterCircleMaxImages;
    public List<Image> HideLetterCircleMaxExtImages;
    public List<TextMeshProUGUI> HideLetterTexts;
    //public CanvasGroup[] HideLetterTextCGs;
    public List<Animator> HideLetterCircleAnims;
    [Tooltip("it = 1 - HideLetterCircleImageScaleStop")] public List<float> HideLetterCircleImageScaleDiffs = new();
    public float HideLetterCircleImageScaleStop = 0.7f;

    [Header("Lockpick")]
    public Image LockpickBarImage;
    public Image LockBarImage;
    public CanvasGroup[] LockpickLetterCG;
    public TextMeshProUGUI[] LockpickLetterTexts;
    public GameObject[] LockpickLetterConditionObjs;
    public List<TextMeshProUGUI> LockpickLetterConditionTexts { get; set; } = new();
    public GameObject[] LockpickWordObjs;
    public List<TextMeshProUGUI> LockpickWordTexts { get; set; } = new();
    public TextMeshProUGUI LockpickTypingText;
    public Animator LockpickTypingPromptAnim;

    void Start()
    {
        for (int i = 0; i < LockpickLetterConditionObjs.Length; i++)
        {
            LockpickLetterConditionTexts.Add(LockpickLetterConditionObjs[i].GetComponentInChildren<TextMeshProUGUI>());
        }
        for (int i = 0; i < LockpickWordObjs.Length; i++)
        {
            LockpickWordTexts.Add(LockpickWordObjs[i].GetComponentInChildren<TextMeshProUGUI>());
        }
    }

}
