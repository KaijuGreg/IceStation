using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour{

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform iconTemplate;



    private void Awake() {
        iconTemplate.gameObject.SetActive(false); // this turns off the icon so that you can't see it yet...
    }

    private void Start() {

        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
        
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e) {
        UpdateVisual();
    }
    
    private void UpdateVisual() {
        
        // this below cleans up the children from the previous plate's ingredients added, so that we don't keep getting more on top added...
        foreach (Transform child in transform) {
            if (child == iconTemplate) continue; // this 'continue' skips the template, so that it can be used below
            Destroy(child.gameObject);
        }

        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) { // go through the ingredients(kitchenObjectSO) on the plate's KitchenObjectSO List
            Transform iconTransform = Instantiate(iconTemplate, transform); // the 'transform here refers to this GameObject. So the iconTemplate will be a child of this PlateIconUI gameObject
            iconTransform.gameObject.SetActive(true);// this turns the icon back on...
            iconTransform.GetComponent<PlateIconSingleUI>().SetKitchenObjectSO(kitchenObjectSO); //this assigns the sprite image of the SO icon, to the icon Image

        }

    }


}
