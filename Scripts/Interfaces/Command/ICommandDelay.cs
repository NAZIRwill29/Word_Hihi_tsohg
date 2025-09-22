using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommandDelay : ICommand
{
    public ICommandExecuteDelay ICommandExecuteDelay { get; set; }
    public bool IsFinish { get; set; }
    public float Delay { get; set; }// Delay in seconds
    public float Cooldown { get; set; }
    //ABILITY() - 5
    IEnumerator ExecuteDelay();
}
