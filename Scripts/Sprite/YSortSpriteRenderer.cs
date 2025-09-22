using UnityEngine;

public class YSortSpriteRenderer : YSort
{
    private SpriteRenderer sr;

    protected override void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    protected override void LateUpdate()
    {
        float yPos = transform.position.y + offsetY;
        int order = Mathf.RoundToInt(yPos * -precision);

        if (sr != null)
            sr.sortingOrder = order;
    }
}
