using UnityEngine;
using System.Collections;
using System;

public class EnemyAttack1 : Attack
{
    protected override Vector2 direction { get { return new Vector2(0,1); } }//direction of raycast
    protected override int length { get { return 12; } }//distance to raycast for hit
    protected override Vector2 offset{ get{ return new Vector2(0, -6); } }//offset from projectile location to raycast origin
    protected override Actor.UnitGroup source { get { return Actor.UnitGroup.Enemy; } }//unit group of projectile
    protected override int DAMAGE { get { return 20; } }//damage dealt
    protected override int TIME { get { return 60; } }//length of attack

    private static float speed = 4;//move speed
    public Actor src { get; set; }//whatever launched the attack
    public int cmd { get; set; }//index of command that launched attack

    protected override void die()
    {
        src.messageQueue[1] = false;
    }
    protected override void notify(bool hit)
    {
        DH.ping ("sent");
        if (hit) Actor.notify(src.GetType().Name,1,cmd);
        else Actor.notify(src.GetType().Name,0,cmd);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //augment shape or other properties here
        if (gameObject.GetComponent<SpriteRenderer>().flipX) transform.Translate(-speed, 0, 0);
        else transform.Translate(speed, 0, 0);//set positive when done debugging
        collide();
        timeout();
    }
}
