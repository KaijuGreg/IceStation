using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter {

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public event EventHandler OnPlayerGrabbedObject;


    public override void Interact(PlayerController player) {

        if (!player.HasKitchenObject()) { // Player is not carrying anything, give them kitchenObject

            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player); //give it to the player
       

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty); // this notifies the ContainerCounterVisual so that it can play the animation for open/close counter
        }
      
    }




}
