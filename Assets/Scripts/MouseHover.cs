using UnityEngine;
using UnityEngine.EventSystems;

public class MouseHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public static UserInterface instance;
    public bool empty = false;
    public float cash;
    public string name;
    
    public void OnPointerEnter(PointerEventData eventData) {
        if (empty) {
            UserInterface.instance.toolText.text = "Empty Inventory Slot";
        } else {
            UserInterface.instance.toolText.text = name + "\nWorth: " + cash + "$";
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        UserInterface.instance.toolText.text = "";
    }
}
