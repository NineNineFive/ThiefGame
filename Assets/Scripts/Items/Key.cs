using UnityEngine;

public class Key : Item {
    public Key(string name, Sprite sprite, float worth) {
        this.name = name;
        this.sprite = sprite;
        this.worth = worth;
        isUsable = false;
    }

    public override void use() {
        // do nothing, as keys usage is by walking into the door
    }
}