using UnityEngine;

/// <summary>
/// Each Sector manages the loading and unloading of content for a specific part of the level based on proximity to the player.
///
/// This works with the GameSectors script to set/unset a dirty flag to minimize unnecessary updates.
/// </summary>
public class Sector3D : Sector
{
    [Tooltip("Offset to transform position")]
    public Vector3 m_CenterOffset;

    [Tooltip("Minimum distance to load")] public float m_LoadRadius;

    [Header("Visualization")]
    [Tooltip("Material used when the sector's content is loaded.")]
    [SerializeField]
    Material m_ActiveMaterial;

    [Tooltip("Material used when the sector's content is unloaded.")]
    [SerializeField]
    Material m_InactiveMaterial;

    // Reference to the MeshRenderer for visualization
    MeshRenderer m_MeshRenderer;
    protected override void Awake()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();

        base.Awake();
    }

    // Load sector content
    public override void LoadContent()
    {
        base.LoadContent();

        if (m_MeshRenderer != null)
            m_MeshRenderer.material = m_ActiveMaterial;
    }

    // Unload sector content
    public override void UnloadContent()
    {
        base.UnloadContent();

        if (m_MeshRenderer != null)
            m_MeshRenderer.material = m_InactiveMaterial;
    }

    // Check if the player is close enough to consider loading this sector
    public override bool IsPlayerClose(Vector3 playerPosition)
    {
        return Vector3.Distance(playerPosition, transform.position + m_CenterOffset) <= m_LoadRadius;
    }

    protected override void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + m_CenterOffset, m_LoadRadius);
    }

    protected override void OnDestroy()
    {

    }
}