using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Class for Scene load triggers
/// </summary>
public class SceneTransition : MonoBehaviour
{
    public string scenePath;
    public UnitManager unit;
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
        if(unit.Player.transform.position.x>=transform.position.x)
            SceneManager.LoadScene(scenePath);
    }
}
