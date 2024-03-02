using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{

    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;
    
    private void Start() {
        PlayerController.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged; // here we subscribe the 'Player(originally named 'Instance' here)_OnSelectedCounterChanged' method
                                                                                                 // to listen to the event in PlayerController.Instance.OnSelectedCounterChanged

    }

    private void Player_OnSelectedCounterChanged(object sender, PlayerController.OnSelectedCounterChangedEventArgs e) {
        if (e.selectedCounter == baseCounter) { // this is saying here, if the PlayerController selectedCounter is the same as this SelectedCounterVisual's ClearCounter type turn it on/off
            Show();
        }
        else {
            Hide();
        }
    }


    private void Show() {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        visualGameObject.SetActive(true);
    }

    private void Hide() {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        visualGameObject?.SetActive(false);
    }

}
