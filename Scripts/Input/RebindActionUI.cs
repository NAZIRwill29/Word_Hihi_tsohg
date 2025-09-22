using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindActionUI : MonoBehaviour
{
    [SerializeField] private InputManagerSO inputManagerSO;
    [SerializeField] private NameDataFlyweight m_InputDetailNameData; // Match by name

    private InputDetail _inputDetail;

    // UI Components
    [SerializeField] private TextMeshProUGUI m_ActionLabel;
    [SerializeField] private TextMeshProUGUI m_BindingText;
    [SerializeField] private GameObject m_RebindOverlay;
    [SerializeField] private TextMeshProUGUI m_RebindText;

    private InputActionRebindingExtensions.RebindingOperation m_RebindOperation;

    private void Awake()
    {
        if (inputManagerSO != null)
            _inputDetail = inputManagerSO.InputDetails.Items.Find(i => i.Name == m_InputDetailNameData.Name);
    }

    // private void Start()
    // {
    //     UpdateBindingDisplay();
    // }

    public void StartInteractiveRebind()
    {
        if (_inputDetail?.inputAction == null)
        {
            Debug.LogWarning($"InputDetail not found for '{m_InputDetailNameData.Name}'");
            return;
        }

        var action = _inputDetail.inputAction;
        int bindingIndex = 0; // Only supports first binding; expand if needed

        m_RebindOperation?.Cancel();
        action.Disable();

        m_RebindOperation = action.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(op =>
            {
                action.Enable();
                m_RebindOverlay?.SetActive(false);
                UpdateBindingDisplay();
                m_RebindOperation.Dispose();
            })
            .OnCancel(op =>
            {
                action.Enable();
                m_RebindOverlay?.SetActive(false);
                UpdateBindingDisplay();
                m_RebindOperation.Dispose();
            });

        m_RebindOverlay?.SetActive(true);
        m_RebindText.text = "Waiting for input...";
        m_RebindOperation.Start();
    }

    public void UpdateBindingDisplay()
    {
        if (_inputDetail?.inputAction != null && m_BindingText != null)
        {
            string displayString = _inputDetail.inputAction.GetBindingDisplayString();
            m_BindingText.text = displayString;
        }

        if (m_ActionLabel != null)
        {
            m_ActionLabel.text = m_InputDetailNameData.Name;
        }
    }

    public void ResetToDefault()
    {
        _inputDetail?.inputAction?.RemoveAllBindingOverrides();
        UpdateBindingDisplay();
    }
}
