using UnityEngine;
/// <summary>
/// manager for passing data between actors
/// </summary>
public abstract class UnitManager : MonoBehaviour {
    public GameObject point;
    protected Collectable[] points;
    public BasicCamera cam;//shouldn't be basic specifically but no time to make abstract class
    public PlayerUI hud;
    public PlayerModel Player;//set from unity window
    public Vector2 Spawn;
    //
    public UnifiedAI sys;
    //dont need duplicates of these
    protected long timer;//unlikely but could put error code for overflow
    public static Evaluator judge = Evaluator.getInstance();
    public static Assessor watcher = Assessor.getInstance();
    //something here for stage information on where to spawn stuff

    // Use this for initialization
    void Start ()
    {
        if(point!=null)points = point.GetComponentsInChildren<Collectable>();
        sys = new UnifiedAI();
        judge.link(sys);
        watcher.Out=judge;//this should fail fairly often
        judge.target = watcher.target();
        //generate everything in sprite bank here
        setup();
        Player = Instantiate(Player.gameObject as GameObject).GetComponent<PlayerModel>();
        Player.transform.Translate(Spawn);
        Player.um = this;
        cam=Instantiate(cam.gameObject as GameObject).GetComponent<BasicCamera>();
        cam.setTarget(Player);
        hud =Instantiate(hud.gameObject as GameObject).GetComponent<PlayerUI>();//should figure out at some point how to hook camera in
        timer = 0;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        hud.Health.refresh(Player.life/PlayerModel.maxLife);
        hud.Points.refresh(Player.points);
        //preprocess code goes in prework,checking detection zones etc
        prework();
        if (sys.awake)
        {
            if (timer % 20 == 0)//adjust how often to evaluate
            {
                UserFeed data = new UserFeed();
                data.location = new Vector2(Player.gameObject.transform.position.x, Player.gameObject.transform.position.y);
                data.life = Player.life;
                data.score = Player.points;
                data.time = timer;
                watcher.update(data);
            }
            //what would normally be in fixed update goes here
            postwork();
        }
        ++timer;
    }
    void OnApplicationQuit()
    {
        //thread can survive if not killed when game loads
        while(judge.core.IsAlive)judge.core.Abort();
        DH.ping("Thread Survived:"+judge.core.IsAlive);
    }
    /// <summary>
    /// should be used for loading actors and attack sets
    /// should have file loading for saved ai states
    /// </summary>
    public abstract void setup();
    /// <summary>
    /// subclasses should use this to do any preprocessing -- detection zones etc
    /// </summary>
    public abstract void prework();
    /// <summary>
    /// subclasses should use this for anything that would normally be in fixed update
    /// </summary>
    public abstract void postwork();
    /// <summary>
    /// should reset stage
    /// note- remember to save ai state before resetting
    /// </summary>
    public abstract void reset();
    /// <summary>
    /// advance the stage, may need code for branching paths
    /// note- remember to save ai state before progressing
    /// </summary>
    public abstract void progress();
    public float pointsLeft()
    {
        if (points == null || points.Length < 1) return 0;
        else
        {
            float i = 0;
            foreach(Collectable c in points)
            {
                if (c != null) ++i;
            }
            return i / points.Length;
        }
    }
}
