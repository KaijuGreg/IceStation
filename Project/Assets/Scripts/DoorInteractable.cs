using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable {

    private Animator animator;
    private bool isOpen;
    
    private void Awake() {
        
        animator = GetComponent<Animator>();
    }


    public void ToggleDoor() {
        isOpen = !isOpen;
        Debug.Log("Door is OPEN!");
        //animator.SetBool("IsOpen", isOpen);

    }

    public void Interact(Player player) {
       ToggleDoor();    
    }

    public Transform GetTransform() {
        return transform;
    }
}
