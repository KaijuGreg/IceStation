using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable {
  
    public void Interact(Transform interactorTransform) {

        Debug.Log("Interact NPC!");
    }

    public Transform GetTransform() {
        return transform;
    }

}
