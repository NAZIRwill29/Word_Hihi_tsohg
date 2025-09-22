using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputManagerDetail
{
    public NameDataFlyweight NameData;
    [SerializeField] private InputManagerSO m_InputManagerSO;
    public void Initialize()
    {
        m_InputManagerSO.Initialize();
    }
    public void Disable()
    {
        m_InputManagerSO.Disable();
    }
    public void HandleInput()
    {
        m_InputManagerSO.HandleInput();
    }
    public InputManagerSO GetInputManagerSO()
    {
        return m_InputManagerSO;
    }
}

public class InputSystem : MonoBehaviour
{
    [SerializeField] private ListWrapper<InputManagerDetail> m_InputManagerDetails;
    public InputManagerSO CurrentInputManagerSO;
    private int m_Index = 0;

    void Start()
    {
        CurrentInputManagerSO = m_InputManagerDetails.Items[m_Index]?.GetInputManagerSO();
        CurrentInputManagerSO.Initialize();
    }

    void Update()
    {
        //if (GameManager.Instance.IsPause) return;
        CurrentInputManagerSO.HandleInput();
    }

    public void Change(string name)
    {
        if (m_InputManagerDetails.TryGetIndex(x => x.NameData.Name == name, out int index))
        {
            CurrentInputManagerSO.Disable();
            m_Index = index;
            CurrentInputManagerSO = m_InputManagerDetails.Items[m_Index].GetInputManagerSO();
        }
        CurrentInputManagerSO.Initialize();
    }
}
