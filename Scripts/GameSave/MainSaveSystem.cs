using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;

public class MainSaveSystem : MonoBehaviour
{
    [SerializeField] private CloudSaveSystem m_CloudSaveSystem;
    [SerializeField] private LocalSaveSystem m_LocalSaveSystem;
    [SerializeField] private PlayTime m_PlayTime;
    public GameDataSO GameDataSO;
    private bool m_IsSignedIn;

    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            m_IsSignedIn = true;
            await m_CloudSaveSystem.LoadFromCloud(true);
            m_LocalSaveSystem.SaveLocal();
        }
        else
            m_LocalSaveSystem.LoadLocal();
    }

    private void OnEnable()
    {
        if (m_CloudSaveSystem)
        {
            m_CloudSaveSystem.OnLoadFailed.AddListener(OnCloudLoadFailed);
            m_CloudSaveSystem.OnLoadCancelled.AddListener(OnCloudLoadCancelled);
        }

        if (m_LocalSaveSystem)
            m_LocalSaveSystem.OnLoadFailed.AddListener(OnLocalLoadFailed);
    }

    private void OnDisable()
    {
        if (m_CloudSaveSystem)
        {
            m_CloudSaveSystem.OnLoadFailed.RemoveListener(OnCloudLoadFailed);
            m_CloudSaveSystem.OnLoadCancelled.RemoveListener(OnCloudLoadCancelled);
        }

        if (m_LocalSaveSystem)
            m_LocalSaveSystem.OnLoadFailed.RemoveListener(OnLocalLoadFailed);
    }

    public async void OnCloudLoadFailed()
    {
        m_LocalSaveSystem.LoadLocal();
        if (m_IsSignedIn)
            await m_CloudSaveSystem.SaveToCloud(true);
    }

    public void OnCloudLoadCancelled()
    {
        m_LocalSaveSystem.LoadLocal();
    }

    public void OnLocalLoadFailed()
    {
        m_LocalSaveSystem.SaveLocal();
    }

    public async void Save(bool isPriority = false)
    {
        GameDataSO.GameData.totalPlayTime += m_PlayTime.GetPlayTime();
        m_LocalSaveSystem.SaveLocal();
        if (m_CloudSaveSystem && m_IsSignedIn)
            await m_CloudSaveSystem.SaveToCloud(isPriority);
    }

    public async void Load(bool isPriority = false)
    {
        if (m_CloudSaveSystem && m_IsSignedIn)
            await m_CloudSaveSystem.LoadFromCloud(isPriority);
        else
            m_LocalSaveSystem.LoadLocal();
    }
}
