using UnityEngine;
using System.Collections;
using System;

public class PlayerAttack0 :  Attack
{
    protected override Vector2 direction { get { return new Vector2(1, 0); } }//direction of raycast
    protected override int length { get { return 20; } }//distance to raycast for hit
    protected override Vector2 offset { get { return new Vector2(-10, 0); } }//offset from projectile location to raycast origin
    protected override Actor.UnitGroup source { get { return Actor.UnitGroup.Player; } }//unit group of projectile
    protected override int DAMAGE { get { return 20; } }//damage dealt
    protected override int TIME { get { return 5; } }//length of attack
    //override notify and die if needed
    protected override void notify(bool hit)
    {
        if(hit)Actor.notify(null,-1,0);
    }
    // Update is called once per frame
    void FixedUpdate () {
        collide();
        timeout();
	}
}
