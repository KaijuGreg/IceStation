using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateDirection : MonoBehaviour
{
    [SerializeField]
    private Transform _object;  //name of some random object we want to measure distance from 


    private void Update() {
        // direction = destination - source
        Vector3 direction = _object.position - transform.position;
        direction.Normalize();
        Debug.Log ("Magnitude: " + direction.magnitude);
        Debug.DrawRay(transform.position, direction, Color.green);


    }

}
