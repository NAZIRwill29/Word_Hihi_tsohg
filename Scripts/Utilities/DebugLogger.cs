using UnityEngine;
using System.Text;

public class DebugLogger : MonoBehaviour
{
    private StringBuilder sb = new StringBuilder(256); // Reusable StringBuilder

    [Tooltip("This prefix is added to the start of each logged message")]
    [SerializeField] private string m_LogPrefix = "[DebugLogger]";

    public void LogMessage(string message)
    {
        LogMessage(message, LogType.Log); // Fixed: Use LogType.Log
    }

    public void LogWarningMessage(string message)
    {
        LogMessage(message, LogType.Warning);
    }

    public void LogErrorMessage(string message)
    {
        LogMessage(message, LogType.Error);
    }

    private void LogMessage(string message, LogType logType)
    {
        string formattedMessage = $"{m_LogPrefix} {message}";

        switch (logType)
        {
            case LogType.Warning:
                Debug.LogWarning(formattedMessage);
                break;
            case LogType.Error:
                Debug.LogError(formattedMessage);
                break;
            default:
                Debug.Log(formattedMessage);
                break;
        }
    }

    // Optimized LogFormat methods using StringBuilder
    public void LogFormat(string format, object arg1)
    {
        LogFormatInternal(format, LogType.Log, arg1); // Fixed: Use LogType.Log
    }

    public void LogFormat(string format, object arg1, object arg2)
    {
        LogFormatInternal(format, LogType.Log, arg1, arg2);
    }

    public void LogFormat(string format, object arg1, object arg2, object arg3)
    {
        LogFormatInternal(format, LogType.Log, arg1, arg2, arg3);
    }

    private void LogFormatInternal(string format, LogType logType, params object[] args)
    {
        sb.Clear();
        sb.AppendFormat(format, args);
        LogMessage(sb.ToString(), logType);
    }
}
