using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchVisual : MonoBehaviour {
    
    [SerializeField] private SwitchInteractable switchInteractable;

    [SerializeField] private MeshRenderer buttonLightMeshRenderer;
    [SerializeField] private Material buttonOnMaterial;
    [SerializeField] private Material buttonOffMaterial;

    private bool isSwitchOff;


    private void Start() {
        switchInteractable.DoorAccessGranted += SwitchInteractable_DoorAccessGranted;
        switchInteractable.DoorAccessDenied += SwitchInteractable_DoorAccessDenied;
    }

    private void SwitchInteractable_DoorAccessDenied(object sender, System.EventArgs e) {
        SetSwitchColourOff();
    }

    private void SwitchInteractable_DoorAccessGranted(object sender, System.EventArgs e) {
        SetSwitchColourOn();
    }

    private void SetSwitchColourOff() {
        buttonLightMeshRenderer.material = buttonOffMaterial;
    }

    private void SetSwitchColourOn() {
        buttonLightMeshRenderer.material = buttonOnMaterial;
    }
    

    //This is not being used...
    private void ToggleColour() {
        isSwitchOff = !isSwitchOff;
        if (isSwitchOff) {
            SetSwitchColourOn();
        }
        else {
            SetSwitchColourOff();
        }

    }
    



}
