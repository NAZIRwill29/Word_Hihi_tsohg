using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class LocalSaveSystem : MonoBehaviour
{
    private string localSavePath;
    public GameDataSO gameDataSO;
    public UnityEvent OnLoadFailed;

    private void Awake()
    {
        localSavePath = Application.persistentDataPath + "/savegame.json";
    }

    public void SaveLocal()
    {
        string json = JsonUtility.ToJson(gameDataSO);
        File.WriteAllText(localSavePath, json);
        Debug.Log("Game saved locally.");
    }

    public void LoadLocal()
    {
        if (File.Exists(localSavePath))
        {
            string json = File.ReadAllText(localSavePath);
            JsonUtility.FromJsonOverwrite(json, gameDataSO);
        }
        else
        {
            Debug.Log("No local save file found.");
            OnLoadFailed?.Invoke();
        }
    }
}
