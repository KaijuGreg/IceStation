using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//NOTE: Important lesson on CLEARING STATICS at timestamp 9h:05


public class GameManagerKitchen : MonoBehaviour {
   
    public static GameManagerKitchen Instance { get; private set; }


    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;


    private enum State {

        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,

    }

    private State state;
    private float waitingToStartTimer = 3f; 
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 50f;
    private bool isGamePaused = false;

    private void Awake() {
        Instance = this;
        state = State.WaitingToStart;
        
    }

    private void Start() {

        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;

    }

    private void GameInput_OnPauseAction(object sender, EventArgs e) {

        TogglePauseGame();
    }

    private void Update() {

        switch (state) {
            case State.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer < 0f) {

                    state = State.CountdownToStart;
                    OnStateChanged?.Invoke (this, EventArgs.Empty);
                }
                break;

            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0f) {

                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f) {

                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.GameOver:
                            
                break;
        }

        Debug.Log(state);
    }



    public bool IsGamePlaying() {
         return state == State.GamePlaying;
    }

    public bool IsCountdownToStartActive() {
        return state == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer() {

        return countdownToStartTimer;
    }

    public bool IsGameOver() {
        return state == State.GameOver;
    }


    public float GetGamePlayingTimerNormalized() {
        return 1 - ( gamePlayingTimer / gamePlayingTimerMax); 
    }

    public void TogglePauseGame() {

        isGamePaused = !isGamePaused; // this flips isGamePaused, from FALSE to TRUE, and ALSO then from TRUE to FALSE
        if (isGamePaused) {
            Time.timeScale = 0f; // this pauses the game as everything has time.deltatime on it
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }else {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
        
        
    }

}
