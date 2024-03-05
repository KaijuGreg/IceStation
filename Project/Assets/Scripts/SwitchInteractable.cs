using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SwitchInteractable : MonoBehaviour,IInteractable,IItemObjectParent {

    [SerializeField] private MeshRenderer buttonLightMeshRenderer;
    [SerializeField] private Material buttonOnMaterial;
    [SerializeField] private Material buttonOffMaterial;
    [SerializeField] Transform itemHoldLocation;
    private Item item;

    private bool isSwitchOff;

    public event EventHandler DoorAccessGranted;

    public void Interact(Player player) {

        if (!player.HasItem()) { // check to see if player is holding item. Player not holding item.

            GetItem().SetItemObjectParent(player); // this gives the item to the player

        }
        else {  // player is holding item
            if (item == null) { //There is an NO item here on structure
                if (player.HasItem()) {
                    if (player.GetItem() is ItemAccessKey) { // this tests for ACCESS KEY here!
                        player.GetItem().SetItemObjectParent(this); // this places the item on the structure
                        DoorAccessGranted?.Invoke(this, EventArgs.Empty);   
                    }else {
                        Debug.Log("Wrong key");
                    }
                    
                }

            }

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
        }
        else {
            SetSwitchColourOn();
        }

    }

    public void PushButton() {
        ToggleColour();
    }

}
