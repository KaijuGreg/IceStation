using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LightSwitchInteractable : MonoBehaviour, IInteractable {

    public event EventHandler LightSwitchedOn;
    public event EventHandler LightSwitchedOff;

    private bool isLightOff;

    public void Interact(Player player) {

        ToggleLight();
        Debug.Log("You hit the light switch");
       

    }

    public Transform GetTransform() {
        return transform;
    }


    private void TurnOn() {
        LightSwitchedOn?.Invoke(this, EventArgs.Empty);
    }


    private void TurnOff() {
        LightSwitchedOff?.Invoke(this, EventArgs.Empty);
    }

    private void ToggleLight() {
        isLightOff = !isLightOff;
        if (isLightOff) {
            TurnOn();
        }
        else {
            TurnOff();
        }
    }

}
