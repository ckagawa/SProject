using UnityEngine;
using System.Collections;

public class GroundCollider : MonoBehaviour{

    ///simple class, if unit is on ground, grounded should be true
    ///if not, outside elem should set grounded to false
    public bool grounded;
	void Start () {
        grounded = false;
	}

    // no update-- shouldnt matter how many frames on ground so no need to check each frame
    // remember collider needs to have a rigid body
    void OnTriggerEnter2D(Collider2D other)
    {
        grounded = true;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        grounded = false;
    }
}
