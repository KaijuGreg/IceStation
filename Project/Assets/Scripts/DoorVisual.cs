using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorVisual : MonoBehaviour {

    [SerializeField] private SwitchInteractable doorSwitch;

    private Animator animator;

    private void Awake() {
    
        animator = GetComponent<Animator>();

    }

    private void Start() {

        doorSwitch.DoorAccessGranted += DoorSwitch_DoorAccessGranted;
    }

    private void DoorSwitch_DoorAccessGranted(object sender, System.EventArgs e) {
        animator.SetTrigger("DoorOpen");
    }
}
