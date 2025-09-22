using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthZone : StatusZone
{
    [SerializeField] protected HealthDataFlyweight m_HealthDataFlyweight;

    public override object CreateEffectData()
    {
        return m_HealthDataFlyweight.HealthData;
    }
}
