using System;
using UnityEngine;

/// <summary>
/// This method calculates playtime across multiple sessions and saves it.
/// </summary>
public class PlayTimePersistent : MonoBehaviour
{
    private DateTime sessionStartTime;

    void Start()
    {
        sessionStartTime = DateTime.UtcNow;
    }

    public void SavePlayTime()
    {
        TimeSpan sessionDuration = DateTime.UtcNow - sessionStartTime;
        float totalPlayTime = PlayerPrefs.GetFloat("TotalPlayTime", 0) + (float)sessionDuration.TotalSeconds;
        PlayerPrefs.SetFloat("TotalPlayTime", totalPlayTime);
        PlayerPrefs.Save();
    }

    public float GetTotalPlayTime()
    {
        return PlayerPrefs.GetFloat("TotalPlayTime", 0); // Returns total playtime in seconds
    }
}
