using UnityEngine;
using UnityEngine.PlayerLoop;


public class HumanBehaviour : MonoBehaviour {
    public float speed = 1f;
    public void turn(Vector2 pos) {
        Vector2 direction = (pos - (Vector2) transform.position).normalized;
        if (direction!=new Vector2(0.0f, -1.0f)) {
            transform.up = direction;
        }
    }

    public void move(float horizontal, float vertical) {
        transform.Translate(new Vector2(horizontal * Time.deltaTime * speed, vertical * Time.deltaTime * speed),Space.World);
    }

    
    public void AITurn(Quaternion rotation) {
        transform.rotation = rotation;
    }
    public void AIMove(Vector2 position, Vector2 target) {
        transform.position = Vector2.MoveTowards(position, target, Time.deltaTime * speed);
    }
}
