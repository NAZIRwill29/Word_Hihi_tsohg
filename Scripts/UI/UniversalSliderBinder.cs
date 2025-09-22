using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class FloatEvent : UnityEvent<float> { }

[System.Serializable]
public class IntEvent : UnityEvent<int> { }

public class UniversalSliderBinder : MonoBehaviour
{
    [Header("Slider Reference")]
    [SerializeField] private Slider slider;

    [Header("Mode")]
    [SerializeField] private bool useWholeNumbers = false;

    [Header("Events")]
    public FloatEvent onValueChangedFloat;
    public IntEvent onValueChangedInt;

    private bool isSettingValueFromCode = false;

    void Awake()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }

        slider.wholeNumbers = useWholeNumbers;
        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void OnSliderChanged(float value)
    {
        if (isSettingValueFromCode) return;

        if (useWholeNumbers)
            onValueChangedInt.Invoke(Mathf.RoundToInt(value));
        else
            onValueChangedFloat.Invoke(value);
    }

    /// <summary>
    /// Call this method from external scripts to update the slider visually.
    /// </summary>
    public void SetSliderValue(float value)
    {
        isSettingValueFromCode = true;
        slider.value = useWholeNumbers ? Mathf.Round(value) : value;
        isSettingValueFromCode = false;
    }
}
