using UnityEngine;

public abstract class Item {
    public Vector3 position;
    public Vector3 scale;
    public string name;
    public float worth;
    public Sprite sprite;
    public bool isUsable;
    public bool isActivated;

    public abstract void use();
}