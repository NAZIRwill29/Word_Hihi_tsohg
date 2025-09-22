using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// interface to wrap your actions in a "command object"
public interface ICommand
{
    public int ObjectId { get; set; }
    public string IdName { get; set; }
    public string CmdName { get; set; }
    public void Execute();
}