using UnityEngine;
/// <summary>
/// accessor for changing text boxes with numbers
/// </summary>
public class Counter : MonoBehaviour {
    public void refresh(int value)
    {
        gameObject.GetComponent<UnityEngine.UI.Text>().text = value.ToString();
    }
}
