using UnityEngine;
using UnityEngine.InputSystem;

public class RebindSaveLoadNew : MonoBehaviour
{
    public InputManagerSO inputManagerSO;

    public void LoadRebind()
    {
        if (inputManagerSO == null) return;

        foreach (var detail in inputManagerSO.InputDetails.Items)
        {
            string json = PlayerPrefs.GetString($"rebind_{detail.Name}");
            if (!string.IsNullOrEmpty(json))
                detail.inputAction.LoadBindingOverridesFromJson(json);
        }
    }

    public void SaveRebind()
    {
        if (inputManagerSO == null) return;

        foreach (var detail in inputManagerSO.InputDetails.Items)
        {
            string json = detail.inputAction.SaveBindingOverridesAsJson();
            PlayerPrefs.SetString($"rebind_{detail.Name}", json);
        }
    }
}
