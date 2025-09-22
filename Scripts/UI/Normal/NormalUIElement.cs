using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NormalUIElement : MonoBehaviour
{
    public CanvasGroup PlayerHealthCG;
    public CanvasGroup InteractIndicatorCG;
    public Image InteractIndicatorImage;
    public CanvasGroup StaminaBoxCG;
    public Image StaminaBarRightImage, StaminaBarLeftImage;
    public List<SpriteWithName> InteractIndicatorSprites;

    [Header("Menu")]
    public Animator MenuAnim;
    public Animator[] MenuSectionAnims;
    [Header("Inventory")]
    public GameObject[] InventoryItemSlotObjs;
    public Image[] InventoryItemIconImages;
    public TextMeshProUGUI[] InventoryItemQuantityTexts;
    public Animator InventoryItemUIAnim;
    public Image InventoryItemImage;
    public TextMeshProUGUI InventoryItemNameText, InventoryItemDescText;
    public CanvasGroup InventoryButtonContainerCG;
    public Button InventoryPositiveBtn, InventoryNegativeBtn;
    public TextMeshProUGUI InventoryPositiveText { get; set; }
    public TextMeshProUGUI InventoryNegativeText { get; set; }

    [Header("PopUp")]
    public Animator PopUpAnim;
    public Image PopUpCenterBoxImage, PopUpCenterImage;
    public TextMeshProUGUI PopUpCenterCountText { get; set; }
    public Image PopUpDescTextContImage;
    public TextMeshProUGUI PopUpNameText;
    public TextMeshProUGUI PopUpDescText;
    public GameObject PopUpButtonContainerObj;
    public Button PopUpNegativeBtn, PopUpPositiveBtn;
    public TextMeshProUGUI PopUpNegativeBtnText { get; set; }
    public TextMeshProUGUI PopUpPositiveBtnText { get; set; }

    [Header("Other")]
    public Animator OverlayAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (InventoryPositiveBtn)
            InventoryPositiveText = InventoryPositiveBtn.GetComponentInChildren<TextMeshProUGUI>();
        if (InventoryNegativeBtn)
            InventoryNegativeText = InventoryNegativeBtn.GetComponentInChildren<TextMeshProUGUI>();
        if (PopUpCenterBoxImage)
            PopUpCenterCountText = PopUpCenterBoxImage.GetComponentInChildren<TextMeshProUGUI>();
        if (PopUpNegativeBtn)
            PopUpNegativeBtnText = PopUpNegativeBtn.GetComponentInChildren<TextMeshProUGUI>();
        if (PopUpPositiveBtn)
            PopUpPositiveBtnText = PopUpPositiveBtn.GetComponentInChildren<TextMeshProUGUI>();
    }

    #region Menu
    public void ShowMenu(bool isTrue)
    {
        // for (int i = 1; i < MenuSectionAnims.Length; i++)
        // {
        //     MenuSectionAnims[i].SetBool("Show", false);
        // }
        MenuAnim.SetBool("Show", isTrue);
        //MenuSectionAnims[0].Animator.SetBool("Show", isTrue);
    }
    public void OnMenuSectChange(int num)
    {
        for (int i = 0; i < MenuSectionAnims.Length; i++)
        {
            bool isShow = i == num;
            MenuSectionAnims[i].SetBool("Show", isShow);
            // Debug.Log("OnMenuSectChange " + i + " " + isShow);
        }
    }
    #endregion

    #region PopUp
    public void ShowPopUp(bool isTrue)
    {
        PopUpAnim.SetBool("Show", isTrue);
    }
    #endregion
}
