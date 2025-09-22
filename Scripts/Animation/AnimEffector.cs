using UnityEngine;

public class AnimEffector : MonoBehaviour
{
    [SerializeField] private TriggerEnter m_TriggerEnter;
    [SerializeField] private TriggerStay m_TriggerStay;
    [SerializeField] private TriggerExit m_TriggerExit;
    [SerializeField] private ObjectT ObjectT;
    [SerializeField] private AnimEffectSO AnimEffectSO;

    [SerializeField] private float m_AnimTime = 0.5f;

    private float m_AnimCooldown;
    private float m_AnimOffCooldown;

    void Start()
    {
        m_AnimCooldown = m_AnimTime;
    }

    private void OnEnable()
    {
        if (m_TriggerEnter) m_TriggerEnter.OnEnter2D += TriggerEffect;
        if (m_TriggerStay) m_TriggerStay.OnStay2D += TriggerEffect;
        if (m_TriggerExit) m_TriggerExit.OnExit2D += TriggerExit;
    }

    private void OnDisable()
    {
        if (m_TriggerEnter) m_TriggerEnter.OnEnter2D -= TriggerEffect;
        if (m_TriggerStay) m_TriggerStay.OnStay2D -= TriggerEffect;
        if (m_TriggerExit) m_TriggerExit.OnExit2D -= TriggerExit;
    }

    private void Update()
    {
        if (m_AnimCooldown > 0) m_AnimCooldown -= Time.deltaTime;
        if (m_AnimOffCooldown > 0) m_AnimOffCooldown -= Time.deltaTime;
    }

    private void TriggerEffect(Collider2D other)
    {
        if (ObjectT == null || !ObjectT.IsAlive) return;
        if (AnimEffectSO == null) return;
        if (m_AnimCooldown > 0) return;

        Animator animator = other.GetComponent<Animator>();
        if (animator != null)
            AnimEffectSO.AnimEffect(animator);

        m_AnimCooldown = m_AnimTime;
    }

    private void TriggerExit(Collider2D other)
    {
        if (ObjectT == null || !ObjectT.IsAlive) return;
        if (AnimEffectSO == null) return;
        if (m_AnimOffCooldown > 0) return;

        Animator animator = other.GetComponent<Animator>();
        if (animator != null)
            AnimEffectSO.AnimEffectOff(animator);

        m_AnimOffCooldown = m_AnimTime;
    }
}
