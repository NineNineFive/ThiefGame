using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameMenu : MonoBehaviour {
	public void backToMenu() {
		SceneManager.LoadScene(0);
	}
}
