using UnityEngine;
/// <summary>
/// Package used by AI to pass user data
/// </summary>
public class UserFeed {
    public Vector2 location { get; set; }
    public float life { get; set; }
    public int score { get; set; }
    public long time { get; set; }
    public string msg { get; set; }
    //more actual data later
}
