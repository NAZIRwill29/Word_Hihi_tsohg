using System.Collections;
using UnityEngine;

public class ActionCommand : ICommandDelay
{
    public ICommandExecuteDelay ICommandExecuteDelay { get; set; }
    public float Delay { get; set; }
    public float Cooldown { get; set; }
    public bool IsFinish { get; set; }
    public string IdName { get; set; }
    public string CmdName { get; set; }
    public int ObjectId { get; set; }
    private ExecuteActionCommandData m_Data = new();

    public ActionCommand(ICommandExecuteDelay commandExecuteDelay, int objectId, string name, string idName = "", float delay = 0f, float cooldown = 0f, ExecuteActionCommandData data = null)
    {
        ICommandExecuteDelay = commandExecuteDelay;
        CmdName = name;//kick1, kick2, kick3
        ObjectId = objectId;
        Delay = delay;
        Cooldown = cooldown;
        IdName = idName;//kick
        m_Data = data;
        // if (m_Data != null && m_Data.Poolable != null)
        //     Debug.Log("data poolable " + m_Data.Poolable);
    }

    //ABILITY() - 5
    public IEnumerator ExecuteDelay()
    {
        //6
        yield return new WaitForSeconds(Delay);
        //Debug.Log("(6)ExecuteDelay " + GetTime.GetCurrentTime("full-ms"));
        //Execute();
        ICommandExecuteDelay.ExecuteDelay(CmdName, m_Data);
        ActionCommandPool.ReturnCommand(this);
    }

    public void Execute()
    {
        //Debug.Log("exec " + m_Name + " action move" + GetTime.GetCurrentTime());
        //ICommandExecute.Execute(m_Name);
    }

    public void Initialize(ICommandExecuteDelay commandExecuteDelay, int objectId, string name, string idName, float delay, float cooldown, ExecuteActionCommandData data)
    {
        ICommandExecuteDelay = commandExecuteDelay;
        ObjectId = objectId;
        CmdName = name;
        IdName = idName;
        Delay = delay;
        Cooldown = cooldown;
        m_Data = data;
    }

    public void Reset()
    {
        ICommandExecuteDelay = null;
        ObjectId = 0;
        CmdName = "";
        IdName = "";
        Delay = 0f;
        Cooldown = 0f;
        m_Data = null;
    }

}