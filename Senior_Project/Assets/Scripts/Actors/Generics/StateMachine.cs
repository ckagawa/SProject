using System.Collections;
/// <summary>
/// Basic Finite State Machine
/// takes states provided by user as well as string identifiers
/// all states should be added durring initialization, states may not be removed
/// states that are conditional should be handled using Input Sets within State class
/// base state is optional but if none is provided initial state will be treated as default
/// </summary>
public class StateMachine
{
    //contains your states
    private Hashtable StateSet;
    //required start state
    private string initial;
    //optional base state
    private string neutral;
    private State current;

    public ICollection getStates() { return StateSet.Keys; }
    public State getState(string Key) { if (StateSet.ContainsKey(Key)) return (State)StateSet[Key]; else return State.INVALID; }
    public State getBaseState() {if(StateSet.ContainsKey(neutral)&&neutral!=null){ return (State)StateSet[neutral]; } else return null; }

    public StateMachine()
    {
        StateSet = new Hashtable();
        neutral = null;
        initial = null;
        current = null;
    }
    public StateMachine(string Key,State BaseState)
    {
        if(Key==null||BaseState==null)
        {
            throw new System.ArgumentNullException();
        }
        StateSet = new Hashtable();
        StateSet.Add(Key,BaseState);
        neutral = Key;
        initial = Key;
        current = null;
    }
    //all additions should not function after first next call
    public bool addState(string Key, State Value)
    {
        if (Key == null || Value==null || current!= null)
        {
            return false;
        }
        StateSet.Add(Key, Value);
        return (StateSet.ContainsKey(Key) && StateSet[Key]!=null);
    }
    //define starting state, will not function without
    public bool setInitial(string Key)
    {
        if (Key == null || !StateSet.ContainsKey(Key))
        {
            return false;
        }
        else
        {
            initial = Key;
            return true;
        }
    }
    //StateMachine Iteration
    public void next(InputSet In)
    {
        //make sure theres something to work with
        if(StateSet.Count<1|| initial == null ||In == null)
        {
            return;
        }
        if(current==null)
        {
            current = (State)StateSet[initial];
        }
        //run the current state
        current.advance(In);
        //update to next state
        try
        {
            if (current.update()) { current = (State)StateSet[current.next()]; }
        }
        catch (System.Exception)
        {
            current = State.INVALID;
        }
        finally
        {
            if (current == State.INVALID) { current = (State)StateSet[initial]; }
        }
    }
}