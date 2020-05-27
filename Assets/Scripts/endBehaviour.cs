using System.Collections;
using UnityEngine;

public class endBehaviour : MonoBehaviour {
	private void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
			if (GameManager.getInstance().endingPrevented) {
				UserInterface.instance.playerMessage.text = "Cant end with guard following!";
			} else {
				UserInterface.instance.playerMessage.text = "Press Enter to end game";
				if (Input.GetKey(KeyCode.Return)) {
					GameManager.getInstance().endGame();
				}
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
			UserInterface.instance.playerMessage.text = "";
		}
	}
}
