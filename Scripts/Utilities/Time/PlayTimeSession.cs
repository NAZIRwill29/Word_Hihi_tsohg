using UnityEngine;

/// <summary>
/// Time.realtimeSinceStartup keeps running even if Time.timeScale = 0
/// </summary>
public class PlayTimeSession : PlayTime
{
    private float sessionStartTime;

    void Start()
    {
        sessionStartTime = Time.realtimeSinceStartup;
    }

    public override float GetPlayTime()
    {
        float playTime = Time.realtimeSinceStartup - sessionStartTime;
        //reset
        sessionStartTime = Time.realtimeSinceStartup;

        return playTime;
    }

}
