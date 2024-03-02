using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour, IItemObjectParent {

    [SerializeField] Transform itemHoldLocation;
    private Item item;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            IInteractable interactable = GetInteractableObject();
            if (interactable != null) { 
                    interactable.Interact(transform);
            }
        }    
    }

    // Find the closest Interactable...
    public IInteractable GetInteractableObject() {
        
        // 1. Find interactables on put them in a list...

        List<IInteractable> interactableList = new List<IInteractable>();
        float interactRange = 2f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray) {
            if (collider.TryGetComponent(out IInteractable interactable)) {
                interactableList.Add(interactable);
            }
        }

        // 2. Find the closest interactable...

        IInteractable closestInteractable = null; // declare a variable for the closest IInteractable
        foreach (IInteractable interactable in interactableList) {
            if (closestInteractable == null) {  // if this is empty make the first Interactable the closest...
                closestInteractable = interactable;
            } 
            else { //find out which interactable is closer to the player...
                 if (Vector3.Distance(transform.position, interactable.GetTransform().position)< 
                    Vector3.Distance(transform.position, closestInteractable.GetTransform().position)) {
                    //closer...
                    closestInteractable = interactable;
                 }

            }
        }

        return closestInteractable;
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
