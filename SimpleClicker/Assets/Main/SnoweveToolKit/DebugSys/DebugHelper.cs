using UnityEngine;
using System.Diagnostics;

public static class DebugHelper
{
    private const string DEFAULT_COLOR = "ffffff"; 
    
    [Conditional("UNITY_EDITOR")]
    public static void LogEditor(object message, Object context = null, Color? color = null)
    {
        if (color.HasValue)
        {
            string hexColor = ColorUtility.ToHtmlStringRGB(color.Value);
            UnityEngine.Debug.Log($"<color=#{hexColor}>[EditorLog] {message}</color>", context);
        }
        else
        {
            UnityEngine.Debug.Log($"[EditorLog] {message}", context);
        }
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogWarningEditor(object message, Object context = null, Color? color = null)
    {
        if (color.HasValue)
        {
            string hexColor = ColorUtility.ToHtmlStringRGB(color.Value);
            UnityEngine.Debug.LogWarning($"<color=#{hexColor}>[EditorWarning] {message}</color>", context);
        }
        else
        {
            UnityEngine.Debug.LogWarning($"[EditorWarning] {message}", context);
        }
    }
    
    [Conditional("UNITY_EDITOR")]
    public static void LogErrorEditor(object message, Object context = null, Color? color = null)
    {
        if (color.HasValue)
        {
            string hexColor = ColorUtility.ToHtmlStringRGB(color.Value);
            UnityEngine.Debug.LogError($"<color=#{hexColor}>[EditorError] {message}</color>", context);
        }
        else
        {
            UnityEngine.Debug.LogError($"[EditorError] {message}", context);
        }
    }

    private static string GetHexColor(Color? color)
    {
        return color.HasValue ? ColorUtility.ToHtmlStringRGB(color.Value) : DEFAULT_COLOR;
    }
}