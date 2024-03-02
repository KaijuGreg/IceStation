using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Structure : MonoBehaviour, IItemObjectParent {

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

    

}
