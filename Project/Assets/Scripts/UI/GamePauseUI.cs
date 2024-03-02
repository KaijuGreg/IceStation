using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour {


    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionsButton;

    private void Awake() {

        resumeButton.onClick.AddListener(() => {
            GameManagerKitchen.Instance.TogglePauseGame();

        });

        mainMenuButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        optionsButton.onClick.AddListener(() => {
            Hide(); // this hides the PAUSE when the options is clicked and opened...
            // below tells us when the option closes so that we know when to show the OPTIONS again
            OptionsUI.Instance.Show(Show);
            // this simultaneously turns on the OPTIONSUI, whilst assigning the GamePauseUI's Show() method into the OptionsUI 'Action OnCloseButtonAction'
        }); 

    
    }



    private void Start() {
       
        GameManagerKitchen.Instance.OnGamePaused += GameManagerKitchen_OnGamePaused;
        GameManagerKitchen.Instance.OnGameUnpaused += GameManagerKitchen_OnGameUnpaused;
        
        Hide();
    }

    private void GameManagerKitchen_OnGameUnpaused(object sender, System.EventArgs e) {
        Hide();
        OptionsUI.Instance.Hide();
    }

    private void GameManagerKitchen_OnGamePaused(object sender, System.EventArgs e) {
        Show();
        //MusicManager.Instance.SetPauseVolume();
    }

    private void Show() {
        gameObject.SetActive(true);

        resumeButton.Select(); //this will make the RESUME button selected, so that it is visible on first load
    }


    private void Hide() {
        gameObject.SetActive(false);
    }

}
