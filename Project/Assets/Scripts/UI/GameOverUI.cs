using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI recipesDeliveredText;

    private void Start() {
        
        GameManagerKitchen.Instance.OnStateChanged += GameManagerKitchen_OnStateChanged;
        Hide();
    }

    private void GameManagerKitchen_OnStateChanged(object sender, System.EventArgs e) {

        if (GameManagerKitchen.Instance.IsGameOver()) {
            Show();

            recipesDeliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipesAmount().ToString();
        }
        else {
            Hide();
        }

    }


    private void Show() {
        gameObject.SetActive(true);

    }

    private void Hide() {
        gameObject?.SetActive(false);
    }


}
