using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchInteractable : MonoBehaviour,IInteractable, IItemObjectParent {

    [SerializeField] private MeshRenderer buttonLightMeshRenderer;
    [SerializeField] private Material buttonOnMaterial;
    [SerializeField] private Material buttonOffMaterial;
    [SerializeField] Transform itemHoldLocation;
    private Item item;

    private bool isSwitchOff;

    private void SetSwitchColourOff() {
        buttonLightMeshRenderer.material = buttonOffMaterial;
    }

    private void SetSwitchColourOn() {
        buttonLightMeshRenderer.material = buttonOnMaterial;
    }

    private void ToggleColour() {
        isSwitchOff = !isSwitchOff;
        if (isSwitchOff) {
            SetSwitchColourOff();
        }else {
            SetSwitchColourOn();
        }

    }

    public void PushButton() {
        ToggleColour();
    }

    public void Interact(Player player) {

        if (item == null) {
            //No item here
            Debug.Log("There is no item here.");
            if (player.HasItem()) {
                player.GetItem().SetItemObjectParent(this);
            }

        }
        else {
            //There is an item here
            Debug.Log("I'm holding " + item.GetComponent<Item>().GetItemSO().itemName);
            item.SetItemObjectParent(player);
        }

        PushButton();
        Debug.Log("Button is PUSHED");

    }

    public Transform GetTransform() { // this has been used for the IInteractable for the location of the interactable items/objects.
        return transform;
    }

    public Transform GetItemHoldLocation() {
        return itemHoldLocation;
    }

    public void SetItem(Item item) {
        this.item = item;
    }

    public Item GetItem() {
        return item;
    }

    public void ClearItem() {
        item = null;    
    }

    public bool HasItem() {
        return item != null;
    }
}
