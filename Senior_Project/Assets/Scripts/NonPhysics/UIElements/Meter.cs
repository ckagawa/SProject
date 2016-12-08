using UnityEngine;

public class Meter : MonoBehaviour {
	public void refresh(float value)
    {
        transform.localScale = new Vector3(value,1);
    }
}
