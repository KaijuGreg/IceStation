using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter {

    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    


    public override void Interact(PlayerController player) {

        if (!HasKitchenObject()) {
            //There IS NO KitchenObject here on counter
            if (player.HasKitchenObject()) { //if not, then let's check the player itself for kitchenObject
                //Player is carrying something
                player.GetKitchenObject().SetKitchenObjectParent(this); // this sets this ClearCounter as parent and puts it on the counter
            } else {
                //Player not carrying anything. Therefore, can't place anything
            }

        } else {
            //There IS a KitchenObject here on this Counter
            if (player.HasKitchenObject()) {
                //Player is carrying something. So it is here player could be carrying a Plate. So we test for that...
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) { // so we try to get the plate...
                    
                    //Player is holding a Plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {   // try to add IngredientSO the object to the plate
                        GetKitchenObject().DestroySelf(); // then destroy it from the counter
                    }
                }else { // here we want to add an ingredient to a plate
                    //Player is not carrying a Plate, BUT is carrying something else (eg sliced tomatoes)
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject)) { // checking the Counter for kitchenObject
                        //Counter is holding a plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) {
                            player.GetKitchenObject().DestroySelf(); //remove from the player add it
                        }
                    }
                }


            }else {
                //Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
       
    }
    
    


}
