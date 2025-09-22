using UnityEngine;

/// <summary>
/// Each Sector manages the loading and unloading of content for a specific part of the level based on proximity to the player.
///
/// This works with the GameSectors script to set/unset a dirty flag to minimize unnecessary updates.
/// </summary>
public class Sector : MonoBehaviour
{
    [Header("Scene assets")]
    [SerializeField] protected SceneLoader m_SceneLoader;

    [SerializeField] protected string m_ScenePath;
    // Properties
    public bool IsLoaded { get; protected set; } = false;
    public bool IsDirty { get; protected set; } = false;

    protected virtual void Awake()
    {
        m_SceneLoader = FindFirstObjectByType<SceneLoader>();

        if (m_SceneLoader == null)
        {
            Debug.LogError("[Sector]: SceneLoader not found in the scene.");
        }
        // Reset the dirty flag to start
        Clean();

        IsLoaded = false;
    }

    // Mark the sector as needing an update
    public void MarkDirty()
    {
        IsDirty = true;

        //Debug.Log("Sector " + gameObject.name + " is marked dirty");
    }

    // Load sector content
    public virtual void LoadContent()
    {
        // Implement content loading logic
        IsLoaded = true;

        if (!string.IsNullOrEmpty(m_ScenePath))
            m_SceneLoader.LoadSceneAdditivelyByPath(m_ScenePath);

        //Debug.Log($"{gameObject.name} Loading sector content...");
    }

    // Unload sector content
    public virtual void UnloadContent()
    {
        // Content unloading logic
        IsLoaded = false;
        m_SceneLoader.UnloadSceneByPath(m_ScenePath);

        Debug.Log("Unloading sector content...");
    }

    // Check if the player is close enough to consider loading this sector
    public virtual bool IsPlayerClose(Vector3 playerPosition)
    {
        return true;
    }

    // Reset the dirty flag after updating
    public void Clean()
    {
        IsDirty = false;
    }

    protected virtual void OnDrawGizmosSelected()
    {
    }

    protected virtual void OnDestroy()
    {
        m_SceneLoader.UnloadSceneImmediately(m_ScenePath);
    }
}