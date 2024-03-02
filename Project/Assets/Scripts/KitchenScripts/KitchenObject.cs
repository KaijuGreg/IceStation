using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour {

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO() { //this method returns a KitchenObjectSO type 

        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        //this function assigns 'kitchenObjectParent' field of this class
        //A: we clear old kitchenObjectParent...// this method is part of the process of letting Kitchen object NOTIFY the kitchenObjectParent that it belongs to

        if (this.kitchenObjectParent != null) {
            this.kitchenObjectParent.ClearKitchenObject(); // Part1/2 so this clears the current(old) kitchenObjectParent of this KitchenObject from this kitchenObjectParent.
        }

        //B: we add it to the new kitchenObjectParent
        this.kitchenObjectParent = kitchenObjectParent;
      

        // a small check to see if the kitchenObjectParent is clear to add a kitchenObject
        if (kitchenObjectParent.HasKitchenObject()) {
            Debug.LogError("IKitchenObjectParent already has a KitchenObject!!");
        }

        kitchenObjectParent.SetKitchenObject(this); // part2/2 this Setter function then tells the kitchenObjectParent it now has THIS KitchenObject

        //C: we update the 'visual' of kitchen object and move it to the new kitchenObjectParent
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform(); // here 'transform.parent' is setting the parent of the 'KitchenObject''s transform. ie to the kitchenObjectParent's placement node transform
        transform.localPosition = Vector3.zero;

               
         
    }

    public IKitchenObjectParent GetKitchenObjectParent() { //this function returns this 'kitchenObjectParent' field, this is so the kitchenObject knows where it is

        return kitchenObjectParent;
    }

    public void DestroySelf() {
        kitchenObjectParent.ClearKitchenObject() ; // this unparents it from the parent
        Destroy(gameObject); //Destroy itself

    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject) {
        if (this is PlateKitchenObject) {
            plateKitchenObject = this as PlateKitchenObject; //this KitchenObject cast as a plateKitchenObject now...
            return true;
        }else {
            plateKitchenObject=null; // with the 'out' output parameter plateKitchenObject must be set to something.
            return false; 
        }

    }

    // this static function below refactors what was originally on both the cuttingCounter and the containCounter.
    // it makes the kitchen object itself responsible for the spawning 
    public static KitchenObject SpawnKitchenObject (KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent) {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);  // this spawns the sliced tomato, exactly like the ContainerCounter spawns a kitchenObject
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();// we catch it/assign it to this kitchenObject
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent); // 'this' parents the sliced tomato to THIS counter

        return kitchenObject;
    }


}
