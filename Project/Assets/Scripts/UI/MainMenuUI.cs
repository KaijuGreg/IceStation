using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {

    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake() {

        playButton.onClick.AddListener(() => { // this is a lambda expression, used instead of creating a separate function. so it's a shorthand

            Loader.Load(Loader.Scene.GameScene);

        });

        quitButton.onClick.AddListener(() => { 

            Application.Quit(); 
        
        });


        Time.timeScale = 1f; // this resets the time when you get to the Main Menu from the pause menu so that when you resume the game it plays and is not paused.
    }

}
