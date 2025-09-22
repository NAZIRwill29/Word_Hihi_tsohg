using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIElement : MonoBehaviour
{
    [Header("MenuUI")]
    public CanvasGroup MenuUICG;

    [Header("UI")]
    public AnimatorWithName[] AnimatorWithNames;

    [Header("GameOver")]
    public Image GameOverUI;
    public Image GameOverImage;
    public TextMeshProUGUI GameOverDescText;

    [Header("Controller")]
    public CanvasGroup[] ControllerModeCGs;

    void Start()
    {
    }

    #region Controller
    public void OnControllerModeChange(int num)
    {
        for (int i = 0; i < ControllerModeCGs.Length; i++)
        {
            bool isShow = i == num;
            CanvasGroupFunc.ModifyCG(ControllerModeCGs[i], isShow ? 1 : 0, isShow, isShow);
        }
    }
    #endregion
}
