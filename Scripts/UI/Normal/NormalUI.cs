using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NormalUI : MonoBehaviour
{
    [SerializeField] private NormalSystem m_NormalSystem;
    [SerializeField] private NormalUIElement m_NormalUIElement;
    [SerializeField] private InteractManagerSO m_InteractManagerSO;
    [SerializeField] private UIFollow2D m_InteractIndicatorUIFollow2D;
    [SerializeField] private MultiButtonSelector m_MenuSectMultiButtonSelector;
    [SerializeField] private Inventory m_Inventory;
    private bool m_IsShowMenu;
    [SerializeField] private ActionControllerSO m_UseItemActionControllerSO;
    [SerializeField] private ActionControllerSO m_CloseActionControllerSO;
    [SerializeField] private GameObjectActionControllerSO m_DropItemActionControllerSO;

    void OnEnable()
    {
        if (m_InteractManagerSO)
        {
            m_InteractManagerSO.OnCanInteract.AddListener(OnCanInteract);
            m_InteractManagerSO.OnInteract.AddListener(OnInteract);
            m_InteractManagerSO.OnShowItemPopUp.AddListener(OnShowPopUp);
            m_InteractManagerSO.OnClosePopUp.AddListener(OnClosePopUp);
        }
        if (m_MenuSectMultiButtonSelector)
            m_MenuSectMultiButtonSelector.OnButtonSelected.AddListener(m_NormalUIElement.OnMenuSectChange);
        if (m_Inventory)
            m_Inventory.OnInventoryChange.AddListener(OnInventoryChange);
        if (m_UseItemActionControllerSO)
            m_UseItemActionControllerSO.UnityEvent.AddListener(UseItem);
        if (m_DropItemActionControllerSO)
            m_DropItemActionControllerSO.UnityEventGameObject.AddListener(DropItem);
        if (m_CloseActionControllerSO)
            m_CloseActionControllerSO.UnityEvent.AddListener(OnClosePopUp);
    }

    void OnDisable()
    {
        if (m_InteractManagerSO)
        {
            m_InteractManagerSO.OnCanInteract.RemoveListener(OnCanInteract);
            m_InteractManagerSO.OnInteract.RemoveListener(OnInteract);
            m_InteractManagerSO.OnShowItemPopUp.RemoveListener(OnShowPopUp);
            m_InteractManagerSO.OnClosePopUp.RemoveListener(OnClosePopUp);
        }
        if (m_MenuSectMultiButtonSelector)
            m_MenuSectMultiButtonSelector.OnButtonSelected.RemoveListener(m_NormalUIElement.OnMenuSectChange);
        if (m_Inventory)
            m_Inventory.OnInventoryChange.RemoveListener(OnInventoryChange);
        if (m_UseItemActionControllerSO)
            m_UseItemActionControllerSO.UnityEvent.RemoveListener(UseItem);
        if (m_DropItemActionControllerSO)
            m_DropItemActionControllerSO.UnityEventGameObject.RemoveListener(DropItem);
        if (m_CloseActionControllerSO)
            m_CloseActionControllerSO.UnityEvent.RemoveListener(OnClosePopUp);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    #region Menu
    public void OnShowMenu()
    {
        m_IsShowMenu = !m_IsShowMenu;
        m_NormalUIElement.ShowMenu(m_IsShowMenu);
        m_MenuSectMultiButtonSelector.SelectButton(0);
        InventoryBar();
    }

    public void OnMenuBarTab()
    {
        m_MenuSectMultiButtonSelector.BarTab();
    }

    public void InitInventoryBar()
    {
        m_MenuSectMultiButtonSelector.SelectButton(0);
        InventoryBar();
    }
    public void InitWordBar()
    {
        m_MenuSectMultiButtonSelector.SelectButton(1);
        WordBar();
    }
    public void InitMapBar()
    {
        m_MenuSectMultiButtonSelector.SelectButton(2);
        MapBar();
    }
    public void InitDictionaryBar()
    {
        m_MenuSectMultiButtonSelector.SelectButton(3);
        DictionaryBar();
    }

    #region Inventory
    public void InventoryBar()
    {
        m_NormalSystem.CurrentState = "Inventory";
        OnInventoryChange();
    }
    public void OnInventoryChange()
    {
        foreach (var item in m_NormalUIElement.InventoryItemSlotObjs)
        {
            item.gameObject.SetActive(false);
        }

        for (int i = 0; i < m_Inventory.Items.Count; i++)
        {
            m_NormalUIElement.InventoryItemSlotObjs[i].SetActive(true);
            m_NormalUIElement.InventoryItemIconImages[i].sprite = m_Inventory.Items[i].ItemDataFlyweight.Icon;
            //m_NormalUIElement.InventoryItemNameTexts[i].text = m_Inventory.Items[i].ItemDataFlyweight.Name;
            m_NormalUIElement.InventoryItemQuantityTexts[i].text = m_Inventory.Items[i].quantity.ToString();
        }
    }
    //USED() - in itembutton
    public void InspectItem(int num)
    {
        m_NormalSystem.CurrentState = "Inspect";
        ItemDataFlyweight itemDataFlyweight = m_Inventory.InspectItem(num);

        m_NormalUIElement.InventoryItemUIAnim.SetBool("Show", true);
        //CanvasGroupFunc.ModifyCG(m_NormalUIElement.InventoryItemUIAnim, 1, true, true);
        m_NormalUIElement.InventoryItemImage.sprite = itemDataFlyweight.Icon;
        m_NormalUIElement.InventoryItemNameText.text = itemDataFlyweight.NameData.Name;
        m_NormalUIElement.InventoryItemDescText.text = itemDataFlyweight.Description;

        if (itemDataFlyweight is DropableItemSO dropableItemSO)
        {
            CanvasGroupFunc.ModifyCG(m_NormalUIElement.InventoryButtonContainerCG, 1, true, true);

            m_NormalUIElement.InventoryNegativeBtn.gameObject.SetActive(true);
            m_NormalUIElement.InventoryNegativeText.text = dropableItemSO.NegativeText;

            if (dropableItemSO is ConsumableItemSO consumableItemSO)
            {
                m_NormalUIElement.InventoryPositiveBtn.gameObject.SetActive(true);
                m_NormalUIElement.InventoryPositiveText.text = consumableItemSO.PositiveInItemText;
            }
            else
                m_NormalUIElement.InventoryPositiveBtn.gameObject.SetActive(false);
        }
        else
            CanvasGroupFunc.ModifyCG(m_NormalUIElement.InventoryButtonContainerCG, 0, false, false);
    }
    //USED() - in itembutton
    public void CloseInspectItem()
    {
        if (m_NormalSystem.CurrentState != "Inspect") return;
        m_NormalSystem.CurrentState = "Inventory";
        m_NormalUIElement.InventoryItemUIAnim.SetBool("Show", false);
    }
    public void UseItem()
    {
        if (m_NormalSystem.CurrentState == "Inspect")
            m_Inventory.UseItem();
    }
    public void DropItem(GameObject gameObject)
    {
        if (m_NormalSystem.CurrentState == "Inspect")
            m_Inventory.DropItem(gameObject);
    }
    #endregion
    #region Word
    public void WordBar()
    {
        m_NormalSystem.CurrentState = "Word";
    }
    #endregion
    #region Map
    public void MapBar()
    {
        m_NormalSystem.CurrentState = "Map";
    }
    #endregion
    #region Dictionary
    public void DictionaryBar()
    {
        m_NormalSystem.CurrentState = "Dictionary";
    }
    #endregion
    #endregion

    #region PopUp
    private void OnShowPopUp(Item item)
    {
        m_NormalUIElement.ShowPopUp(true);
        m_NormalUIElement.PopUpNameText.text = item.ItemDataFlyweight.NameData.Name;
        m_NormalUIElement.PopUpCenterImage.sprite = item.ItemDataFlyweight.Icon;
        m_NormalUIElement.PopUpCenterCountText.text = item.quantity.ToString();
        m_NormalUIElement.PopUpDescText.text = item.ItemDataFlyweight.Description;
        m_NormalUIElement.PopUpNegativeBtn.gameObject.SetActive(true);
        m_NormalUIElement.PopUpPositiveBtn.gameObject.SetActive(true);
        m_NormalUIElement.PopUpNegativeBtnText.text = item.ItemDataFlyweight.NegativeText + " (" + m_NormalSystem.NegativeInputDetail.inputAction.GetBindingDisplayString() + ")";
        m_NormalUIElement.PopUpPositiveBtnText.text = item.ItemDataFlyweight.PositiveText + " (" + m_NormalSystem.PositiveInputDetail.inputAction.GetBindingDisplayString() + ")";
    }
    public void OnShowPopUp(string name, Sprite icon, string quantity, string description)
    {
        m_NormalUIElement.ShowPopUp(true);
        m_NormalUIElement.PopUpNameText.text = name;
        m_NormalUIElement.PopUpCenterImage.sprite = icon;
        m_NormalUIElement.PopUpCenterCountText.text = quantity;
        m_NormalUIElement.PopUpDescText.text = description;
        m_NormalUIElement.PopUpNegativeBtn.gameObject.SetActive(true);
        m_NormalUIElement.PopUpPositiveBtn.gameObject.SetActive(false);
        m_NormalUIElement.PopUpNegativeBtnText.text = "Close (" + m_NormalSystem.NegativeInputDetail.inputAction.GetBindingDisplayString() + ")";
    }
    public void OnClosePopUp()
    {
        m_NormalUIElement.ShowPopUp(false);
    }
    #endregion

    #region Other
    public void OverlayAnimation(bool isShow, int num)
    {
        m_NormalUIElement.OverlayAnimator.SetBool("Show", isShow);
        m_NormalUIElement.OverlayAnimator.SetInteger("State", num);
    }
    #endregion

    private void OnCanInteract(string interactName, bool isTrue)
    {
        m_NormalUIElement.InteractIndicatorCG.alpha = isTrue ? 1 : 0;
        if (isTrue)
            m_NormalUIElement.InteractIndicatorImage.sprite = m_NormalUIElement.InteractIndicatorSprites.Find(x => x.NameData.Name == interactName).Sprite;

        // m_NormalUIElement.PlayerHealthCG.alpha = isTrue ? 1 : 0;
        // m_NormalUIElement.StaminaBoxCG.alpha = isTrue ? 1 : 0;
    }

    private void OnInteract(string interactName)
    {
        m_NormalUIElement.InteractIndicatorImage.sprite = m_NormalUIElement.InteractIndicatorSprites.Find(x => x.NameData.Name == interactName).Sprite;
        switch (interactName)
        {
            case "UnHide":
                m_NormalUIElement.PlayerHealthCG.alpha = 0;
                m_NormalUIElement.StaminaBoxCG.alpha = 0;
                break;
            case "Hide":
                m_NormalUIElement.PlayerHealthCG.alpha = 1;
                m_NormalUIElement.StaminaBoxCG.alpha = 1;
                break;
            case "Inspect":
                m_NormalUIElement.PlayerHealthCG.alpha = 0;
                m_NormalUIElement.StaminaBoxCG.alpha = 0;
                m_NormalUIElement.InteractIndicatorCG.alpha = 0;
                break;
            case "UnInspect":
                m_NormalUIElement.PlayerHealthCG.alpha = 1;
                m_NormalUIElement.StaminaBoxCG.alpha = 1;
                m_NormalUIElement.InteractIndicatorCG.alpha = 1;
                break;
            default:
                break;
        }
    }

    public void ChangeTargetUIFollow(Transform transform)
    {
        m_InteractIndicatorUIFollow2D.target = transform;
    }
}
