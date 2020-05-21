using UnityEngine;

public class ValueableBehaviour : MonoBehaviour {
    public string name;
    public Valueable valueable;
    private Sprite sprite;
    public float worth;
    public void Start() {
        sprite = GetComponent<SpriteRenderer>().sprite;
        valueable = new Valueable(name, sprite, worth);
        
    }

    public void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            UserInterface.instance.playerMessage.text = "Press Spacebar to pick up";
            if(Input.GetKey(KeyCode.Space)) {
                PlayerController controller = other.gameObject.GetComponent<PlayerController>();
                controller.inventory.addItem(valueable);
                UserInterface.instance.updateInventory();
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