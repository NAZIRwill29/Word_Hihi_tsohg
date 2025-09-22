using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class MultiButtonSelector : MonoBehaviour
{
    [Header("Button Group (Drag Buttons Here)")]
    public List<Button> buttons = new List<Button>();

    [Header("Events")]
    public UnityEvent<int> OnButtonSelected; // Passes selected button index

    private int currentSelectedIndex = -1;

    void Start()
    {
        SetupButtons();
    }

    public void SetupButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i; // Local copy for closure
            buttons[i].onClick.AddListener(() => SelectButton(index));
        }

        // Optionally select the first button by default
        if (buttons.Count > 0)
            SelectButton(0);
    }

    public void SelectButton(int index)
    {
        if (index < 0 || index >= buttons.Count)
            return;

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].interactable = i != index;
        }

        currentSelectedIndex = index;
        OnButtonSelected.Invoke(index);
    }

    public int GetSelectedIndex() => currentSelectedIndex;

    public void BarTab()
    {
        currentSelectedIndex++;
        if (currentSelectedIndex >= buttons.Count)
            currentSelectedIndex = 0;
        SelectButton(currentSelectedIndex);
    }
}
