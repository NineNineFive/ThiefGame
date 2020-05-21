using System;
using System.Linq;
using UnityEngine;

public class Inventory {
    public Item[] items;
    public Transform itemPrefab;
    public Inventory(int space) {
        items = new Item[space];
    }

    public void addItem(Item item) {
        for (int i = 0; i < items.Length; i++) {
            if (items[i] == null) {
                items[i] = item;
                break;
            }
        }
    }

    public void removeItem(Item item) {
        for (int i = 0; i < items.Length; i++) {
            if (items[i] == item) {
                items[i] = null;
                break;
            }
        }
    }
    public bool checkForItem(string name)
    {
        bool searchResult = false;
        foreach (Item item in items) {
            if (item != null) {
                if (item.name == name) {
                    searchResult = true;
                }
            }
        }
        
        return searchResult;
    }
    
    public void showInventory() {
        string itemString = "";
        foreach (Item item in items) {
            if (item == null) {
                itemString += "empty ";
            } else {
                itemString += item.name + " ";
            }
        }
        //Debug.Log(itemString);
    }
}