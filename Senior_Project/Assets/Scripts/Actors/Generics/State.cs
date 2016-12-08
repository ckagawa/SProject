using System.Collections;
/// <summary>
/// class representing a state within a Finite State machine
/// </summary>
public class State {
    protected string nxt;//state to be transitioned to
    protected bool end;//boolean state change, true if new state is queued
    protected int duration;//length of state in frames, 0 and -1 special cases
    protected System.Func<InputSet, string[]>[] command;//length of state in frames
    protected int progress;//number of frames progressed so far
    public bool IsValid { get; set; }//check for validity, for states with transition requirements
    protected Hashtable possible;//array of valid transitions;
    protected bool redun;

    protected StateMachine sm;
    public void setStateMachine(StateMachine Manager) { sm = Manager; }

    public static State INVALID = new State();//constant for broken path handling
    protected State() { duration = -1; IsValid = false; }
    /// <summary>
    /// representation of a state. functions must take an InputSet and return a list of strings representing
    /// triggers. triggers may be used to flag premature termination of 
    /// </summary>
    /// <param name="Duration"> length of state in frames, must be >=0, value 0 unique for continual looping</param>
    /// <param name="Scripting">array of functions to be called</param>
    /// <param name="Manager">StateMachine associated with</param>
    public State(int Duration, System.Func<InputSet,string[]>[] Scripting,StateMachine Manager,bool redundant)
    {
        if (Duration < 0) throw new System.ArgumentOutOfRangeException();
        if (Scripting == null) throw new System.ArgumentNullException();
        nxt = null;
        end = false;
        duration = Duration;
        command = Scripting;
        progress = 0;
        IsValid = true;
        redun = redundant;
        sm = Manager;//may be null but will throw exception if advance is called
        possible = new Hashtable();
        possible.Add("Default",null);
    }
    /// <summary>
    /// processes a frame of input
    /// </summary>
    /// <param name="In">inputs for the current frame</param>
    public virtual void advance(InputSet In)
    {
        //advance requires a state machine
        if (In == null) throw new System.ArgumentNullException();
        if (sm == null) throw new System.InvalidOperationException("Operation cannot be called without an attached StateMachine");
        if (redun&&command.Length>0)
        {
            string[] flags = command[0](In);
            foreach (string trigger in flags)
            {
                if (possible.ContainsKey(trigger)) end = validate((string)possible[trigger]);
            }
        }
        else if(progress < command.Length && command[progress] != null)
        {
            string[] flags = command[progress](In);
            foreach (string trigger in flags)
            {
                if (possible.ContainsKey(trigger)) end = validate((string)possible[trigger]);
            }
        }
        if (duration > 0) { ++progress; }
    }
    /// <summary>
    /// checks for state transition
    /// </summary>
    /// <returns>true if a state transtion is queued else false</returns>
    public bool update()
    {
        if (progress>= duration && duration > 0) end = true;
        return end;
    }
    /// <summary>
    /// Resets to base state
    /// </summary>
    /// <returns>next state key</returns>
    public string next() {
        string nextState = nxt;
        //reset state
        progress = 0;
        end = false;
        if (possible["Default"] == null) nxt = null; else nxt = (string)possible["Default"];
        return nextState;
    }
    /// <summary>
    /// bind a state that may be transtioned to
    /// </summary>
    public void link(string trigger, string follow)
    {
        if (!possible.ContainsKey(trigger)) possible.Add(trigger, follow);
    }
    /// <summary>
    /// 
    /// </summary>
    public void setDefault(string basecase)
    {
        possible["Default"] = basecase;
        nxt = basecase;
    }
    //check if state can be transitioned to
    protected bool validate(string candidate)
    {
        if(sm.getState(candidate).IsValid){
            nxt = candidate;
            return true;
        }
        return false;
    }
}
