using UnityEngine;
using System.Collections;

public class TransitionTrigger : MonoBehaviour {

    protected Vector2 direction { get { return new Vector2(0, 1); } }//direction of raycast
    protected int length { get { return 6; } }//distance to raycast for hit
    protected Vector2 offset { get { return new Vector2(0, -3); } }//offset from projectile location to raycast origin
    public int type;
    // Update is called once per frame
    void FixedUpdate()
    {
        //augment shape or other properties here
        collide();
    }
    /// <summary>
    /// Since rigidbodies were removed need a new way to check collision
    /// this will not be as robust(only 1D check) but good enough for this
    /// </summary>
    protected virtual void collide()
    {
        Vector2 cast = offset;
        cast.x += transform.position.x;
        cast.y += transform.position.y;
        RaycastHit2D other = Physics2D.Raycast(cast, direction, length);
        if (other.collider != null && other.collider.gameObject.GetComponent<PlayerModel>() != null)
        {
            if (gameObject.GetComponent<SpriteRenderer>().color != Color.black) gameObject.GetComponent<SpriteRenderer>().color = Color.black;
            else gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            UnitManager.watcher.cheat(20,type);
            DH.ping(UnitManager.watcher.target()+" : is Target");
            return;
        }
    }
}
