using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class RebindMoveActionUI : MonoBehaviour
{
    [SerializeField] private InputManagerSO inputManagerSO;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI bindingDisplayText;
    [SerializeField] private GameObject rebindOverlay;
    [SerializeField] private TextMeshProUGUI rebindPromptText;

    [Header("Binding Part Name")]
    [SerializeField] private string bindingPartName = "up"; // up/down/left/right

    private InputActionRebindingExtensions.RebindingOperation rebindOperation;

    // private void Start()
    // {
    //     UpdateBindingDisplay();
    // }

    public void StartRebind()
    {
        if (inputManagerSO == null || inputManagerSO.MoveAction == null)
        {
            Debug.LogWarning("InputManagerSO or MoveAction not set.");
            return;
        }

        int bindingIndex = FindBindingIndex(bindingPartName);
        if (bindingIndex == -1)
        {
            Debug.LogWarning($"Binding part '{bindingPartName}' not found in MoveAction.");
            return;
        }

        inputManagerSO.MoveAction.Disable();
        rebindOperation?.Cancel();

        rebindOperation = inputManagerSO.MoveAction
            .PerformInteractiveRebinding(bindingIndex)
            .OnMatchWaitForAnother(0.1f)
            .OnCancel(op =>
            {
                inputManagerSO.MoveAction.Enable();
                rebindOverlay?.SetActive(false);
                UpdateBindingDisplay();
                op.Dispose();
            })
            .OnComplete(op =>
            {
                inputManagerSO.MoveAction.Enable();
                rebindOverlay?.SetActive(false);
                UpdateBindingDisplay();
                op.Dispose();
            });

        rebindPromptText.text = $"Rebinding '{bindingPartName}'...";
        rebindOverlay?.SetActive(true);
        rebindOperation.Start();
    }

    private int FindBindingIndex(string partName)
    {
        var bindings = inputManagerSO.MoveAction.bindings;
        for (int i = 0; i < bindings.Count; i++)
        {
            if (bindings[i].isPartOfComposite && bindings[i].name.ToLower() == partName.ToLower())
                return i;
        }
        return -1;
    }

    public void UpdateBindingDisplay()
    {
        int index = FindBindingIndex(bindingPartName);
        if (index != -1 && bindingDisplayText != null)
        {
            string display = inputManagerSO.MoveAction.GetBindingDisplayString(index);
            bindingDisplayText.text = display;
        }
    }

    public void ResetToDefault()
    {
        inputManagerSO.MoveAction.RemoveAllBindingOverrides();
        UpdateBindingDisplay();
    }
}
