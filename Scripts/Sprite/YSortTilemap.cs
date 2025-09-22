using UnityEngine;
using UnityEngine.Tilemaps;

public class YSortTilemap : YSort
{
    private TilemapRenderer tr;

    protected override void Awake()
    {
        tr = GetComponent<TilemapRenderer>();
    }

    protected override void LateUpdate()
    {
        float yPos = transform.position.y + offsetY;
        int order = Mathf.RoundToInt(yPos * -precision);

        if (tr != null)
            tr.sortingOrder = order;
    }
}
