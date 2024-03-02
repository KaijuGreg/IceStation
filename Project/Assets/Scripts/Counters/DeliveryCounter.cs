using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter {

    public static DeliveryCounter Instance { get; private set; }

    private void Awake() {
        
        Instance = this;
    }

    public override void Interact(PlayerController player) {

        if (player.HasKitchenObject()) {
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) { // the delivery counter is only going to accept plates as game design rule, so let's check
                // only accepts plates..

                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject); // once we get the plate, test it against the Delivery Manager logic...

                player.GetKitchenObject().DestroySelf(); //if it is a plate let's destroy it...
            }
        }
    }


}
     