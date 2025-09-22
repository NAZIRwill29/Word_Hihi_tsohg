using UnityEngine;

/// <summary>
/// tracks the time since the game started (excluding time spent in menus if Time.timeScale = 0)
/// </summary>/
public class PlayTimeInPlay : PlayTime
{
    private float m_PlayTime;

    void Update()
    {
        m_PlayTime += Time.deltaTime; // Accumulates playtime only when the game is running
    }

    public override float GetPlayTime()
    {
        float playTime = m_PlayTime;
        //reset
        m_PlayTime = 0;

        return playTime; // Returns playtime in seconds
    }

}
