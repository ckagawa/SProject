using UnityEngine;
using System;
/// <summary>
/// class for Enemy0 actors
/// </summary>
public class Enemy0Model : Actor {
    public UnifiedAI over { get; set; }//whatever unit manager is overseeing the enemy
    // remove start from final build, enemies should be loaded from file, not generated programmatically
    new void Start ()
    {
        base.Start();
        moves = false;//can move
        hasPhysics = false;//has gravity
        destructable = true;//destructable
        active = true;//is active
        life = 100;

        group = UnitGroup.Enemy;
        sm = E0Builder.Load();
        attackSprites = E0Builder.loadAttacks();
        //no width/height
    }
    public override bool checkHit(UnitGroup origin, int value)
    {
        if (origin == UnitGroup.Player)
        {
            if (!messageQueue[0])
            {
                life -= value;
                messageQueue[0] = true;
                DH.ping("ValidHit");
            }
            return true;
        }
        else return false;
    }
    public override bool launchHit(int attackIndex,int command)
    {
        GameObject attack;
        //will involve sending attack to evaluator in the future, for now isnt used
        switch (attackIndex){
            case 0:
                attack = Instantiate(attackSprites[0]);
                attack.GetComponent<SpriteRenderer>().flipX = gameObject.GetComponent<SpriteRenderer>().flipX;
                attack.GetComponent<EnemyAttack1>().src = this;
                attack.GetComponent<EnemyAttack1>().cmd = command;
                attack.transform.Translate(gameObject.transform.position);
                //DH.ping("0");
                break;
            case 1:
                attack = Instantiate(attackSprites[0]);
                attack.GetComponent<SpriteRenderer>().flipX = !gameObject.GetComponent<SpriteRenderer>().flipX;
                attack.GetComponent<EnemyAttack1>().src = this;
                attack.GetComponent<EnemyAttack1>().cmd = command;
                attack.transform.Translate(gameObject.transform.position);
                //DH.ping("1");
                break;
            case 2://is bugged but can't fix right now, weighs twice
                attack = Instantiate(attackSprites[0]);
                attack.GetComponent<SpriteRenderer>().flipX = !gameObject.GetComponent<SpriteRenderer>().flipX;
                attack.GetComponent<EnemyAttack1>().src = this;
                attack.GetComponent<EnemyAttack1>().cmd = command;
                attack.transform.Translate(gameObject.transform.position);
                attack = Instantiate(attackSprites[0]);
                attack.GetComponent<SpriteRenderer>().flipX = gameObject.GetComponent<SpriteRenderer>().flipX;
                attack.GetComponent<EnemyAttack1>().src = this;
                attack.GetComponent<EnemyAttack1>().cmd = command;
                attack.transform.Translate(gameObject.transform.position);
                //DH.ping("2");
                break;
        }
        return true;
    }

    public override bool launchHit(int attackIndex)
    {
        throw new NotImplementedException();
    }
}
