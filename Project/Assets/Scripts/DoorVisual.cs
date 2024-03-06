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
        doorSwitch.DoorAccessDenied += DoorSwitch_DoorAccessDenied;
    }

   
    private void DoorSwitch_DoorAccessGranted(object sender, System.EventArgs e) {
        animator.SetTrigger("DoorOpen");
    }

    private void DoorSwitch_DoorAccessDenied(object sender, System.EventArgs e) {
        animator.SetTrigger("DoorClose");
    }


}
