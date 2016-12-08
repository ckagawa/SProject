/// <summary>
/// Input Set used for passing AI commands
/// </summary>
public class AIIn : InputSet {

    //override flags
    public enum AbsoluteFlag { None, Pause, Kill, Invalid }
    public int cmd { get; set; }
    new public bool[] DirectionalIn { get; set; }
    new public bool[] CommandIn { get; set; }
    new public AbsoluteFlag Overrule { get; set; }
    /// <summary>
    /// Basic Constructor
    /// </summary>
    /// <param name="actor">Actor to perform command</param>
    public AIIn(Actor actor)
    {
        //directional block
        this.actor = actor;
    }
}
