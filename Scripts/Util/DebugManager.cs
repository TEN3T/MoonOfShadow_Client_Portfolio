using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager 
{
    private const bool _isPrintDebug = true;
    public  bool isPrintDebug
    {
        get {
            return _isPrintDebug;
        }
    }
    private static DebugManager _instance { get; set; }
    public static DebugManager Instance
    {
        get
        {
            return _instance ?? (_instance = new DebugManager());
        }
    }

    public void PrintDebug(object target) {
        if (isPrintDebug)
            Debug.Log(target);
        
    }
    public void PrintDebug(string target)
    {
        if (isPrintDebug)
            Debug.Log(target);
    }
    public void PrintDebug(object target, object value)
    {
        if (isPrintDebug)
            Debug.Log(target+" : "+value);
    }
    public void PrintDebug(string target, params object[] args)
    {
        if (isPrintDebug)
            Debug.LogFormat(target, args);
    }

    public void PrintWarning(object target)
    {
        if (isPrintDebug)
            Debug.LogWarning(target);
    }
    public void PrintWarning(string target)
    {
        if (isPrintDebug)
            Debug.LogWarning(target);
    }
    public void PrintWarning(object target, object value)
    {
        if (isPrintDebug)
            Debug.LogWarning(target+" : "+value);
    }
    public void PrintWarning(string target, params object[] args)
    {
        if (isPrintDebug)
            Debug.LogWarningFormat(target, args);
    }

    public void PrintError(object target)
    {
        if (isPrintDebug)
            Debug.LogError(target);
    }
    public void PrintError(string target)
    {
        if (isPrintDebug)
            Debug.LogError(target);
    }
    public void PrintError(object target, object value)
    {
        if (isPrintDebug)
            Debug.LogError(target + " : " + value);
    }
    public void PrintError(string target, params object[] args)
    {
        if (isPrintDebug)
            Debug.LogErrorFormat(target, args);
    }

    public void PrintDrawLine()
    {
        if (isPrintDebug)
            Debug.Log("------------------------------------------------------------------");
    }

}
