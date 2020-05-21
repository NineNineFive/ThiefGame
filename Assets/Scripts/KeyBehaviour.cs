using UnityEngine;

public class KeyBehaviour : MonoBehaviour {
    public string password;
    public Key key;
    private Sprite sprite;
    public float worth;
    public void Start() {
        sprite = GetComponent<SpriteRenderer>().sprite;
        key = new Key(password, sprite, worth);
    }

    public void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            UserInterface.instance.playerMessage.text = "Press Spacebar to pick up";
            if(Input.GetKey(KeyCode.Space)){
                PlayerController controller = other.gameObject.GetComponent<PlayerController>();
                controller.inventory.addItem(key);
                UserInterface.instance.updateInventory();
                AudioManager.instance.Play("heavyItem");
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            UserInterface.instance.playerMessage.text = "";
        }
    }
    /*
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            PlayerController controller = other.gameObject.GetComponent<PlayerController>();
            controller.nearItem = gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            PlayerController controller = other.gameObject.GetComponent<PlayerController>();
            controller.nearItem = null;
        }
    }
    */
}