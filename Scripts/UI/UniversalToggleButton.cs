using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(Button))]
public class UniversalToggleButton : MonoBehaviour
{
    [Header("Visual References")]
    public TextMeshProUGUI buttonText;
    public Image iconImage; // Child image to toggle
    public Sprite onSprite;
    public Sprite offSprite;

    [Header("Text States")]
    public string onText = "ON";
    public string offText = "OFF";

    [Header("Toggle Settings")]
    public bool isOn = false;

    [Header("Events")]
    public UnityEvent<bool> onToggleChanged;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Toggle);
        UpdateVisual();
    }

    public void Toggle()
    {
        isOn = !isOn;
        UpdateVisual();
        onToggleChanged.Invoke(isOn);
    }

    public void SetState(bool newState)
    {
        isOn = newState;
        UpdateVisual();
        onToggleChanged.Invoke(isOn);
    }

    private void UpdateVisual()
    {
        if (buttonText != null)
            buttonText.text = isOn ? onText : offText;

        if (iconImage != null)
            iconImage.sprite = isOn ? onSprite : offSprite;
    }
}
