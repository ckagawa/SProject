using UnityEngine;

public class DebugText : MonoBehaviour {
    public void refresh(string value)
    {
        gameObject.GetComponent<UnityEngine.UI.Text>().text = value;
    }
}
