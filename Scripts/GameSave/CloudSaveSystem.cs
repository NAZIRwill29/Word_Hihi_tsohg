using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class CloudSaveSystem : MonoBehaviour
{
    public GameDataSO gameDataSO;
    [SerializeField] private UnityCloudSave m_UnityCloudSave;
    public UnityEvent OnLoadFailed, OnLoadCancelled;

    public async Task SaveToCloud(bool isPriority)
    {
        try
        {
            string todayDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
            if (gameDataSO.GameData.lastCloudSaveDate != todayDate || isPriority)
            {
                gameDataSO.GameData.lastCloudSaveDate = todayDate;
                // var saveData = new Dictionary<string, object>
                // {
                //     { "player_data", JsonUtility.ToJson(gameDataSO) }
                // };

                await m_UnityCloudSave.ForceSaveObjectData("player_data", JsonUtility.ToJson(gameDataSO));
                //await CloudSaveService.Instance.Data.Player.SaveAsync(saveData);
                Debug.Log("Game saved to cloud.");
            }
            else
            {
                Debug.Log("Quota game save has been used");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Cloud Save Failed: " + e.Message);
        }
    }

    public async Task LoadFromCloud(bool isPriority)
    {
        try
        {
            string todayDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
            if (gameDataSO.GameData.lastCloudSaveDate != todayDate || isPriority)
            {
                var savedData = await m_UnityCloudSave.RetrieveSpecificData<string>("player_data");
                if (savedData != null)
                {
                    JsonUtility.FromJsonOverwrite(savedData.ToString(), gameDataSO);
                }
                else
                {
                    Debug.LogWarning("No Cloud Saving data");
                    OnLoadFailed?.Invoke();
                }
            }
            else
            {
                Debug.Log("Quota game load has been used");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Cloud Load Failed: " + e.Message);
            OnLoadFailed?.Invoke();
        }
    }
}
