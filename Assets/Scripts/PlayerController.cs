using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    // TODO: Make Private and make getters  to prevent setting values
    public float vertical = 0;
    public float horizontal = 0;
    public Vector3 mousePosition;
    public static Inventory playerInventory = new Inventory(10);
    public PlayerBehavior playerBehavior;

    // Update is called once per frame
    void Update(){
        // TODO: CLAMP VALUES SO THEY HAVE LIMITS
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        mousePosition = Input.mousePosition;
    }
    
    void FixedUpdate() {
        playerBehavior.Move(horizontal, vertical);
        playerBehavior.LookAtMouse(mousePosition);
    }
}
