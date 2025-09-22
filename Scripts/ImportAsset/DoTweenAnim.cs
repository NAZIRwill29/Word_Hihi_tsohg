using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DoTweenAnim : MonoBehaviour
{
    public Image image;
    public Text text;
    public float endValue, duration;
    public Color endColor;
    public Gradient gradient;
    private RectTransform rectTransform;
    public Vector2 endPosV2;
    public bool snapping, fadeOut, addThousandsSeparator, richTextEnabled;
    public float randomness, jumpPower;
    public Vector2 strength;
    public int vibrato, numJumps, fromValue, endValueInt;
    public ShakeRandomnessMode randomnessMode;
    public System.Globalization.CultureInfo culture;
    public string endValueString, scrambleChars;
    public ScrambleMode scrambleMode;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        //FadeAnim();
    }

    #region Image
    public void ImageFade()
    {
        DOTweenModuleUI.DOFade(image, endValue, duration);
    }
    public void ImageColor()
    {
        DOTweenModuleUI.DOColor(image, endColor, duration);
    }
    public void ImageFill()
    {
        DOTweenModuleUI.DOFillAmount(image, endValue, duration);
    }
    public void ImageGradientColor()
    {
        DOTweenModuleUI.DOGradientColor(image, gradient, duration);
    }
    public void ImageBlendableColor()
    {
        DOTweenModuleUI.DOBlendableColor(image, endColor, duration);
    }
    #endregion
    #region RectTransform
    public void AnchorPos()
    {
        DOTweenModuleUI.DOAnchorPos(rectTransform, endPosV2, duration, snapping);
    }
    public void AnchorMax()
    {
        DOTweenModuleUI.DOAnchorMax(rectTransform, endPosV2, duration, snapping);
    }
    public void AnchorMin()
    {
        DOTweenModuleUI.DOAnchorMin(rectTransform, endPosV2, duration, snapping);
    }
    public void Pivot()
    {
        DOTweenModuleUI.DOPivot(rectTransform, endPosV2, duration);
    }
    public void SizeDelta()
    {
        DOTweenModuleUI.DOSizeDelta(rectTransform, endPosV2, duration, snapping);
    }
    public void PunchAnchorPos()
    {
        DOTweenModuleUI.DOPunchAnchorPos(rectTransform, endPosV2, duration);
    }
    public void ShakeAnchorPos()
    {
        //DOShakeAnchorPos(this RectTransform target, float duration, float strength = 100, int vibrato = 10, float randomness = 90, bool snapping = false, bool fadeOut = true, ShakeRandomnessMode randomnessMode = ShakeRandomnessMode.Full)
        DOTweenModuleUI.DOShakeAnchorPos(rectTransform, duration, strength, vibrato, randomness, snapping, fadeOut, randomnessMode);
    }
    #endregion
    #region Special
    public void JumpAnchorPos()
    {
        //float jumpPower, int numJumps, float duration, bool snapping = false)
        DOTweenModuleUI.DOJumpAnchorPos(rectTransform, endPosV2, jumpPower, numJumps, duration, snapping);
    }
    #endregion
    #region Text
    public void TextDOColor()
    {
        DOTweenModuleUI.DOColor(text, endColor, duration);
    }
    public void TextDOCounter()
    {
        //this Text target, int fromValue, int endValue, float duration, bool addThousandsSeparator = true, CultureInfo culture = null
        DOTweenModuleUI.DOCounter(text, fromValue, endValueInt, duration, addThousandsSeparator, culture);
    }
    public void TextDOFade()
    {
        DOTweenModuleUI.DOFade(text, endValue, duration);
    }
    public void TextDOText()
    {
        //(this Text target, string endValue, float duration, bool richTextEnabled = true, ScrambleMode scrambleMode = ScrambleMode.None, string scrambleChars = null)
        DOTweenModuleUI.DOText(text, endValueString, duration, richTextEnabled, scrambleMode, scrambleChars);
    }
    public void TextDOBlendableColor()
    {
        //(this Text target, string endValue, float duration, bool richTextEnabled = true, ScrambleMode scrambleMode = ScrambleMode.None, string scrambleChars = null)
        DOTweenModuleUI.DOBlendableColor(text, endColor, duration);
    }
    #endregion
}
