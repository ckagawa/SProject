using UnityEngine;
/// <summary>
/// Builder for Enemy0
/// </summary>
public class E0Builder : Builder{
    public static Actor[] ACTORS()
    {
        Actor[] ret = new Actor[] {
            Object.Instantiate(Resources.Load("EnemyActors/Enemy0", typeof(GameObject)) as GameObject).GetComponent<Actor>()
        };
        return ret;
    }
    public static AIIn[] COMMANDSET() //commands used to make CommandSet
    {
        AIIn[] set = new AIIn[] {
            new AIIn(null) {CommandIn = new bool[]{ true,false },DirectionalIn = new bool[] {true,false,false,false },cmd=0 },//attack0
            new AIIn(null) {CommandIn = new bool[]{ true,false },DirectionalIn = new bool[] {false,true,false,false },cmd=1  },//attack1
            new AIIn(null) {CommandIn = new bool[]{ true,false },DirectionalIn = new bool[] {false,false,false,false },cmd=2  }
        };
        return set;
    }
    public static EnemyState Load()
    {
        //triggers {attacking,waiting,hit,moving}
        EnemyState status = new EnemyState();

        State idl = new State(0, new System.Func<InputSet, string[]>[] { idle }, status,false);
        idl.link("A0", "Attack0");
        idl.link("A1", "Attack1");
        idl.link("A2", "Attack2");
        idl.link("Hit", "Damaged");
        status.addState("Idle", idl);
        //attacking state
        State attack0 = new State(1, new System.Func<InputSet, string[]>[] { att0 }, status,false);
        attack0.setDefault("Idle");
        status.addState("Attack0", attack0);
        //
        State attack1 = new State(1, new System.Func<InputSet, string[]>[] { att1 }, status, false);
        attack1.setDefault("Idle");
        status.addState("Attack1", attack1);
        //
        State attack2 = new State(1, new System.Func<InputSet, string[]>[] { att2 }, status, false);
        attack2.setDefault("Idle");
        status.addState("Attack2", attack2);
        //
        State hit= new State(10, new System.Func<InputSet, string[]>[] {null, hurt } , status,false);
        hit.link("Hit", "Dead");
        hit.setDefault("Recover");
        status.addState("Damaged",hit);
        //
        State recover = new State(1, new System.Func<InputSet, string[]>[] { invul }, status,false);
        recover.setDefault("Idle");
        status.addState("Recover", recover);
        //death state
        State dead = new State(0, new System.Func<InputSet, string[]>[] { die }, status,false);
        status.addState("Dead", dead);

        status.setInitial("Idle");
        return status;
    }
    private static string[] idle(InputSet raw)
    {
        AIIn In = (AIIn)raw;
        string[] flags;
        if (In.Overrule == AIIn.AbsoluteFlag.Invalid)
        {
            if (In.actor.messageQueue[0]) flags = new string[] { "Hit" };
            else flags = new string[] { };
            return flags;
        }
        In.actor.GetComponent<SpriteRenderer>().color = Color.red;
        if (In.actor.messageQueue[0]) flags = new string[] { "Hit" };
        else if (In.CommandIn[0])
        {
            if (In.DirectionalIn[0]) { flags = new string[] { "A0" }; }
            else if (In.DirectionalIn[1]) { flags = new string[] { "A1" }; }
            else { flags = new string[] { "A2" }; }
        }
        else
        {
            flags = new string[] { };
        }
        return flags;
    }
    private static string[] att0(InputSet raw)
    {
        AIIn In = (AIIn)raw;
        In.actor.GetComponent<SpriteRenderer>().color = Color.white;
        In.actor.messageQueue[1] = In.actor.launchHit(0,In.cmd);
        string[] flags = new string[] { };
        return flags;
    }
    private static string[] att1(InputSet raw)
    {
        AIIn In = (AIIn)raw;
        In.actor.GetComponent<SpriteRenderer>().color = Color.white;
        In.actor.messageQueue[1] = In.actor.launchHit(1,In.cmd);
        string[] flags = new string[] { };
        return flags;
    }
    private static string[] att2(InputSet raw)
    {
        AIIn In = (AIIn)raw;
        In.actor.GetComponent<SpriteRenderer>().color = Color.white;
        In.actor.messageQueue[1] = In.actor.launchHit(2,In.cmd);
        string[] flags = new string[] { };
        return flags;
    }
    private static string[] hurt(InputSet In)
    {
        string[] flags;
        if (In.actor.life <= 0) flags = new string[] { "Hit" };
        else flags = new string[] { };
        return flags;
    }
    private static string[] die(InputSet In)
    {
        ((Enemy0Model)In.actor).over.killUnit(In.actor.id);
        return new string[]{ };
    }
    private static string[] invul(InputSet In)
    {
        In.actor.messageQueue[0] = false;
        return new string[] { };
    }
    //no updates, unit manager will handle all enemy updating
    public static GameObject[] loadAttacks()
    {
        GameObject[] attacks = new GameObject[] {
            Resources.Load("EnemyAttacks/EnemyAttack1", typeof(GameObject)) as GameObject };
        return attacks;
    }
}
