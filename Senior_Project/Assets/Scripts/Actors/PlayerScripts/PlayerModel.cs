using System;
using UnityEngine;
/// <summary>
/// Class for Player-controlled Actor
/// </summary>
public class PlayerModel : Actor {

    public UnitManager um;//will be null in unity window
    public GroundCollider land;//for collision
    public Animator motion;//will probably not have time to reimplement
    public enum PlayMode {DontDie,Kill,GetPoints};

    public int refire1;//flag to check if attack 1 can be fired

    public static float maxLife = 100;
    public int points;
    // Use this for initialization
    new void Start () {
        //super
        sm = PlayerBuilder.Load();
        motion = GetComponent<Animator>();
        
        //TODO add action handler and ui
        group = UnitGroup.Player;
        //TODO determine start state
        life = maxLife;//change this later
        points = 0;
        height = 12;
        width = 6;
        attackSprites = loadAttacks();
        refire1 = 0;
    }
	void FixedUpdate () {
        live();
        sm.next(new UserIn(this));
        if (refire1 > 0) --refire1;
        //playerUI.updateHealth(life);
	}
    public override bool checkHit(UnitGroup origin, int value)
    {
        if (origin != UnitGroup.Player)
        {
            life -= value;
            return true;
        }
        else return false;
    }
    private static GameObject[] loadAttacks()
    {
        GameObject[] attacks = new GameObject[] {
            Resources.Load("PlayerAttacks/PlayerAttack0", typeof(GameObject)) as GameObject ,
            Resources.Load("PlayerAttacks/PlayerAttack1", typeof(GameObject)) as GameObject};
        return attacks;
    }
    public override bool launchHit(int attackIndex)
    {
        if (attackSprites.Length < attackIndex) return false;
        GameObject attack = Instantiate(attackSprites[attackIndex]);
        switch (attackIndex)
        {
            case 0:
                attack.transform.Translate(transform.position);//set position based on player
                float offset = 24;//offset position based on facing and attack particulars
                if (GetComponent<SpriteRenderer>().flipX) attack.transform.Translate(-offset, 0, 0);
                else attack.transform.Translate(offset, 0, 0);
                break;
            case 1:
                if (refire1>0)
                {
                    Destroy(attack);
                    break;
                }
                attack.transform.Translate(transform.position);//set position based on player
                                                               //offset position based on facing and attack particulars
                attack.GetComponent<SpriteRenderer>().flipX = GetComponent<SpriteRenderer>().flipX;
                refire1 = 60;
                break;
        }
        //change position and direction to match player
        return true;
    }
    private void live()
    {
        if (life <= 0)
        {
            Destroy(gameObject);
            um.reset();
        }
    }

    public override bool launchHit(int attackIndex, int command)
    {
        throw new NotImplementedException();
    }
}
