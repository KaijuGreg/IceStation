using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {

    void Interact(Player player);
    Transform GetTransform(); // this is used for determining how close one Interactable is compared to another
    
}
