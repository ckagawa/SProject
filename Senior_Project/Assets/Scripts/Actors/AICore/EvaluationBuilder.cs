using UnityEngine;
using System.Collections;

public class EvaluationBuilder
{
    //values in key dont matter, strings are meant as descriptors for int
    //inputs from vote -- remove from final builds, waste of memory
    private static string[] key = {
        //matters for don't die
        "Attack Landed","Attack Missed",//0-1
        "Positive Movement","Negative Movement",//2-3
        //matters for getpoints
        "Point Gain",//4
        //matters for kill
        "Player Attack Landed",//5
        "No progress for 1 evaluation period"//6 note-in future need state for no attacks landed
    };
    public static Evaluation Load()
    {
        return new Proxy();
    }
    public static PlayerModel.PlayMode[] Set() { return new PlayerModel.PlayMode[] {PlayerModel.PlayMode.DontDie,
    PlayerModel.PlayMode.Kill,PlayerModel.PlayMode.GetPoints}; }
    private class Proxy : Evaluation
    {
        private ArrayList history;
        private UnifiedAI ai;
        public void Evaluate(PlayerModel.PlayMode mode, int[] value,string src)
        {
            if (ai == null) return;
            //mode is supposed to switch between ai sets but for now will be used for only part of its purpose
            //if
            switch (mode)
            {
                case PlayerModel.PlayMode.DontDie://judgment--hit player good, else bad
                    if (value[0] == 0) ((CommandSet)ai.commands[src]).weigh(value[1], 10, true);
                    else if (value[0] == 1) ;//((CommandSet)ai.commands[src]).weigh(value[1], -1, true) ;
                    break;
                case PlayerModel.PlayMode.GetPoints://judgment--player doesnt score good else bad

                    if (value[0] == 4 || value[0] == 6)
                    {
                        short weight = 2;
                        if (value[0] == 4) weight = -5;
                        int inx = Mathf.Max(getRelevent(6), getRelevent(4));//find commands since last no progress or point
                        if (inx < 0) inx = 0;
                        while (inx < history.Count)
                        {
                            Marker m = (Marker)history[inx];
                            //devalue/value all relevent commands
                            if (m.src != null) ((CommandSet)ai.commands[m.src]).weigh(m.index, weight, true);
                            ++inx;
                        }
                    }
                    break;
                case PlayerModel.PlayMode.Kill://judgment--player can't land attacks good else bad
                    if (value[0] == 5|| value[0] == 6)
                    {
                        short weight = 2;
                        if (value[0] == 5) weight = -1;
                        int inx = Mathf.Max(getRelevent(6), getRelevent(5));//find commands since last no progress or last hit
                        if (inx < 0) inx = 0;
                        while (inx < history.Count)
                        {
                            Marker m = (Marker)history[inx];
                            //devalue/value all relevent commands
                            if(m.src!=null)((CommandSet)ai.commands[m.src]).weigh(m.index, weight, true);
                            ++inx;
                        } 
                    }
                    break;
            }
            DH.ping("Check");
            if (history == null) history = new ArrayList();
            if (history.Count > 30) history.RemoveAt(0);
            Marker log =new Marker() {src = src,code=value[0]};
            if (value.Length > 1) log.index = value[1];
            history.Add(log);
        }
        public void setAI(UnifiedAI commands)
        {
            ai = commands;
        }
        //find everything since the last instance of value in history -1 if not found
        private int getRelevent(int value)
        {
            int ret = -1;
            for(int i = 0; i < history.Count; ++i)
            {
                if (((Marker)history[i]).code == value) ret = i;
            }
            return ret;
        }
        private class Marker
        {
            public string src { get; set; }
            public int index { get; set; }
            public int code { get; set;}//could become int[]
        }
    }
}
