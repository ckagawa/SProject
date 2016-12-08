using UnityEngine;
using System.Collections;

public class UserIn : InputSet{
    //override flags
    public enum AbsoluteFlag{None,Pause,Kill}
    public enum StageFlag {None,Hit };

    new public bool[] DirectionalIn;
    new public bool[] CommandIn;
    new public AbsoluteFlag Overrule { get; set; }
    public UserIn(Actor actor)
    {
        //directional block
        this.actor = actor;
        DirectionalIn = new bool[]
        {
            Input.GetButton("Right"),
            Input.GetButton("Left"),
            Input.GetButton("Up"),
            Input.GetButton("Down"),
        };
        //command block
        CommandIn = new bool[] {
            Input.GetButtonDown("AAttack"),
            Input.GetButtonDown("BAttack"),
            Input.GetButton("Jump"),
            Input.GetButtonDown("Selector"),
            Input.GetButtonDown("Cancel")
        };
    }
}

