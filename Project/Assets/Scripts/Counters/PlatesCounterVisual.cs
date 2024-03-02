using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour {


    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    private List<GameObject> plateVisualGameObjectList;


    private void Awake() {
        
        plateVisualGameObjectList =  new List<GameObject>();

    }

    private void Start() {

        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void PlatesCounter_OnPlateRemoved(object sender, System.EventArgs e) {
        
        GameObject plateGameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];
        plateVisualGameObjectList.Remove(plateGameObject); // remove the plate that we just grabbed above from the list
        Destroy(plateGameObject);// and destroy the visual
    }

    private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e) {
       Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint ); // spawn a plate...
       float plateOffsetY = .15f;
       plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualGameObjectList.Count, 0); // position it in the Y, based on the Count

       plateVisualGameObjectList.Add(plateVisualTransform.gameObject);

    }


    

}
