using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FlickerUI
{
    public CanvasGroup CanvasGroup;
    public int FlickerNum;
    public int TempFlickerNum;
    public float Alpha;
    public bool Increase;
    public float FlickerCooldown;
    public float MaxAlphaCooldown;
    public float MinAlphaCooldown;
}

public class FlickerUIManager : MonoBehaviour
{
    [SerializeField] private CombatUI m_CombatUI;
    public List<FlickerUI> FlickerUIs = new List<FlickerUI>();
    public float FlickerTime = 1f;
    public float MaxAlphaTime = 0.2f;
    public float MinAlphaTime = 0.2f;
    [SerializeField] private float m_FlickerInc = 1f;   // Alpha units per second
    [SerializeField] private float m_MinAlpha = 0.2f;
    [SerializeField] private float m_MaxAlpha = 1f;

    void Update()
    {
        if (GameManager.Instance.IsPause) return;
        if (FlickerUIs == null || FlickerUIs.Count == 0) return;

        float delta = Time.deltaTime;
        for (int i = 0; i < FlickerUIs.Count; i++)
        {
            var ui = FlickerUIs[i];
            if (ui.CanvasGroup == null)
                continue;

            // Wait at full alpha after each half-cycle
            if (ui.MaxAlphaCooldown > 0f)
            {
                ui.MaxAlphaCooldown -= delta;
                continue;
            }
            // Wait at none alpha after each half-cycle
            if (ui.MinAlphaCooldown > 0f)
            {
                ui.MinAlphaCooldown -= delta;
                continue;
            }

            // If still flickering cycles
            if (ui.TempFlickerNum > 0)
            {
                // Pulsate alpha up/down
                ui.Alpha += (ui.Increase ? m_FlickerInc : -m_FlickerInc) * delta;
                if (ui.Alpha <= m_MinAlpha)
                {
                    ui.Alpha = m_MinAlpha;
                    ui.Increase = true;
                    ui.TempFlickerNum--;
                    ui.MinAlphaCooldown = MinAlphaTime;
                }
                else if (ui.Alpha >= m_MaxAlpha)
                {
                    ui.Alpha = m_MaxAlpha;
                    ui.Increase = false;
                    ui.TempFlickerNum--;
                    ui.MaxAlphaCooldown = MaxAlphaTime;
                }

                ui.CanvasGroup.alpha = ui.Alpha;

                // Start cooldown when done all flicker cycles
                if (ui.TempFlickerNum <= 0)
                    ui.FlickerCooldown = FlickerTime;
            }
            // Cooling down between flicker bursts
            else if (ui.FlickerCooldown > 0f)
            {
                ui.FlickerCooldown -= delta;
                if (ui.FlickerCooldown <= 0f)
                {
                    // Reset for next burst
                    ui.TempFlickerNum = ui.FlickerNum * 2;
                    ui.Alpha = m_MaxAlpha;
                    ui.Increase = false;
                    ui.CanvasGroup.alpha = ui.Alpha;

                    m_CombatUI.OnFlickerStartBright?.Invoke(i);
                }
            }
        }
    }

    /// <summary>
    /// Configure flicker parameters for a specific UI element
    /// </summary>
    public void SetFlickerUI(int flickerNum, CanvasGroup canvasGroup)
    {
        FlickerUI flickerUI = new()
        {
            CanvasGroup = canvasGroup,
            FlickerNum = flickerNum,
            TempFlickerNum = flickerNum * 2,
            Alpha = m_MaxAlpha,
            Increase = false,
            FlickerCooldown = 0f,
            MaxAlphaCooldown = 0f,
        };
        flickerUI.CanvasGroup.alpha = flickerUI.Alpha;

        FlickerUIs.Add(flickerUI);
    }

    public void ReseFlickerUI()
    {
        FlickerUIs.Clear();
    }

    public bool CheckFlickerIsBright(int index)
    {
        if (FlickerUIs.Count == 0) return false;
        return FlickerUIs[index].Alpha > m_MinAlpha + 0.1f;
    }
}
