using System;
using UnityEngine;

//Projectile Player A, Projectile Enemy A, Projectile Enemy Homing A, Die FX
//Player Damage FX, Enemy Damage FX, Heal FX, Revive FX
[CreateAssetMenu(fileName = "ThingHappenTrigger", menuName = "ThingHappenTrigger/ThingHappenTrigger")]
public class ThingHappenTriggerSO : ScriptableObject
{
    [SerializeField] private NameDataFlyweight m_SoundNameDataFlyweight, m_FXNameDataFlyweight;
    private string m_SoundName
    {
        get => m_SoundNameDataFlyweight != null ? m_SoundNameDataFlyweight.Name : string.Empty;
    }
    private string m_FXName
    {
        get => m_FXNameDataFlyweight != null ? m_FXNameDataFlyweight.Name : string.Empty;
    }
    [SerializeField] protected bool m_IsHasText = true;
    [SerializeField] private TextFloatData m_TextFloatData;

    public void ObjecTThingHappen(ObjectT objectT, ThingHappenData thingHappenData, string extraText = null, bool isSound = true)
    {
        if (m_TextFloatData == null)
        {
            Debug.Log("no TextFloatData");
            return;
        }
        thingHappenData.TextFloatData.Msg = m_TextFloatData.Msg + extraText;
        thingHappenData.TextFloatData.FontSize = m_TextFloatData.FontSize;
        thingHappenData.TextFloatData.Color = m_TextFloatData.Color;
        thingHappenData.TextFloatData.Position = objectT.transform.position;
        thingHappenData.TextFloatData.Motion = m_TextFloatData.Motion;
        thingHappenData.TextFloatData.Duration = m_TextFloatData.Duration;

        thingHappenData.SoundName = isSound ? m_SoundName : string.Empty;
        thingHappenData.FXName = m_FXName;

        objectT.ThingHappen(thingHappenData);
    }

    // public void ObjecTThingHappen(ObjectT objectT, string extraText = null, bool isSound = true)
    // {
    //     if (m_TextFloatData == null)
    //     {
    //         Debug.Log("no TextFloatData");
    //         return;
    //     }
    //     TextFloatData textFloatDataTemp = new()
    //     {
    //         Msg = m_TextFloatData.Msg,
    //         FontSize = m_TextFloatData.FontSize,
    //         Color = m_TextFloatData.Color,
    //         Position = objectT.transform.position,//
    //         Motion = m_TextFloatData.Motion,
    //         Duration = m_TextFloatData.Duration
    //     };
    //     textFloatDataTemp.Msg += extraText;

    //     ObjecTThingHappen(objectT, textFloatDataTemp);
    // }

    // public void ObjecTThingHappen(ObjectT objectT, TextFloatData textFloatData = null, bool isSound = true)
    // {
    //     ThingHappenData thingHappenData = new()
    //     {
    //         SoundName = isSound ? m_SoundName : string.Empty,
    //         FXName = m_FXName,
    //         TextFloatData = m_IsHasText ? textFloatData : null
    //     };

    //     objectT.ThingHappen(thingHappenData);
    // }
}