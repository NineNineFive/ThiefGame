using UnityEngine;

public class KeyBehaviour : MonoBehaviour {
    public string password;
    public Key key;

    public void Start() {
        key = new Key(password);
    }

    public void OnCollisionEnter2D(Collision2D other) {
        //if (!PlayerController.playerInventory.checkForItem("key")){
        key.name = password;
        
        if (other.gameObject.CompareTag("Player")) {
            PlayerController.playerInventory.addItem(key);
            gameObject.SetActive(false);
        }

        //}
        PlayerController.playerInventory.showInventory();
    }
}