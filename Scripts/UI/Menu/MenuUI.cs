using UnityEngine;
using UnityEngine.UI; // Needed for Image and Text
using TMPro; // If you're using TextMeshPro

public class MenuUI : MonoBehaviour
{
    [SerializeField] private MenuUIElement m_MenuUIElement;

    [Header("Controller")]
    [SerializeField] private MultiButtonSelector m_ControllerMultiButtonSelector;

    void OnEnable()
    {
        m_ControllerMultiButtonSelector.OnButtonSelected.AddListener(m_MenuUIElement.OnControllerModeChange);
    }

    void OnDisable()
    {
        m_ControllerMultiButtonSelector.OnButtonSelected.RemoveListener(m_MenuUIElement.OnControllerModeChange);
    }

    void Update()
    {
        // If not used, you can safely remove this method to save overhead.
    }

    public void ShowWindowInMenuUI(string name, bool isShow)
    {
        if (m_MenuUIElement == null) return;

        CanvasGroupFunc.ModifyCG(m_MenuUIElement.MenuUICG, isShow ? 1 : 0, isShow, isShow);
        GameManager.Instance.Pause(isShow);

        foreach (var anim in m_MenuUIElement.AnimatorWithNames)
        {
            if (anim == null || anim.Animator == null || anim.NameData == null) continue;

            bool shouldShow = anim.NameData.Name == name && isShow;
            anim.Animator.SetBool("Show", shouldShow);
        }
    }

    #region GameOverUI
    public void ShowGameOver(bool isTrue, Sprite gameOverImage = null, string gameOverDescText = null, Sprite gameOverUI = null)
    {
        ShowWindowInMenuUI("GameOverUI", isTrue);

        if (!isTrue) return;

        if (m_MenuUIElement == null) return;

        if (gameOverUI != null && m_MenuUIElement.GameOverUI != null)
            m_MenuUIElement.GameOverUI.sprite = gameOverUI;

        if (m_MenuUIElement.GameOverImage != null)
            m_MenuUIElement.GameOverImage.sprite = gameOverImage;

        if (m_MenuUIElement.GameOverDescText != null)
            m_MenuUIElement.GameOverDescText.text = gameOverDescText ?? "";
    }
    #endregion

    public void SaveBackBtn()
    {
        //TODO() - save
    }
}
