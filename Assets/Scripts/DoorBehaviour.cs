using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public string password;
    
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && PlayerController.playerInventory.checkForItem(password))
        {
            gameObject.SetActive(false);
        }
    }
}