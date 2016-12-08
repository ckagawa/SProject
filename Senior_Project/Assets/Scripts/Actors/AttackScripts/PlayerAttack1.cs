using UnityEngine;

public class PlayerAttack1 :  Attack
{
    protected override Vector2 direction { get { return new Vector2(1, 0); } }//direction of raycast
    protected override int length { get { return 12; } }//distance to raycast for hit
    protected override Vector2 offset { get { return new Vector2(-6, 0); } }//offset from projectile location to raycast origin
    protected override Actor.UnitGroup source { get { return Actor.UnitGroup.Player; } }//unit group of projectile
    protected override int DAMAGE { get { return 20; } }//damage dealt
    protected override int TIME { get { return 90; } }//length of attack

    private static float speed = 3;
    //override notify and die if needed
    protected override void notify(bool hit)
    {
        if (hit) Actor.notify(null, -1,0);
    }
    void FixedUpdate()
    {
        if (gameObject.GetComponent<SpriteRenderer>().flipX) transform.Translate(-speed, 0, 0);
        else transform.Translate(speed, 0, 0);//set positive when done debugging
        collide();
        timeout();
    }
}
