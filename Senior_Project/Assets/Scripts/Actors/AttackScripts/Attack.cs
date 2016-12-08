using UnityEngine;

public abstract class Attack : MonoBehaviour {
    private int duration = 0;//counter for timeout
    //these need to be set by subclasses
    protected abstract Vector2 direction { get; }//direction of raycast
    protected abstract int length { get; }//distance to raycast for hit
    protected abstract Vector2 offset { get; }//offset from projectile location to raycast origin
    protected abstract Actor.UnitGroup source { get; }//unit group of projectile
    protected abstract int DAMAGE { get; }//damage dealt
    protected abstract int TIME { get; }//length of attack
    //should find a better way to do this
    ///used for AI interpreter to process hits -- hit if hit, false if timeout
    protected virtual void notify(bool hit)
    {
    }
    ///used for any fizzle/hit sprite creation
    protected virtual void die()
    {
    }
    /// <summary>
    /// Since rigidbodies were removed need a new way to check collision
    /// this will not be as robust(only 1D check) but good enough for this
    /// </summary>
    protected virtual void collide() {
        Vector2 cast = offset;
        cast.x += transform.position.x;
        cast.y += transform.position.y;
        RaycastHit2D other = Physics2D.Raycast(cast,direction,length);
        if(other.collider != null && other.collider.gameObject.GetComponent<Actor>()!=null)
        {
            if (other.collider.gameObject.GetComponent<Actor>().checkHit(source, DAMAGE))
            {
                notify(true);
                die();
                Destroy(gameObject);
                return;
            }
        }
    }
    /// <summary>
    /// All attacks will have a finite lifespan, used to enforce lifespan
    /// </summary>
    protected void timeout()
    {
        ++duration;
        if (TIME < duration)
        {
            die();
            notify(false);
            Destroy(gameObject);
        }
    }
}
