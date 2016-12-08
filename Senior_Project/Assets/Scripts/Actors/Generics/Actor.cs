using UnityEngine;
/// <summary>
/// Class for characters --
/// character includes player and any game object with AI behavior
/// </summary>
public abstract class Actor : MonoBehaviour {
    //basic classifications
    public int id { get; set; }//used for identifying actors, shouldnt be used for programmatic generation
    public bool moves;//is mobile
    public bool hasPhysics;//affected by gravity
    public bool destructable;//is destroyable, can be linked to life
    public bool active;//animating
    public float life;
    public bool[] messageQueue;//message queue with 10 flags for communicating outside inputs -- current meanings
    //0: hit
    //1: in action(enemies only)
    public GameObject[] attackSprites;
    //for collidable objects, identifier for affiliation
    public enum UnitGroup {None,Player,Enemy};
    public UnitGroup group;
    public StateMachine sm;//attached state machine
    public int movementGroup;//for movement calculations
    public int width,height;//for calculating collision
    public float gravity;//used for jumping and falling

    // Use this for initialization
    public void Start () {
        //defaults
        messageQueue = new bool[10];
        moves = true;//can move
        hasPhysics = true;//has gravity
        destructable = true;//destructable
        active = true;//is active
        life = 100;

        //must be configured in subclass start()
        group = UnitGroup.None;
        sm = null;
        width = 0;
        height = 0;
        movementGroup = 0;
        gravity = 0;
    }
    /// <summary>
    /// checks hit validity
    /// </summary>
    /// <param name="origin">unit group of hit source</param>
    /// <param name="value">amount of damage</param>
    /// <returns>true if hit is valid, else false</returns>
    public abstract bool checkHit(UnitGroup origin, int value);
    /// <summary>
    /// instantiates attacks
    /// </summary>
    /// <param name="attackIndex">index of attack</param>
    /// <returns>true if an attack was created, else false</returns>
    public abstract bool launchHit(int attackIndex);
    public abstract bool launchHit(int attackIndex,int command);
    /// <summary>
    /// Send attack information to assessor for evaluation
    /// </summary>
    /// <param name="index">index of actor</param>
    /// <param name="value">command code of action</param>
    /// <param name="cmd">index of associated command</param>
    public static void notify(string index, int value, int cmd)
    {
        Assessor.getInstance().update(new UserFeed { time = -1,
            life = value,score = cmd, msg = index
        });
    }
}
