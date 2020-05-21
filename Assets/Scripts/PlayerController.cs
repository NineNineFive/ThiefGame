using System;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    // TODO: Make Private and make getters  to prevent setting values
    public float vertical = 0;
    public float horizontal = 0;
    public Vector3 mousePosition;
    public Inventory inventory = new Inventory(24);
    public HumanBehaviour humanBehaviour;
    public Camera camera;

    private void Start() {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update(){
        // TODO: CLAMP VALUES SO THEY HAVE LIMITS
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        mousePosition = Input.mousePosition;
        if (camera != null) {
            camera.transform.position = new Vector3(transform.position.x,transform.position.y,-100);
        }

    }
    
    void FixedUpdate() {
        Vector3 direction = new Vector3(0,0,0);
        if(camera!=null){
            direction = camera.ScreenToWorldPoint(mousePosition);
        }
        humanBehaviour.turn(direction);
        humanBehaviour.move(horizontal, vertical);
    }
}
