using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable]
public class InputDetail
{
    [SerializeField] private string m_Name;
    public NameDataFlyweight NameDataFlyweight;
    public string Name
    {
        get => NameDataFlyweight != null ? NameDataFlyweight.Name : string.Empty;
        set
        {
            if (NameDataFlyweight != null)
            {
                NameDataFlyweight.Name = value;
            }
            m_Name = value;  // Store locally in case NameDataFlyweight is null
        }
    }

    [Range(0, 100)] public float m_CooldownTime = 0.5f;
    [HideInInspector] public float m_Cooldown;

    public InputAction inputAction = new();
    public UnityEvent Action;
}

[CreateAssetMenu(fileName = "InputManagerSO", menuName = "Input/InputManagerSO", order = 1)]
public class InputManagerSO : ScriptableObject
{
    public ListWrapper<InputDetail> InputDetails = new(); // Ensure initialization

    [Header("Controls")]
    [Tooltip("Use WASD keys to move")]
    public InputAction MoveAction;

    public void Initialize()
    {
        //LoadRebinds(); // <- Load on init
        MoveAction.Enable();
        foreach (var item in InputDetails.Items)
        {
            item.inputAction.Enable();
        }
    }

    public void Disable()
    {
        MoveAction.Disable();
        foreach (var item in InputDetails.Items)
        {
            item.inputAction.Disable();
        }
    }

    public void HandleInput()
    {
        foreach (var item in InputDetails.Items)
        {
            if (item.inputAction == null) continue;

            item.m_Cooldown -= Time.deltaTime;
            if (item.m_Cooldown > 0) continue;

            if (item.inputAction.IsPressed())
            {
                item.Action?.Invoke();
                item.m_Cooldown = item.m_CooldownTime;
            }
        }
    }

    // // ✅ Save all bindings
    // public void SaveRebinds()
    // {
    //     // Save MoveAction
    //     string moveJson = MoveAction.SaveBindingOverridesAsJson();
    //     PlayerPrefs.SetString("rebind_MoveAction", moveJson);

    //     // Save each InputDetail's InputAction
    //     for (int i = 0; i < InputDetails.Items.Count; i++)
    //     {
    //         var detail = InputDetails.Items[i];
    //         if (detail.inputAction != null)
    //         {
    //             string key = $"rebind_InputDetail_{detail.Name}";
    //             string json = detail.inputAction.SaveBindingOverridesAsJson();
    //             PlayerPrefs.SetString(key, json);
    //         }
    //     }

    //     PlayerPrefs.Save(); // Ensure it's written
    // }

    // // ✅ Load all bindings
    // public void LoadRebinds()
    // {
    //     // Load MoveAction
    //     if (PlayerPrefs.HasKey("rebind_MoveAction"))
    //     {
    //         string moveJson = PlayerPrefs.GetString("rebind_MoveAction");
    //         MoveAction.LoadBindingOverridesFromJson(moveJson);
    //     }

    //     // Load each InputDetail's InputAction
    //     foreach (var detail in InputDetails.Items)
    //     {
    //         if (detail.inputAction != null)
    //         {
    //             string key = $"rebind_InputDetail_{detail.Name}";
    //             if (PlayerPrefs.HasKey(key))
    //             {
    //                 string json = PlayerPrefs.GetString(key);
    //                 detail.inputAction.LoadBindingOverridesFromJson(json);
    //             }
    //         }
    //     }
    // }
}

