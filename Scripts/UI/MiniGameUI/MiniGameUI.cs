using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class MiniGameUI : MonoBehaviour
{
    [SerializeField] private MiniGameUIElement m_MiniGameUIElement;
    private int m_LockingWordNeeded, m_LockingwordLengthConditions;

    public void ShowMiniGame(bool isTrue, string name = "")
    {
        CanvasGroupFunc.ModifyCG(m_MiniGameUIElement.MiniGameUICG, isTrue ? 1 : 0, isTrue, isTrue);

        foreach (CanvasGroupWithName item in m_MiniGameUIElement.SectionUICGs)
        {
            CanvasGroupFunc.ModifyCG(item.CanvasGroup, 0, false, false);
        }

        CanvasGroupWithName match = m_MiniGameUIElement.SectionUICGs.Find(x => x.NameData.Name == name);
        if (match != null && match.CanvasGroup != null)
        {
            CanvasGroupFunc.ModifyCG(match.CanvasGroup, isTrue ? 1 : 0, isTrue, isTrue);
        }
        else if (!string.IsNullOrEmpty(name))
        {
            Debug.LogWarning($"MiniGameUI: Section with name '{name}' not found.");
        }
    }

    #region Hide Section

    public void ShowHideSection()
    {
        foreach (GameObject item in m_MiniGameUIElement.HideLetterBoxObjs)
        {
            //CanvasGroupFunc.ModifyCG(item, 0, false, false);
            item.SetActive(false);
        }
    }

    public void ShowHideLetter(int num, bool isTrue, string letter = "")
    {
        if (num < 0 || num >= m_MiniGameUIElement.HideLetterBoxObjs.Count)
        {
            Debug.LogWarning($"[MiniGameUI] Invalid index {num} in ShowHideLetter {m_MiniGameUIElement.HideLetterBoxObjs.Count}");
            return;
        }

        //CanvasGroupFunc.ModifyCG(m_MiniGameUIElement.HideLetterBoxObjs[num], isTrue ? 1 : 0, isTrue, isTrue);
        m_MiniGameUIElement.HideLetterBoxObjs[num].SetActive(isTrue);
        if (num < m_MiniGameUIElement.HideLetterTexts.Count)
            m_MiniGameUIElement.HideLetterTexts[num].text = letter;
        else
            Debug.LogWarning($"[MiniGameUI] Invalid index {num} in ShowHideLetter text {m_MiniGameUIElement.HideLetterTexts.Count}");
    }

    public void SetLetterCircleIndicator(int index, float sizeMax, float sizeMaxExt)
    {
        ChangeSizeHideLetterCircleMaxImageMinScaleDiff(index, sizeMax);
        ChangeSizeHideLetterCircleMaxExtImageMinScaleDiff(index, sizeMaxExt);
    }

    public void HideLetterCircleAnimation(int num, bool isTrue)
    {
        if (num < 0 || num >= m_MiniGameUIElement.HideLetterCircleAnims.Count)
        {
            Debug.LogWarning($"[MiniGameUI] Invalid index {num} in HideLetterCircleAnimation {m_MiniGameUIElement.HideLetterTexts.Count}");
            return;
        }
        if (m_MiniGameUIElement.HideLetterCircleAnims[num].gameObject.activeInHierarchy)
        {
            m_MiniGameUIElement.HideLetterCircleAnims[num].SetBool("Flicker", isTrue);
        }
    }

    public void ChangeSizeHideLetterCircleImageMinScaleDiff(int index, float num)
    {
        if (!IsValidIndex(index)) return;
        if (index < 0 || index >= m_MiniGameUIElement.HideLetterCircleImageScaleDiffs.Count)
        {
            Debug.LogWarning($"[MiniGameUI] Invalid index {index} in ChangeSizeHideLetterCircleImageMinScaleDiff");
            return;
        }

        float scale = GetScaledValue(index, num);
        ChangeSizeHideLetterCircleImage(m_MiniGameUIElement.HideLetterCircleImages[index], scale);
    }

    public void ChangeSizeHideLetterCircleMaxImageMinScaleDiff(int index, float num)
    {
        if (!IsValidIndex(index)) return;
        if (index < 0 || index >= m_MiniGameUIElement.HideLetterCircleImageScaleDiffs.Count)
        {
            Debug.LogWarning($"[MiniGameUI] Invalid index {index} in ChangeSizeHideLetterCircleImageMinScaleDiff");
            return;
        }

        float scale = GetScaledValue(index, num);
        ChangeSizeHideLetterCircleImage(m_MiniGameUIElement.HideLetterCircleMaxImages[index], scale);
    }

    public void ChangeSizeHideLetterCircleMaxExtImageMinScaleDiff(int index, float num)
    {
        if (!IsValidIndex(index)) return;
        if (index < 0 || index >= m_MiniGameUIElement.HideLetterCircleImageScaleDiffs.Count)
        {
            Debug.LogWarning($"[MiniGameUI] Invalid index {index} in ChangeSizeHideLetterCircleImageMinScaleDiff");
            return;
        }

        float scale = GetScaledValue(index, num);
        //Image image = m_MiniGameUIElement.HideLetterCircleMaxExtImages[index];
        // if (image == null)
        //     Debug.LogWarning($"[MiniGameUI] Invalid image {index} in ChangeSizeHideLetterCircleMaxExtImageMinScaleDiff");
        ChangeSizeHideLetterCircleImage(m_MiniGameUIElement.HideLetterCircleMaxExtImages[index], scale);
    }

    private void ChangeSizeHideLetterCircleImage(Image image, float scale)
    {
        if (image != null)
            image.rectTransform.localScale = new Vector3(scale, scale, 1f);
        else
            Debug.LogWarning($"[MiniGameUI] Invalid image in ChangeSizeHideLetterCircleImage");
    }

    private bool IsValidIndex(int index)
    {
        if (index < 0 || index >= m_MiniGameUIElement.HideLetterBoxObjs.Count)
        {
            Debug.LogWarning($"[MiniGameUI] Invalid index {index} in HideLetterBoxObjs {m_MiniGameUIElement.HideLetterBoxObjs.Count}");
            return false;
        }
        return true;
    }

    private float GetScaledValue(int index, float input)
    {
        // if (index >= m_MiniGameUIElement.HideLetterCircleImageScaleDiffs.Count)
        //     return 1f;

        float clamped = Mathf.Clamp01(input);
        return m_MiniGameUIElement.HideLetterCircleImageScaleStop + clamped * m_MiniGameUIElement.HideLetterCircleImageScaleDiffs[index];
    }

    public int GetHideLetterBoxCount()
    {
        return m_MiniGameUIElement.HideLetterBoxObjs.Count;
    }

    public void ChangeHideBar(float num)
    {
        if (m_MiniGameUIElement.HideBarImage != null)
            m_MiniGameUIElement.HideBarImage.fillAmount = Mathf.Clamp01(num);
    }

    #endregion

    #region Lock Section
    public void ShowLockSection(string letters, int wordNeeded, int[] wordLengthConditions)
    {
        m_LockingWordNeeded = wordNeeded;
        m_LockingwordLengthConditions = wordLengthConditions.Length;
        for (int i = 0; i < m_MiniGameUIElement.LockpickWordObjs.Length; i++)
        {
            m_MiniGameUIElement.LockpickWordObjs[i].SetActive(false);
        }
        for (int i = 0; i < m_MiniGameUIElement.LockpickLetterConditionObjs.Length; i++)
        {
            m_MiniGameUIElement.LockpickLetterConditionObjs[i].SetActive(false);
        }
        for (int i = 0; i < letters.Length; i++)
        {
            m_MiniGameUIElement.LockpickLetterCG[i].alpha = 1;
            m_MiniGameUIElement.LockpickLetterTexts[i].text = letters[i].ToString();
        }
        for (int i = 0; i < m_LockingWordNeeded; i++)
        {
            m_MiniGameUIElement.LockpickWordObjs[i].SetActive(true);
            m_MiniGameUIElement.LockpickWordTexts[i].text = "Word " + i + " : ";
        }
        for (int i = 0; i < m_LockingwordLengthConditions; i++)
        {
            m_MiniGameUIElement.LockpickLetterConditionObjs[i].SetActive(true);
            m_MiniGameUIElement.LockpickLetterConditionTexts[i].text = wordLengthConditions[i] + " Letters : ";
        }
        UpdateLockUI();
    }

    public int GetLockLetterBoxCount()
    {
        return m_MiniGameUIElement.LockpickLetterCG.Length;
    }

    public void UpdateLockUI()
    {
        m_MiniGameUIElement.LockBarImage.fillAmount = GameManager.Instance.LockingMechanic.MiniGameLockInteractable.LockHealth / GameManager.Instance.LockingMechanic.MiniGameLockInteractable.StartLockHealth;
        m_MiniGameUIElement.LockpickBarImage.fillAmount = GameManager.Instance.LockingMechanic.LockpickHealth / GameManager.Instance.LockingMechanic.StartLockpickHealth;
        for (int i = 0; i < m_LockingWordNeeded; i++)
        {
            m_MiniGameUIElement.LockpickWordTexts[i].text += GameManager.Instance.LockingMechanic.Words;
        }
        for (int i = 0; i < m_LockingwordLengthConditions; i++)
        {
            m_MiniGameUIElement.LockpickLetterConditionTexts[i].text += GameManager.Instance.LockingMechanic.WordFitConditions;
        }
    }
    #endregion
}
