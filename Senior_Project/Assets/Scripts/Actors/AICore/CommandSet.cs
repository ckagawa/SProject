using UnityEngine;
/// <summary>
/// Representation of an Artificial Intelligence script
/// </summary>
public class CommandSet{
    //shouldn't be here but necessary to avoid all values capping and breaking AI
    //should be fairly low to only permit a few high priority commands, must be 0<x<1
    //bad way of doing this, fix in future
    private static float SLoad = .2f;
    //max of 100 commands bc counters are shorts 
    //and also scrub would break if sum of all shorts could exceed max int value
    Entry[] cmds;
    short[] achievement;//fitness of actions for achieving goals
    short[] support;//fitness of actions for assisting in achieving goals
    short baseSize;
    short lastValid;
    private static short ADJUSTVALUE = 50;//value used when combining
    /// <summary>
    /// Basic Constructor
    /// </summary>
    /// <param name="commands">Array of initial commands</param>
    /// <param name="setSize">Maximum number of Commands</param>
    public CommandSet(AIIn[] commands, short setSize)
    {
        if (setSize < commands.Length||commands == null||setSize>100) throw new System.ArgumentException();
        cmds = new Entry[setSize];
        achievement = new short[setSize];
        support = new short[setSize];
        baseSize = 0;
        while (baseSize < commands.Length)
        {
            cmds[baseSize] = new Entry();
            cmds[baseSize].index = baseSize;
            cmds[baseSize].setEntry(commands[baseSize]);
            support[baseSize] = 0;
            achievement[baseSize] = 0;
            ++baseSize;
        }
        lastValid = (short)(baseSize-1);
    }
    /// <summary>
    /// Gets next Action
    /// </summary>
    /// <returns>package containing commands for next action</returns>
	public Entry next()
    {
        //theres a $12 weighted random number plugin on the asset store, will consider that after project is over
        float rng = lastValid+1;
        foreach (short e in achievement)
        {
            rng += e;
        }
        rng *= Random.value;
        int choice = 0;
        while ( choice<lastValid)
        {
            rng -= (achievement[choice]+1);
            if (0 >= rng) break;
            else ++choice;
        }
        return cmds[choice];
    }
    /// <summary>
    /// Getter for command fitness ratings
    /// </summary>
    /// <param name="index">index of command</param>
    /// <param name="achieve">true if desire achievement fitness, else support fitness</param>
    /// <returns></returns>
    public int check(int index, bool achieve)
    {
        if (achieve) return achievement[index];
        else return support[index];
    }
    /// <summary>
    /// Alter command fitness rating
    /// </summary>
    /// <param name="index">index of command</param>
    /// <param name="value">amount to change by</param>
    /// <param name="achieve">true if achievement fitness, else support</param>
    public void weigh(int index,short value,bool achieve)
    {
        int score;
        if (achieve) score = achievement[index];
        else score = support[index];
        if (score+value > short.MaxValue)
        {
            if (achieve)
            {
                achievement[index] = short.MaxValue;
                scrub(achievement);
            }
            else
            {
                support[index] = short.MaxValue;
                scrub(support);
            }
        }
        else
        {
            if (achieve)
            {
                achievement[index] += value;
                if (achievement[index] < 0) achievement[index] = 0;
            }
            else
            {
                support[index] += value;
                if (support[index] < 0) support[index] = 0;
            }
        }
        DH.ping("Set "+index + " to " + achievement[index]);
    }
    /// <summary>
    /// Used to combine existing commands into new ones
    /// currently non-functional
    /// if indices contain more commands than can be made into one sequence
    /// discard starting from first of first and going towards last of second
    /// </summary>
    /// <param name="first">index of commands to be performed first</param>
    /// <param name="second">index of commands to be perfomed after</param>
    /// <returns>true if a new command was created</returns>
    public bool combine(short first, short second)
    {/*
        if (lastValid+1>=cmds.Length) return false;
        //could probably put code here to replace least successful command
        //but for now nothing new once full
        else
        {
            ++lastValid;
            cmds[lastValid] = new EntrySet(cmds[first],cmds[second]);
            support[lastValid] = 0;
            achievement[lastValid] = ADJUSTVALUE;
            weigh(first, (short)-ADJUSTVALUE, false);
            weigh(second, (short)-ADJUSTVALUE, true);
            return true;
        }removed because command indexing*/
        return false;
    }
    //keeps values within expected range
    private void scrub(short[] target)
    {
        int chk = 0;
        int total = 0;
        for (int i = 0; i <= lastValid; ++i)
        {
            chk += short.MaxValue;
            total += target[i];
        }
        if (total>(chk*SLoad))
        {
            for (int i = 0; i <= lastValid; ++i)
            {
                target[i] = (short)Mathf.Round(target[i] / (2 * total));
            }
        }
    }
    /// <summary>
    /// helper classes to hold command entries
    /// </summary>
    public class Entry
    {
        public int index { get; set; }
        private AIIn cmd;
        private bool f = false;
        public virtual void setEntry(AIIn In)
        {
            cmd = In;
        }
        public virtual bool flag()
        {
            f = !f;
            return f;
        }
        public virtual AIIn getCmd()
        {
            return cmd;
        }
    }
    public class EntrySet : Entry
    {
        private AIIn[] cmd = new AIIn[5];
        private int nxt = 0;
        private int lst = 0;
        public EntrySet(Entry a, Entry b)
        {
            while (a.flag())
            {
                setEntry(a.getCmd());
            }
            while(b.flag())
            {
                setEntry(a.getCmd());
            }
        }
        override public void setEntry(AIIn In)
        {
            if (lst > 4)
            {
                cmd = new AIIn[] { cmd[1], cmd[2], cmd[3], cmd[4], In };
            }
            else
            {
                cmd[lst] = In;
                ++lst;
            }
        }
        override public bool flag()
        {
            if (nxt >= lst)
            {
                nxt = 0;
                return false;
            }
            else return true;
        }
        override public AIIn getCmd()
        {
            return cmd[nxt++];
        }
    }
}
