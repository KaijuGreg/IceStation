using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Monetization;

public class GameStartCountdownUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI countdownText;


    private void Start() {
        GameManagerKitchen.Instance.OnStateChanged += GameManagerKitchen_OnStateChanged;
        Hide();
    }

    private void GameManagerKitchen_OnStateChanged(object sender, System.EventArgs e) {
     
        if (GameManagerKitchen.Instance.IsCountdownToStartActive()) {
            Show();
        }else {
            Hide();
        }

    }

    private void Update() {
        countdownText.text = Mathf.Ceil(GameManagerKitchen.Instance.GetCountdownToStartTimer()).ToString();
    }

    private void Show() {
        gameObject.SetActive(true);
    
    }

    private void Hide() {
        gameObject?.SetActive(false);
    }



}
