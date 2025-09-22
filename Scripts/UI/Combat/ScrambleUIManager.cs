using System.Collections.Generic;
using TMPro;
using UnityEngine;

// [System.Serializable]
// public class ScrambleUI
// {
//     public RectTransform TextRect;
// }

// Moves and "swings" text movers within specified bounds, bouncing off edges
public class ScrambleUIManager : MonoBehaviour
{
    //public List<ScrambleUI> MovingTexts = new();
    [SerializeField] private float m_ScrambleTextMaxX, m_ScrambleTextMinX;
    [SerializeField] private float m_ScrambleTextMaxY, m_ScrambleTextMinY;
    [SerializeField] private float m_ScrambleTextTextWidth = 50f;
    [SerializeField] private float m_ScrambleTextTextHeight = 50f;

    public void SetRandomPositionUIs(List<TextMeshProUGUI> scrambleTexts, int count)
    {
        if (count == 0) return;

        List<Rect> occupiedRects = new List<Rect>();

        for (int i = 0; i < count; i++)
        {
            const int maxTries = 20;
            Vector2 newPos = Vector2.zero;
            Rect newRect;
            int tries = 0;

            do
            {
                float randomX = Random.Range(m_ScrambleTextMinX, m_ScrambleTextMaxX);
                float randomY = Random.Range(m_ScrambleTextMinY, m_ScrambleTextMaxY);
                newPos = new Vector2(randomX, randomY);
                newRect = new Rect(newPos.x, newPos.y, m_ScrambleTextTextWidth, m_ScrambleTextTextHeight);
                tries++;
            }
            while (occupiedRects.Exists(r => r.Overlaps(newRect)) && tries < maxTries);

            occupiedRects.Add(newRect);
            scrambleTexts[i].rectTransform.anchoredPosition = newPos;
        }
    }
}
