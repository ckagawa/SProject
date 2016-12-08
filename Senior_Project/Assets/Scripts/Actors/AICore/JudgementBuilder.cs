using UnityEngine;
using System.Collections;
using System;

public class JudgementBuilder
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
        "No progress for 1 evaluation period"//6
    };
    public static Judgment Load()
    {
        return new Proxy();
    }
    private class Proxy : Judgment
    {
        private ArrayList history;//should be used in the future
        private Vector2 lastLoc;
        private int score;
        private long lastPoint;
        private float health;
        private long lastAttack;//when ai attacked
        private long lastHit;//when player attacked--doesnt do anything now
        private int dontdiescore;
        private int killscore;
        private int pointscore;
        private static int wait = 180;//how much time must pass before an inaction vote is cast
        public Proxy()
        {
            dontdiescore = 0;
            killscore = 0;
            pointscore =  0;
            lastHit = 0;
            lastAttack = 0;
            lastPoint = 0;
        }
        /// <summary>
        /// ideally should actually have had multiple values
        /// because of time limitations does not
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Assessor.Package assess(UserFeed input)
        {
            Assessor.Package ret = null;
            if (lastLoc == null) lastLoc = input.location;
            if (input.time == -1)//if true, not an update
            {
                if(input.life<0)//player hit
                {
                    ++killscore;
                    return new Assessor.Package {data = new int[]{ 5,input.score } };
                }
                else if(input.life>0)//ai hit
                {
                    //dont die score might increment here
                    return new Assessor.Package { src = input.msg, data = new int[] { 0,input.score } };
                }
                else
                {
                    return new Assessor.Package { src = input.msg, data = new int[] { 1,input.score } };
                }
            }
            //bunch of resets for when new stage starts
            if (input.time < lastHit)
            {
                health = input.life;
                lastHit = input.time;
            }
            if (input.time < lastAttack) lastAttack = input.time;
            if (input.time < lastPoint)
            {
                score = input.score;
                lastPoint = input.time;
            }
            if (input.life < health)
            {
                health = input.life;
                lastAttack = input.time;
                //not computing time factors
                //would have some value here to modify evaluation based on how long often hits occur
            }
            if(input.score!= score)
            {
                score = input.score;
                //should have code here for judging based on % collectables collected
                pointscore+=2;
                return new Assessor.Package { data = new int[] { 4 } };//this shouldnt be a return statement but no time
            }
            //there should be something about movement here
            if(lastPoint+wait<input.time&&lastAttack+wait<input.time)//differentiate later
            {
                ret = new Assessor.Package { data = new int[] { 6 } };
                lastAttack = input.time;
                ++dontdiescore;
            }
            return ret;
        }
        public void cheat(int i, int index)
        {
            if (index == 1) killscore+=i;
            else if (index == 2) dontdiescore+=i;
            else pointscore+=i;
            monitor();
        }
        private void monitor()
        {
            if(dontdiescore>100||killscore>100||pointscore>100)
            {
                dontdiescore /= 2;
                killscore /= 2;
                pointscore /= 2;
            }
        }
        public PlayerModel.PlayMode target()
        {
            if (killscore > dontdiescore)
            {
                if (pointscore > killscore) return PlayerModel.PlayMode.GetPoints;
                else return PlayerModel.PlayMode.Kill;
            }
            else if (pointscore > dontdiescore)
            {
                return PlayerModel.PlayMode.GetPoints;
            }
            else return PlayerModel.PlayMode.DontDie;
        }
    }
}
