using UnityEngine;
/// <summary>
/// class for debug ui window
/// </summary>
public class DebugText : MonoBehaviour {
    public void refresh(string value)
    {
        gameObject.GetComponent<UnityEngine.UI.Text>().text = value;
    }
}
