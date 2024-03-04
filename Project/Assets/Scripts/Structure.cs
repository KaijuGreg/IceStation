using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Structure : MonoBehaviour, IItemObjectParent, IInteractable {

    [SerializeField] private Transform itemHoldLocation;
    [SerializeField] private ItemSO itemSO;
      
    private Item item;

     
        

    private ItemSO GetItemSO() {
        return itemSO;
    }


    public Transform GetItemHoldLocation() {
        return itemHoldLocation;
    }

    public void SetItem(Item item) { // this allows the item to tell this structure, this is now the parent of this item.
        this.item = item;
    }

    public Item GetItem() {
        return item;
    }

    public void ClearItem() {
        item = null;
    }

    public bool HasItem() {
        return item != null; //this says if it is NOT null, return the item.
    }

    public void Interact(Player player) {
        if (item == null) {
            //No item here
            Debug.Log("There is no item here.");
            if (player.HasItem()) {
                player.GetItem().SetItemObjectParent(this);
            }

        }else {
            //There is an item here
            Debug.Log("I'm holding " + item.GetComponent<Item>().GetItemSO().itemName);
            item.SetItemObjectParent(player);
        }

        

    }

    public Transform GetTransform() {
        return transform;
    }
}
