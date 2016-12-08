using UnityEngine;
public class StartUM : UnitManager
{
    public override void postwork()
    {
        hud.Out.refresh("Current Target: " + judge.target + " Prediction: " + watcher.target().ToString() + " : " + timer.ToString());
    }

    public override void prework()
    {
    }

    public override void progress()
    {
    }

    public override void reset()
    {
    }

    public override void setup()
    {
        Spawn = new Vector2(0, 0);
    }
}

