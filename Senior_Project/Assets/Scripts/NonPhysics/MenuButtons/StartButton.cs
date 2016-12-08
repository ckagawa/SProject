using UnityEngine.SceneManagement;
/// <summary>
/// Start screen Button
/// </summary>
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
