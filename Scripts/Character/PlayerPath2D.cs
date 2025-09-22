using UnityEngine;

public class PlayerPath2D : PlayerPath
{
    private Vector3[] positions;

    protected override void Update()
    {
        if (GameManager.Instance.IsPause) return;
        if (pointList.Count == 0) return;

        // Ensure positions array is large enough to avoid frequent allocations
        if (positions == null || positions.Length < pointList.Count)
        {
            positions = new Vector3[pointList.Count];
        }

        // Transform positions to 2D without re-allocating memory
        for (int i = 0; i < pointList.Count; i++)
        {
            positions[i].x = pointList[i].x;
            positions[i].y = pointList[i].y;
            positions[i].z = 0;
        }

        // Update LineRenderer
        lineRenderer.positionCount = pointList.Count;
        lineRenderer.SetPositions(positions);
    }
}
