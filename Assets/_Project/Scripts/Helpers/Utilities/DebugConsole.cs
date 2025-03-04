using UnityEngine;

public static class DebugConsole
{
    public static void Log(object message, GameObject context = null)
    {
        Debug.Log($"|DEBUG|{message}", context);
    }
}