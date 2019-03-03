using UnityEngine;
using UnityEditor;

public static class UnityConsole
{
    [MenuItem("Tools/Clear Console %#c")] // CMD + SHIFT + C
    public static void Clear()
    {
        // This simply does "LogEntries.Clear()" the long way:
        var logEntries = System.Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        clearMethod.Invoke(null, null);
    }

}