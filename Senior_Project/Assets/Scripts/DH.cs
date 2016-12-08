using UnityEngine;
using System.Collections;
//helper to make debugging easier
public static class DH{
    private static bool running = true;
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
