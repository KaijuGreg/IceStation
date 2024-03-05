using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {


    [SerializeField] private ItemSO itemSO;

    private IItemObjectParent itemObjectParent; // this is so the item knows which structure/IItemObjectParent it belongs to 

   
    public ItemSO GetItemSO() {
        return itemSO;
    }

    public Transform GetTransform() {
       return transform;
    }


    /*
   public void Interact(Player player) {

       SetItemObjectParent(player.gameObject.GetComponent<IItemObjectParent>());

       Debug.Log("Interacting with the: " + itemSO.itemName);

   }
  */

    //this allows the item to manage and tell the parent structure where the item belongs to...
    public void SetItemObjectParent(IItemObjectParent itemObjectParent) { // here we tell the item, the structure passed through, is now structure it is attached to

        
        //1. First we clear/unlink item from the existing parent
        if (this.itemObjectParent != null) {
            this.itemObjectParent.ClearItem();
        }
        //2. here the new parent is assigned to the item
        this.itemObjectParent = itemObjectParent;

        if (itemObjectParent.HasItem()) { //this is just a check to see if the structure already has an item
            Debug.Log("IItemObjectParent already has an item!");
        }

        //3. we tell the new structure/parent it now has this item
        itemObjectParent.SetItem(this);

        //4. we visually move the Item to the Location Point
        transform.parent = itemObjectParent.GetItemHoldLocation(); // this assigns the items to it's new location so that it actually visually moves
        
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public IItemObjectParent GetItemObjectParent() {
        return itemObjectParent;
    }

}
