using UnityEngine;
/// <summary>
/// Builders --
/// builders carry construction information for game elements that in a final build
/// would be created through file loading
/// </summary>
public class Builder{
    //fill with whatever stats
    // --0: player
    protected static int gravLimit = 10;//bound on gravity, -gravLimit < gravity < gravlimit
    //stats for different unit categories, index 0: move speed, 1: jump modifier, 2: jump strength
    protected static float[][] statBank = {
        new float[] { 1.5f, .5f, 7f },//player move group
        new float[] { 1.5f, .5f, 7f },//enemy move group0 --
    };
    //for whatever reason left-side geometry calculates differently than right, need to account for that while modeling
    protected static bool move(bool right, Actor actor)
    {
        bool succeeded;
        float move = statBank[actor.movementGroup][0];
        Vector2 drct;
        Vector3 start = actor.transform.position;
        if (right)
        {
            drct = new Vector2(1, 0);
            start.x += actor.width;
        }
        else
        {
            drct = new Vector2(-1, 0);
            start.x -= actor.width;
            move = -move;
        }
        drct.Normalize();
        RaycastHit2D collision = Physics2D.Raycast(start, drct, move);
        if (collision.collider == null)
        {
            actor.transform.Translate(move, 0, 0);
            succeeded = true;
        }//fix this in future, will create bug that allows walking through thin walls
        else
        {
            actor.transform.Translate(move * collision.fraction, 0, 0);
            succeeded = false;
        }
        
        if (move != 0) actor.GetComponent<SpriteRenderer>().flipX = !right;
        return succeeded;
    }
    //add upwards momentum -- also handles gravity
    protected static void jump(Actor actor)
    {
        if (actor.gravity > gravLimit) actor.gravity = gravLimit;
        else if (actor.gravity < -gravLimit) actor.gravity = -gravLimit;
        float move = statBank[actor.movementGroup][1];
        Vector2 drct;
        Vector3 start = actor.transform.position;
        if (actor.gravity<0)
        {
            drct = new Vector2(0, -1);
            start.y -= actor.height;
        }
        else
        {
            drct = new Vector2(0,1);
            start.y += actor.height;
        }
        drct.Normalize();
        RaycastHit2D collision = Physics2D.Raycast(start, drct, move * actor.gravity);
        //creates bug where player can land on projectiles, be careful not to break collision when fixing
        if (collision.collider == null )
        {
            actor.transform.Translate(new Vector3(0, move * actor.gravity, 0));
        }
        else
        {
            //fix this in future, could create bug that allows walking through thin walls
            actor.transform.Translate(new Vector3(0, move * actor.gravity * collision.fraction, 0));
            actor.gravity %= 1;//dont remove this, this is to start player falling when bonks off ceilings
        }
        actor.gravity -= .2f;
    }
    protected static void surface(Actor actor)
    {
        Vector3 start = actor.transform.position;
        start.y -= actor.height;
        Vector2 dir = new Vector2(1, 0);
        dir.Normalize();
        RaycastHit2D collision = Physics2D.Raycast(start, dir, actor.height);
        if (collision.collider != null) actor.transform.Translate(new Vector3(0, actor.height * collision.fraction, 0));
    }
}
