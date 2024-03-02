using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress {

    //NOTE: Important lesson on CLEARING STATICS at timestamp 9h:05
    // as static event the listeners will still be there
    public static event EventHandler OnAnyCut; // we are going to fire this when any cutting counter fires a trigger to cut

   new public static void ResetStaticData() {
        OnAnyCut = null; // this resets any clears all the listeners 
    }

    public event EventHandler<IHasProgress.OnProgressChangedEventsArgs> OnProgressChanged; // this is to notify the cutting progress bar

    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;

    public override void Interact(PlayerController player) {
        if (!HasKitchenObject()) {
            //There IS NO KitchenObject here on counter
            if (player.HasKitchenObject()) { //if not, then let's check the player itself for kitchenObject
                //Player is carrying something
                //Check if it has an input, if so it can be placed by the player onto the cuttingCounter
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    //Here the player is carrying something that can be Cut
                    player.GetKitchenObject().SetKitchenObjectParent(this); // this sets this ClearCounter as parent and puts it on the counter
                    cuttingProgress = 0; // initialising the cutting progress when something can be Cut.

                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO()); //get the recipe to find progress max counter int

                    //firing off the event for progress bar below...
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventsArgs {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax //int is cast as float so that we don't get ZERO when dividing in c#
                    });
                }
            }
            else {
                //Player not carrying anything. Therefore, can't place anything
            }

        }
        else {
            //There IS a KitchenObject here on this Counter
            if (player.HasKitchenObject()) {

                //Player is carrying something. Therefore, don't give them anything, we don't want to give the player two items
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) { // so we try to get the plate...

                    //Player is holding a Plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {   // try to add IngredientSO the object to the plate
                        GetKitchenObject().DestroySelf(); // then destroy it from the counter
                    }
                }

            }
            else {
                //Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }


    public override void InteractAlternate(PlayerController player) { 
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) {
            //so we CUT if here is kitchenObject here AND it can be cut. timecode 4:45:54 for explanation. (ie so it means Bread is excluded)
            cuttingProgress++;

            OnCut?.Invoke(this, EventArgs.Empty);
            //Debug.Log(OnAnyCut.GetInvocationList().Length); //this was to test for the clearing of the static data
            OnAnyCut?.Invoke(this, EventArgs.Empty);


            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            //firing off the event for progress bar below...
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventsArgs {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax //int is cast as float so that we don't get ZERO when dividing in c#
            });

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax) {

                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO()); //we get the output before destroying the kitchenObject

                GetKitchenObject().DestroySelf(); //this gets rid of the 'whole' kitchen object before we replace with the sliced kitchenObject

                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this); // then we spawn the new 'outputKitchenObjectSO'
            }
        }

    }

    // this checks for the existence of a recipe the player might be carrying has an input, and therefore can be sliced. This is for excluding Bread, which can't be sliced.
    private bool HasRecipeWithInput (KitchenObjectSO inputKitchenObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    //below, gives the sliced version, ie the 'output' property of the cuttingRecipeSO, ie the output KitchenObjectSO
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {// take this inputKitchenObjectSO and find a match with in the RecipeSO...
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSO != null) { 
            return cuttingRecipeSO.output; // Here it gives the SLICED version of the recipe, ie the output KitchenObjectSO
        }else {
            return null;
        }
               
    }

    //This method returns only the CuttingRecipeSO, based on the KitchenObjectSO...
    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) { // recieve the KitchenObjectSO for input...
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) { //go through the recipes on this counter, find a match, return a cuttingRecipeSO
            if (cuttingRecipeSO.input == inputKitchenObjectSO) {
                return cuttingRecipeSO;
            }        
        }
        return null;
    }
    



}
