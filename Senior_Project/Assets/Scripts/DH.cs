using UnityEngine;
/// <summary>
/// class to help with debugging
/// </summary>
public static class DH{
    private static bool running = false;//set true if debugging
	// Use this for initialization
    public static void ping(object msg)
    {
        if(running)Debug.Log(msg);
    }
    public static void ping(object msg,Object obj)
    {
        if (running) Debug.Log(msg,obj);
    }
}
