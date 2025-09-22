using UnityEngine;

[System.Serializable]
public class HearNSound
{
    public float LineOfHear;
    public float LineOfHearSoundVol;
}

public class HearSense : MonoBehaviour
{
    [SerializeField] private DetectionSense m_DetectionSense;
    public HearNSound[] HearNSounds;
    private Vector2 m_CenterPos;

    public bool HearPlayer(Vector2 centerPos, Vector2 playerPos, float addLineOfHear = 0)
    {
        if (m_DetectionSense == null)
        {
            Debug.LogWarning("DetectionSense is not assigned in HearSense.");
            return false;
        }

        if (GameManager.Instance.player2D.ObjectVisibility)
        {
            DataBoolVariable data = VariableFinder.GetVariableContainNameFromList(GameManager.Instance.player2D.ObjectVisibility.StatsData.DataBoolVars, "CanBeHear");
            if (data != null)
            {
                if (!VariableChanger.IsBoolNull(data.IsCan)) return false;
            }
            else
            {
                Debug.LogWarning("IsCanBeHear is not assigned in player2D.");
            }
        }
        else
        {
            Debug.LogWarning("player2D is not assigned in GameManager.");
        }

        float distanceFromPlayer = Vector2.Distance(centerPos, playerPos);
        Vector2 directionToPlayer = (playerPos - centerPos).normalized;

        m_DetectionSense.DistanceFromPlayer = distanceFromPlayer;
        m_DetectionSense.DirectionToPlayer = directionToPlayer;

        float soundVol = GameManager.Instance.player2D.ObjectAudioMulti.soundVolProd;

        foreach (var hearNSound in HearNSounds)
        {
            if (distanceFromPlayer < hearNSound.LineOfHear + addLineOfHear && hearNSound.LineOfHearSoundVol <= soundVol)
            {
                return true;
            }
        }
        return false;
    }

    void OnDrawGizmosSelected()
    {
        if (HearNSounds == null || HearNSounds.Length == 0)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, HearNSounds[0].LineOfHear);
    }
}
