using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class Boss0UM : UnitManager
{
    public override void postwork()
    {
        sys.run();
        hud.Out.refresh("Current Target: "+judge.target+" Prediction: "+watcher.target().ToString()+" : "+timer.ToString());
    }

    public override void prework()
    {
    }

    public override void progress()
    {
        SceneManager.LoadScene("Boss0");
    }

    public override void reset()
    {
        SceneManager.LoadScene("Boss0");
    }

    public override void setup()
    {
        Spawn = new Vector2(50, 50);
        Actor[]a = E0Builder.ACTORS();
        ((Enemy0Model)a[0]).over = sys;
        sys.loadAI(a[0].GetType().Name, new CommandSet(E0Builder.COMMANDSET(), 20));
        sys.loadActors(a);
    }
}
