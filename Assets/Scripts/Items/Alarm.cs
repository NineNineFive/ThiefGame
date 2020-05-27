using UnityEngine;

public class Alarm : Item {
    public Alarm(Vector3 scale, string name, Sprite sprite, float worth) {
        this.scale = scale;
        this.name = name;
        this.sprite = sprite;
        this.worth = worth;
        isUsable = true;
    }

    public override void use() {
        // Call item placer script
        GameObject obj = new GameObject(name);
        obj.transform.position = Data.instance.player.transform.position;
        obj.transform.localScale = scale;
        SpriteRenderer spriteRenderer = obj.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;

        Data.instance.player.GetComponent<PlayerController>().inventory.removeItem(this);
        UserInterface.instance.updateInventory();
        
        AudioManager.instance.playAlarm(obj);
        
        
        GameObject.Destroy(obj, 9.5f);

        // MAKE COLLIDER IF YOU WANT TO PICK IT UP AGAIN
        //obj.AddComponent<BoxCollider2D>().isTrigger = true;
    }
}