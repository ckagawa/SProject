using UnityEngine;
using System.Collections;
/// <summary>
/// class in charge of figuring out what player is currently trying to do
/// </summary>
// TODO--change this structure if desired
// currently loads a single Unified AI on stage load
// ideally would load commands table for each player mode 
// too little time to implement at the moment
public class UnifiedAI{

    public string fileLocation;//location load file from
    
    public Hashtable commands = new Hashtable();// object type -> commandset
    private Unit[] units;//array of all actual actors
    public bool awake = true;//true if any unit is active, else false
    /// <summary>
    /// functionally a replacement for the fixed update function
    /// </summary>
    public void run()
    {
        for(int i = 0; i< units.Length; ++i)
        {
            if (units[i] != null && units[i].actor.active)
            {
                giveOrders(i);
            }
        }
    }
    //
    private void giveOrders(int index)
    {
        //if cannot act -- used for sending blank commands
        if (units[index].actor.messageQueue[1])
        {
            units[index].actor.sm.next(new AIIn(units[index].actor) {Overrule = AIIn.AbsoluteFlag.Invalid});
            return;
        }
        //for units' first time running
        if (units[index].entry==null) units[index].entry = ((CommandSet)commands[units[index].actor.GetType().Name]).next();
        AIIn order;
        //reroll till a valid command comes up
        while (!units[index].entry.flag())
        {
            units[index].entry = ((CommandSet)commands[units[index].actor.GetType().Name]).next();
        }
        order = units[index].entry.getCmd();
        order.actor = units[index].actor;
        units[index].actor.sm.next(order);
    }
    public void killUnit(int ident)
    {
        for(int i = 0; i < units.Length; ++i)
        {
            if (units[i] != null)
            {
                if (units[i].actor.id == ident)
                {
                    Object.Destroy(units[i].actor.gameObject);
                    DH.ping("killed it");
                    //not sure how c# garbage collection works, might create problems
                    units[i] = null;
                    break;
                }
            }
        }
    }
    /// <summary>
    /// load command sets
    /// </summary>
    /// <param name="key">TypeName of associated class</param>
    /// <param name="val">command set for that class</param>
    public void loadAI(string key, CommandSet val)
    {
        commands.Add(key, val);
    }
    /// <summary>
    /// load all the actors for the stage
    /// </summary>
    /// <param name="actors"></param>
    public void loadActors(Actor[] actors)
    {
        Unit[] bank = new Unit[actors.Length];
        int counter = 0;
        foreach(Actor a in actors)
        {
            if (commands.ContainsKey(a.GetType().Name))
            {
                a.id = counter;
                bank[counter] = new Unit() {actor = a};
                ++counter;
            }
        }
        units = bank;
    }
    /// <summary>
    /// imlement later, used to load ai progress
    /// </summary>
    private void loadFile()
    {
        //for loading, insert code for reading in
    }
    //implement later, used to load ai progress
    private void saveFile()
    {

    }
    /// <summary>
    /// implement later, used to change Ai sets
    /// when player objective changes, hash table should as well
    /// may be difficult with how updates are currently handled
    /// </summary>
    public void switchTarget()
    {
        //change between either Unified AIs or command sets here
    }
    private class Unit
    {
        public Actor actor { get; set; }
        public CommandSet.Entry entry { get; set; }
        
    }
}
