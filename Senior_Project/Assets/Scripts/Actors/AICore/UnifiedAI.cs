using UnityEngine;
using System.Collections;
/// <summary>
/// Manager for Actors' Command Sets
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
    /// Replacement for the FixedUpdate function
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
    //order a unit
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
    /// <summary>
    /// destroy an Actor in the stage
    /// </summary>
    /// <param name="ident">index of Actor</param>
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
    //unimplemented file loader
    private void loadFile()
    {
        //for loading, insert code for reading in
    }
    //unimplemented file writer
    private void saveFile()
    {

    }
    /// <summary>
    /// Unimplemented --
    /// change command sets to new target PlayMode
    /// </summary>
    public void switchTarget()
    {
        //change between either Unified AIs or command sets here
    }
    //represents a npc on screen
    private class Unit
    {
        public Actor actor { get; set; }
        public CommandSet.Entry entry { get; set; }
        
    }
}
