using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent {

    public static event EventHandler OnAnyObjectPlacedHere; //this is an event to fire off the sound for placement on all counters

    public static void ResetStaticData() {
        OnAnyObjectPlacedHere = null; // this resets any clears all the listeners 
    }

    [SerializeField] private Transform counterTopPoint;


    private KitchenObject kitchenObject;


    public virtual void Interact (PlayerController player) {
        Debug.LogError("BaseCounter.Interact();");
    }

    public virtual void InteractAlternate(PlayerController player) {
        //Debug.LogError("BaseCounter.InteractAlternate();");
    }


    public Transform GetKitchenObjectFollowTransform() { // this method gets the counterTopPoint transform. This is to move kitchen object to the parent counterTopPoint.
        return counterTopPoint;

    }


    public void SetKitchenObject(KitchenObject kitchenObject) { // this sets the kitchenObject of this counter. So that the counter knows it is now a parent to the kitchenObject
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null) {
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }

    }

    public KitchenObject GetKitchenObject() { // this returns the kitcheObject of this counter
        return kitchenObject;
    }

    public void ClearKitchenObject() {
        kitchenObject = null;
    }

    public bool HasKitchenObject() { // this is handy method to return the kitchenObject to tell us what the kitchenObject is
        return kitchenObject != null;
    }

}
