using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteractable : MonoBehaviour,IInteractable {

    [SerializeField] private MeshRenderer buttonLightMeshRenderer;
    [SerializeField] private Material buttonOnMaterial;
    [SerializeField] private Material buttonOffMaterial;

    private bool isSwitchOff;

    private void SetSwitchColourOff() {
        buttonLightMeshRenderer.material = buttonOffMaterial;
    }

    private void SetSwitchColourOn() {
        buttonLightMeshRenderer.material = buttonOnMaterial;
    }

    private void ToggleColour() {
        isSwitchOff = !isSwitchOff;
        if (isSwitchOff) {
            SetSwitchColourOff();
        }else {
            SetSwitchColourOn();
        }

    }

    public void PushButton() {
        ToggleColour();
    }

    public void Interact(Transform interactorTransform) {
        PushButton();
        Debug.Log("Button is PUSHED");

    }

    public Transform GetTransform() {
        return transform;
    }
}
