using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {

    void Interact(Transform interactorTransform);
    Transform GetTransform(); // this is used for determining how close one Interactable is compared to another
    
}
