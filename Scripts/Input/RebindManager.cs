using UnityEngine;
using UnityEngine.InputSystem;

public class RebindManager : MonoBehaviour
{
    public InputManagerSO inputManagerSO;
    public RebindActionUI[] RebindActionUIs;
    public RebindMoveActionUI[] RebindMoveActionUIs;

    private void Start()
    {
        LoadRebind();

        // Update all binding displays
        foreach (RebindActionUI item in RebindActionUIs)
        {
            item.UpdateBindingDisplay();
        }
        foreach (RebindMoveActionUI item in RebindMoveActionUIs)
        {
            item.UpdateBindingDisplay();
        }
    }

    public void LoadRebind()
    {
        if (inputManagerSO == null) return;

        // Load MoveAction
        if (PlayerPrefs.HasKey("rebind_MoveAction"))
        {
            string moveJson = PlayerPrefs.GetString("rebind_MoveAction");
            inputManagerSO.MoveAction.LoadBindingOverridesFromJson(moveJson);
        }

        // Load InputDetails
        foreach (var detail in inputManagerSO.InputDetails.Items)
        {
            string key = $"rebind_InputDetail_{detail.Name}";
            if (PlayerPrefs.HasKey(key))
            {
                string json = PlayerPrefs.GetString(key);
                detail.inputAction.LoadBindingOverridesFromJson(json);
            }
        }
    }

    public void SaveRebind()
    {
        if (inputManagerSO == null) return;

        // Save MoveAction
        string moveJson = inputManagerSO.MoveAction.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebind_MoveAction", moveJson);

        // Save InputDetails
        foreach (var detail in inputManagerSO.InputDetails.Items)
        {
            string key = $"rebind_InputDetail_{detail.Name}";
            string json = detail.inputAction.SaveBindingOverridesAsJson();
            PlayerPrefs.SetString(key, json);
        }

        PlayerPrefs.Save(); // Ensure it's written
    }

    public void ResetToDefault()
    {
        foreach (RebindActionUI item in RebindActionUIs)
        {
            item.ResetToDefault();
        }
        foreach (RebindMoveActionUI item in RebindMoveActionUIs)
        {
            item.ResetToDefault();
        }
    }
}
