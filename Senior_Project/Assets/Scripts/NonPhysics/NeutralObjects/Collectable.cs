using UnityEngine;
/// <summary>
/// Class for Collectable objects
/// </summary>
public class Collectable : MonoBehaviour {

    protected Vector2 direction { get { return new Vector2(0, 1); } }//direction of raycast
    protected int length { get { return 6; } }//distance to raycast for hit
    protected Vector2 offset { get { return new Vector2(0, -3); } }//offset from projectile location to raycast origin
    // Update is called once per frame
    void FixedUpdate()
    {
        //augment shape or other properties here
        collide();
    }
    // Since rigidbodies were removed need a new way to check collision
    // this will not be as robust(only 1D check) but good enough for this
    protected virtual void collide()
    {
        Vector2 cast = offset;
        cast.x += transform.position.x;
        cast.y += transform.position.y;
        RaycastHit2D other = Physics2D.Raycast(cast, direction, length);
        if (other.collider != null && other.collider.gameObject.GetComponent<PlayerModel>() != null)
        {
            other.collider.gameObject.GetComponent<PlayerModel>().points += 100;
            Destroy(gameObject);
            return;
        }
    }
}
