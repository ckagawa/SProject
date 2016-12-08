using UnityEngine;
/// <summary>
/// Class for camera which loosely tracks player
/// </summary>
public class BasicCamera : MonoBehaviour {
    private PlayerModel target;
    private static float yield = 3f;//tracking deadzone, will only adjust if more than this much off center
    void FixedUpdate()
    {
        //code to make the camera follow the player
        if (target == null) return;
        Vector2 update = target.transform.position - transform.position;
        if (update.magnitude > yield)
        {
            float offset = update.magnitude - yield;
            update.Normalize();
            update.Scale(new Vector2(offset,offset));
            transform.Translate(update);
        }
    }
    public void setTarget(PlayerModel p)
    {
        target = p;
    }
}
