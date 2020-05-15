using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GuardBehaviour : MonoBehaviour
{
    public void LookAt(Vector3 p)
    {
        Vector2 pos = p;
        Vector2 direction = (pos - (Vector2) transform.position).normalized;
        transform.up = direction;
    }

    public void Move(float horizontal, float vertical)
    {
        transform.Translate(new Vector3(horizontal * Time.deltaTime, vertical * Time.deltaTime, 0), Space.World);
        //camera.transform.position = new Vector3(transform.position.x,transform.position.y,-100);
    }

    public void Move2(Vector3 position, Vector3 target, float speed)
    {
        transform.position = Vector3.MoveTowards(position, target, Time.deltaTime * speed);
    }
    
}