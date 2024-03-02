using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject {

    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs {

        public KitchenObjectSO kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;//  list of ingredients we ALLOW on the plate, thereby excluding things like uncut tomatoes for example.
    
    private List<KitchenObjectSO> kitchenObjectSOList;


    //-----------------------------------------------------------------------------------------------------------------

    private void Awake() {
        
        kitchenObjectSOList = new List<KitchenObjectSO>(); // need to initialise the list to use it...
    }
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO) { // Here we test if ingredient can be added
       
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO)) { // this to test kitchenObjectSO is VALID and can be added to the plate (ie it is not uncut tomatoes)
            //Not a valid ingredient, so DON'T ADD IT...
            return false;
        }
        
        if (kitchenObjectSOList.Contains(kitchenObjectSO)) { // this is to check that there is ONLY ONE type of ingredient. As we are keeping the design simple.
            //Already has this type, so DON'T ADD IT...
            return false;
        } else { // Doesn't have this ingredient so add it...
            kitchenObjectSOList.Add(kitchenObjectSO); // add the SO to the list

            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs {
                kitchenObjectSO = kitchenObjectSO
            }); 

            return true;
        }
                       
    }


    public List<KitchenObjectSO> GetKitchenObjectSOList() {
        return kitchenObjectSOList;

    }



}
