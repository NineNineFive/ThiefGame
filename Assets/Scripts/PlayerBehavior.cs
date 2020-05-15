
using System;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour {
    private FieldOfView fieldOfView;
    public Camera camera;
    
    public void LookAtMouse(Vector3 mousePosition) { 
        Vector2 pos = camera.ScreenToWorldPoint(mousePosition);
        Vector2 direction = (pos - (Vector2) transform.position).normalized;
        transform.up = direction;
    }

    public void Move(float horizontal, float vertical) {
        transform.Translate(new Vector3(horizontal * Time.deltaTime, vertical * Time.deltaTime, 0),Space.World);
        camera.transform.position = new Vector3(transform.position.x,transform.position.y,-100);
    }
}
