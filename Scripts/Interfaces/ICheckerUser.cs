using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICheckerUser
{
    public Dictionary<string, Type> TypeCache { get; set; }
    public bool IsCacheInitialized { get; set; }
    public StatTriggerFlyweight StatTriggerFlyweight { get; }
}
