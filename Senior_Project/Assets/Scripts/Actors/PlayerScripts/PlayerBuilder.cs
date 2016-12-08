using UnityEngine;
using System.Collections;

//create states for player state machine --class to consolidate state creation
public class PlayerBuilder : Builder{
    public static string[] triggers = {
        "Idle","Moving","Landed","Jump","Falling","Attack0","Attack1"
    };
    //Locked-- states with committed transitions
    public static string[] specialTriggers =
    {
        "Locked"
    };
    public static PlayerState Load()
    {
        PlayerState status = new PlayerState();
        //state 1 -- known bug, if jump too soon after landing direction will fail to update
        State jmp = new State(3, new System.Func<InputSet,string[]>[]{ jumping,airborne,airborne}, status,false);
        jmp.link("Moving", "mAirborne");
        jmp.setDefault("Airborne");
        status.addState("Jumping", jmp);
        //
        State air = new State(0, new System.Func<InputSet, string[]>[] { airborne }, status,false);
        air.link("Landed", "GroundIdle");
        air.link("Attack0", "Attack0");
        air.link("Attack1", "Attack1");
        status.addState("Airborne", air);
        //moving airborne
        State mAir = new State(0, new System.Func<InputSet, string[]>[] { mAirborne }, status,false);
        mAir.link("Landed", "GroundIdle");
        mAir.link("Attack0", "Attack0");
        mAir.link("Attack1", "Attack1");
        mAir.link("Idle", "Airborne");
        status.addState("mAirborne", mAir);
        //
        State idle = new State(0, new System.Func<InputSet, string[]>[] { groundIdle }, status,false);
        //insert fall transition here if stage design warrants
        idle.link("Attack0", "Attack0");
        idle.link("Attack1", "Attack1");
        idle.link("Moving","GroundMoving");
        idle.link("Jump", "Jumping");
        status.addState("GroundIdle", idle);
        //
        State moving = new State(0, new System.Func<InputSet, string[]>[] { groundMoving }, status,false);
        moving.link("Falling", "mAirborne");
        moving.link("Attack0", "Attack0");
        moving.link("Attack1", "Attack1");
        moving.link("Idle", "GroundIdle");
        moving.link("Jump", "Jumping");
        status.addState("GroundMoving", moving);
        //attack
        State atk0 = new State(3, new System.Func<InputSet, string[]>[] { attack0 }, status,false);
        //attack0.link("Moving", "mAirborne"); hit state here
        atk0.setDefault("Airborne");
        status.addState("Attack0", atk0);
        //
        State atk1 = new State(3, new System.Func<InputSet, string[]>[] { attack1 }, status,false);
        //attack1.link("Moving", "mAirborne"); hit state here
        atk1.setDefault("Airborne");
        status.addState("Attack1", atk1);

        status.setInitial("Airborne");
        return status;
    }
    //States
    private static string[] groundIdle(InputSet raw)
    {
        if (checkSpecial(raw)) breakState();
        UserIn In = (UserIn)raw;//TODO -- figure out a better way of doing this
        string[] stack;
        //surface(In.actor); implement at some point to fix clipping
        if (In.CommandIn[2])
        {
            stack = new string[] { triggers[3] };
        }
        else if (In.CommandIn[0]) stack = new string[] { triggers[5] };
        else if (In.CommandIn[1]) stack = new string[] { triggers[6] };
        else if (In.DirectionalIn[0] ^ In.DirectionalIn[1])
        {
            stack = new string[] { triggers[1] };
        }
        else stack = new string[] { };
        return stack;
    }
    private static string[] groundMoving(InputSet raw)
    {
        if (checkSpecial(raw)) breakState();
        UserIn In = (UserIn)raw;//TODO -- figure out a better way of doing this
        string[] stack;
        if (!((PlayerModel)In.actor).land.grounded)
        {
            In.actor.gravity = 0;
            stack = new string[] { triggers[4] };
        }
        else if (In.CommandIn[2])
        {
            stack = new string[] { triggers[3] };
        }
        else if (In.CommandIn[0]) stack = new string[] { triggers[5] };
        else if (In.CommandIn[1]) stack = new string[] { triggers[6] };
        else if (In.DirectionalIn[0] == In.DirectionalIn[1])
        {
            stack = new string[] { triggers[0] };
        }
        else
        {
            move(In.DirectionalIn[0], In.actor);
            stack = new string[] { };
        }
        return stack;
    }
    private static string[] jumping(InputSet raw)
    {
        if (checkSpecial(raw)) breakState();
        UserIn In = (UserIn)raw;//TODO -- figure out a better way of doing this
        In.actor.gravity = statBank[In.actor.movementGroup][2];
        jump(In.actor);
        string[] stack;
        if (In.DirectionalIn[0] != In.DirectionalIn[1])
        {
            stack = new string[] { triggers[1] };
        }
        else stack = new string[] { };
        return stack;
    }
    private static string[] airborne(InputSet raw)
    {
        if (checkSpecial(raw)) breakState();
        UserIn In = (UserIn)raw;//TODO -- figure out a better way of doing this
        string[] stack;
        jump(In.actor);
        if (((PlayerModel)In.actor).land.grounded)//find bettter way to do this
        {
            stack = new string[] { triggers[2] };
        }
        else if (In.CommandIn[0]) stack = new string[] { triggers[5] };
        else if (In.CommandIn[1]) stack = new string[] { triggers[6] };
        else stack = new string[] { };
        return stack;
    }
    //player specific jump
    private static string[] mAirborne(InputSet raw)
    {
        if (checkSpecial(raw)) breakState();
        UserIn In = (UserIn)raw;//TODO -- figure out a better way of doing this
        string[] stack;
        jump(In.actor);
        bool free = move((!In.actor.GetComponent<SpriteRenderer>().flipX), In.actor);
        if (((PlayerModel)In.actor).land.grounded)//find bettter way to do this
        {
            stack = new string[] { triggers[2] };
        }
        else if (In.CommandIn[0]) stack = new string[] { triggers[5] };
        else if (In.CommandIn[1]) stack = new string[] { triggers[6] };
        else if (!free) stack = new string[] { triggers[0] };
        else stack = new string[] { };
        return stack;
    }
    //this will become transition state to determine which attack should be performed
    private static string[] attack0(InputSet raw)
    {
        if (checkSpecial(raw)) breakState();
        UserIn In = (UserIn)raw;
        In.actor.launchHit(0);
        return new string[] { };
    }
    private static string[] attack1(InputSet raw)
    {
        if (checkSpecial(raw)) breakState();
        UserIn In = (UserIn)raw;
        In.actor.launchHit(1);
        return new string[] { };
    }
    //is going to be used in the future for special states
    //should run check as first action in every method
    protected static bool checkSpecial(InputSet raw)
    {
        return false;
    }
    protected static void breakState()
    {
        //code for breaking state
    }
}
