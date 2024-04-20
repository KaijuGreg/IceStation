using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightArrayVisual : MonoBehaviour {

    [SerializeField] LightSwitchInteractable lightSwitch;

    private Animator animator;

    private void Awake() {

        animator = GetComponent<Animator>();
    }

    private void Start() {

        lightSwitch.LightSwitchedOn += LightSwitch_LightSwitchedOn;
        lightSwitch.LightSwitchedOff += LightSwitch_LightSwitchedOff;
    
    }


    private void LightSwitch_LightSwitchedOn(object sender, System.EventArgs e) {
        TurnLightOn();
    }

    private void LightSwitch_LightSwitchedOff(object sender, System.EventArgs e) {
        TurnLightOff();
    }

   //------------------------------------------------------------------------------------------------

    private void TurnLightOn() {

        animator.SetTrigger("LightTurnOn");
    }

    private void TurnLightOff() {
        animator.SetTrigger("LightTurnOff");
    }

  




}
