using UnityEngine;

public class Valueable : Item {
    public Valueable(string name, Sprite sprite, float worth) {
        this.name = name;
        this.sprite = sprite;
        this.worth = worth;
        isUsable = false;
    }

    public override void use() {
        // do nothing
    }
}