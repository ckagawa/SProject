using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartButton : UnityEngine.UI.Button {

	// Use this for initialization
	protected override void Start()
    {
        this.onClick.AddListener(ThisOnClick);
    }
    void ThisOnClick()
    {
        SceneManager.LoadScene("StartStage");
    }
}
