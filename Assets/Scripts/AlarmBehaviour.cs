using System;
using System.Collections;
using System.Text;
using UnityEngine;

public class AlarmBehaviour : MonoBehaviour {
    public Alarm alarm;
    public float worth;
    private Sprite sprite;
    
    public void Awake() {
        sprite = GetComponent<SpriteRenderer>().sprite;
        alarm = new Alarm(transform.position,transform.localScale,"Alarm", sprite, worth);
        //Debug.Log(alarm.isActivated);
    }
    
    /*
    if (Input.GetKeyDown("space") && nearItem != null)
    {
        //if (nearItem.name == "Alarm")
        //{
        inventory.addItem(nearItem.GetComponent<AlarmBehaviour>().alarm);
        inventory.showInventory();
        UserInterface.instance.updateInventory();
        nearItem.SetActive(false);
        //}
    }*/
    public void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            UserInterface.instance.playerMessage.text = "Press Spacebar to pick up";
            if (Input.GetKey(KeyCode.Space)) {
                PlayerController controller = other.gameObject.GetComponent<PlayerController>();
                controller.inventory.addItem(alarm);
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
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            PlayerController controller = other.gameObject.GetComponent<PlayerController>();
            controller.inventory.addItem(GetComponent<AlarmBehaviour>().alarm);
            //controller.nearItem = null;
        }
    }
    */
}