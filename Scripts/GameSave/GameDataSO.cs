using UnityEngine;

[CreateAssetMenu(fileName = "GameDataSO", menuName = "Game Data/Save Data")]
public class GameDataSO : ScriptableObject
{
    public GameData GameData;
}

public class GameData
{
    public float totalPlayTime;
    public int level;
    public int coins;
    public string playerName;
    public string lastCloudSaveDate;
}
