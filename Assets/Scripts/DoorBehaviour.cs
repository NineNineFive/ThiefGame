using UnityEngine;

public class DoorBehaviour : MonoBehaviour {
    public string password;
    
    public void OnCollisionEnter2D(Collision2D other) {
        
        if (other.gameObject.CompareTag("Player")) {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            if (playerController.inventory.checkForItem(password)) {
                // Testing remove key:
                foreach (Item item in playerController.inventory.items) {
                    if(item != null){
                        if(item.name==password){
                            playerController.inventory.removeItem(item);
                        }
                    }
                }
                UserInterface.instance.updateInventory();
                
                AudioManager.instance.Play("lock");
                // ------
                gameObject.SetActive(false);
            }
            else
            {
                UserInterface.instance.playerMessage.text = "You need the " + password;
            }
        }
    }
    
    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            UserInterface.instance.playerMessage.text = "";
        }
    }
}