using UnityEngine;
using TMPro;
using UnityEngine.Pool;

public class FloatingText : MonoBehaviour, IPoolable
{
    private IObjectPool<IPoolable> m_ObjectPool;
    private bool m_IsActive;
    //private GameObject m_Go;
    private TextMeshProUGUI m_TextMesh;
    private Vector3 m_Motion;
    private float m_Duration;
    private float m_Alpha;
    private float m_Timer = 0;
    private Color m_Color = new Color();

    public void Initialize(IObjectPool<IPoolable> pool)
    {
        m_ObjectPool = pool;
        m_TextMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        //if (GameManager.Instance.IsPause) return;
        if (!m_IsActive) return;
        UpdateFloatingText();
    }

    //show text
    public void Show(GameObject textContainer, string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        m_IsActive = true;

        transform.SetParent(textContainer.transform);
        //change text in txt
        m_TextMesh.text = msg;
        m_TextMesh.fontSize = fontSize;
        m_Color = color;
        m_TextMesh.color = color;

        transform.position = position;
        m_Duration = duration;
        m_Motion = motion;

        m_Alpha = 1;
        m_Timer = 0;
    }

    public void UpdateFloatingText()
    {
        //if show is more than duration
        if (m_Timer > m_Duration)
            Deactivate();

        m_Timer += Time.deltaTime;
        gameObject.transform.position += m_Motion * Time.deltaTime;
        m_Alpha = 1 - (m_Timer / m_Duration);
        m_TextMesh.color = new Color(m_Color.r, m_Color.g, m_Color.b, m_Alpha);
    }

    public void Deactivate()
    {
        m_IsActive = false;

        if (m_ObjectPool != null)
        {
            m_ObjectPool.Release(this);
        }
        else
        {
            Debug.LogWarning("Floating Text: Object pool is not assigned. Destroying the Floating Text.");
            Destroy(gameObject);
        }
    }
}