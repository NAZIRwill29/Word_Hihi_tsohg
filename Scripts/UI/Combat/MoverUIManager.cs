using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoverUI
{
    public RectTransform UIRect;
    public Vector2 Direction;
    public float MoveSpeed; // pixels per second
}

public class MoverUIManager : MonoBehaviour
{
    public List<MoverUI> MoverUIs = new();
    [SerializeField] private float m_MoverTextMaxX, m_MoverTextMinX;
    [SerializeField] private float m_MoverTextMaxY, m_MoverTextMinY;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsPause) return;
        if (MoverUIs.Count == 0) return;

        float delta = Time.deltaTime;
        for (int i = 0; i < MoverUIs.Count; i++)
        {
            var mover = MoverUIs[i];
            if (mover.UIRect == null) continue;

            // Move
            Vector2 pos = mover.UIRect.anchoredPosition;
            pos += mover.Direction * mover.MoveSpeed * delta;

            bool bounced = false;

            // Check horizontal bounds
            if (pos.x > m_MoverTextMaxX)
            {
                pos.x = m_MoverTextMaxX;
                mover.Direction.x = -mover.Direction.x;
                bounced = true;
            }
            else if (pos.x < m_MoverTextMinX)
            {
                pos.x = m_MoverTextMinX;
                mover.Direction.x = -mover.Direction.x;
                bounced = true;
            }

            // Check vertical bounds
            if (pos.y > m_MoverTextMaxY)
            {
                pos.y = m_MoverTextMaxY;
                mover.Direction.y = -mover.Direction.y;
                bounced = true;
            }
            else if (pos.y < m_MoverTextMinY)
            {
                pos.y = m_MoverTextMinY;
                mover.Direction.y = -mover.Direction.y;
                bounced = true;
            }

            // On bounce, add a slight random rotation to the direction for "swing"
            if (bounced)
            {
                float angleOffset = Random.Range(-15f, 15f);
                mover.Direction = Quaternion.Euler(0, 0, angleOffset) * mover.Direction;
                mover.Direction.Normalize();
            }

            mover.UIRect.anchoredPosition = pos;
        }
    }

    public void InitializeMovingUIS(List<RectTransform> rects, float minSpeed, float maxSpeed)
    {
        if (rects == null || rects.Count == 0)
        {
            MoverUIs.Clear();
            return;
        }

        MoverUIs.Clear();
        foreach (RectTransform rect in rects)
        {
            MoverUIs.Add(new MoverUI
            {
                UIRect = rect,
                Direction = Vector2.zero
            });
        }
        SetRandomDirections();
        SetRandomSpeeds(minSpeed, maxSpeed);
    }

    public void SetRandomDirections()
    {
        foreach (var mover in MoverUIs)
        {
            float angle = Random.Range(0f, 360f);
            mover.Direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
        }
    }

    public void SetDirections(float angle)
    {
        foreach (var mover in MoverUIs)
        {
            mover.Direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
        }
    }

    public void SetRandomSpeeds(float minSpeed, float maxSpeed)
    {
        foreach (var mover in MoverUIs)
        {
            mover.MoveSpeed = Random.Range(minSpeed, maxSpeed);
        }
    }

    public void SetSpeeds(float speed)
    {
        foreach (var mover in MoverUIs)
        {
            mover.MoveSpeed = speed;
        }
    }
}
