using UnityEngine;

public class Counter : MonoBehaviour {
    public void refresh(int value)
    {
        gameObject.GetComponent<UnityEngine.UI.Text>().text = value.ToString();
    }
}
