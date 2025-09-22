using UnityEngine;

[CreateAssetMenu(fileName = "FloatingTextManager", menuName = "Manager/FloatingTextManager")]
public class FloatingTextManager : ScriptableObject
{
    //[SerializeField] protected ObjectPool m_ObjectPool;

    private GameObject m_TextContainer;
    [SerializeField] private NameDataFlyweight m_FloatingTextNameData;
    private PlayModeManager m_PlayModeManager;

    public void Initialize(GameObject textContainer, PlayModeManager playModeManager)
    {
        m_PlayModeManager = playModeManager;
        m_TextContainer = textContainer;
        m_PlayModeManager.OnPlayModeChange.AddListener(OnPlayModeChange);
    }

    private void OnDestroy()
    {
        m_PlayModeManager.OnPlayModeChange.RemoveListener(OnPlayModeChange);
    }

    private void OnPlayModeChange(string modeName)
    {
        m_TextContainer = GameManager.Instance.TextContainers.Find(x => x.NameData.Name == modeName).GameObject;
    }

    //Show("+" + pesosAmount + " pesos!", 25, Color.yellow, transform.position, Vector3.up * 25, 1.5f);
    public void Show(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        string floatingTextName = m_FloatingTextNameData != null ? m_FloatingTextNameData.Name : "";
        if (GameManager.Instance.ObjectPool.GetPooledObject(floatingTextName) is FloatingText floatingText)
        {
            floatingText.Show(m_TextContainer, msg, fontSize, color, Camera.main.WorldToScreenPoint(position), motion, duration);
        }
    }
}