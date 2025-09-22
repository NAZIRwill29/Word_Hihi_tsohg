using UnityEngine;
using UnityEngine.Tilemaps;

public class Sector2D : Sector
{
    // [Header("Tilemap Info")]
    // [SerializeField] Tilemap[] m_Tilemaps; // Assuming you have a Tilemap component

    [Header("Sector Properties")]
    [SerializeField] Vector2 m_GridPosition; // Position of the sector in grid coordinates
    [SerializeField] Vector2 m_LoadRadiusInTiles; // Radius in tiles to load

    protected override void Awake()
    {
        // Initialize your Tilemap component if needed
        // if (m_Tilemaps == null)
        // {
        //     m_Tilemap = GetComponent<Tilemap>(); // Assuming Tilemap is attached to this GameObject
        // }

        base.Awake();
    }

    // Load sector content
    public override void LoadContent()
    {
        base.LoadContent();
        // Example: Activate tiles within the load radius
        // Adjust this part according to your specific tilemap implementation
        // Example: m_Tilemap.ActivateTilesAroundPosition(m_GridPosition, m_LoadRadiusInTiles);
        // foreach (var item in m_Tilemaps)
        // {
        //     item.gameObject.SetActive(true);
        // }
    }

    // Unload sector content
    public override void UnloadContent()
    {
        base.UnloadContent();

        // Example: Deactivate tiles within the load radius
        // Adjust this part according to your specific tilemap implementation
        // Example: m_Tilemap.DeactivateTilesAroundPosition(m_GridPosition, m_LoadRadiusInTiles);
        // foreach (var item in m_Tilemaps)
        // {
        //     item.gameObject.SetActive(false);
        // }
    }

    // Check if the player is close enough to consider loading this sector
    public override bool IsPlayerClose(Vector3 playerPosition)
    {
        // Convert player position to grid space
        Vector2 playerGridPos = new Vector2Int(Mathf.RoundToInt(playerPosition.x), Mathf.RoundToInt(playerPosition.y));
        Vector2 sectorGridPos = m_GridPosition; // Use stored grid position

        // Calculate separate distances for X and Y
        float distanceX = Mathf.Abs(playerGridPos.x - sectorGridPos.x);
        float distanceY = Mathf.Abs(playerGridPos.y - sectorGridPos.y);

        // Define different thresholds for X and Y distances
        float maxDistanceX = m_LoadRadiusInTiles.x;
        float maxDistanceY = m_LoadRadiusInTiles.y;

        // Check if within the bounds
        return distanceX <= maxDistanceX && distanceY <= maxDistanceY;
    }

    protected override void OnDrawGizmosSelected()
    {
        // Define different dimensions for X and Y based on load radius settings
        float gizmoWidth = m_LoadRadiusInTiles.x;  // Horizontal size (X-axis)
        float gizmoHeight = m_LoadRadiusInTiles.y;     // Vertical size (Y-axis)

        // Draw a wireframe rectangle (flattened cube) to represent the loading area
        Gizmos.color = Color.green; // Use a distinct color for visibility
        Gizmos.DrawWireCube(transform.position, new Vector3(gizmoWidth, gizmoHeight, 1));
    }

    protected override void OnDestroy()
    {
        // Clean up or unload any resources if needed
    }
}
