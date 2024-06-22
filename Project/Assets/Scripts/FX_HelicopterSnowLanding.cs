using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_HelicopterSnowLanding : MonoBehaviour {
    ParticleSystem fxHelicopterSnowLanding;

    // Start is called before the first frame update
    void Start()    {
        fxHelicopterSnowLanding = GetComponent<ParticleSystem>();
    }

    private void Update() {

      
    }


    private void OnTriggerEnter(Collider other) {

        if (other.tag == "helicopter") {

                        fxHelicopterSnowLanding.Play();
        }
        
    }


    private void OnTriggerExit(Collider other) {

        if (other.tag == "helicopter") {

            fxHelicopterSnowLanding.Stop();
        }
        
    }


}
