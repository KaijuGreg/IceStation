using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {


    private const string PLAYER_PREFS_BINDINGS = "InputBindings";
    public static GameInput Instance { get; private set; }
    
    public event EventHandler OnInteractAction; // this declares the Interaction event, it is raised/fired/triggered in the 'Interact_performed' Listener/Method/Subscriber/EventHandler
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;

    public enum Binding {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlternate,
        Pause,
        Gamepad_Interact,
        Gamepad_InteractAlternate,
        Gamepad_Pause
    }


    private PlayerInputAction playerInputAction;

    

    private void Awake()  {
        Instance = this;
        
        playerInputAction = new PlayerInputAction(); // this is the Unity Player Input system. 
        
        
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS)) {  // this is IMPORTANT we do this before we enable BELOW
            playerInputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }
        

        playerInputAction.Player.Enable();              //'Player' is what we named in the Player Input System // this line activates it


        playerInputAction.Player.Interact.performed += Interact_performed;
        // this is the event that Interact has happened.
        //the ".performed" is an event associated with the Interact action. It TRIGGERS/ broadcasts when the key is pressed.
        // the += adds a subscriber, in this case the Interact_performed function below. The 'Interact_performed' method will be triggered.

        playerInputAction.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputAction.Player.Pause.performed += Pause_performed;
        
       

    }

    private void OnDestroy() {
        // this is required for Scene Loading and Clean Up, you need to unsubscribe
        playerInputAction.Player.Interact.performed -= Interact_performed; 
        playerInputAction.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputAction.Player.Pause.performed -= Pause_performed;

        playerInputAction.Dispose();
    }



    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    // this below is the Listener to the Event
    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        // if (OnInteractAction != null)  // this null check, identifies if there are actually subscribers so that it can fire the event
        // { 
        // OnInteractAction(this, EventArgs.Empty);
        // }

        //OR... we can write the above as EXACTLY the same above with the "?" ( this is a called the NULL CONDITIONAL OPERATOR )
        //

        OnInteractAction?.Invoke(this, EventArgs.Empty); // this fires the OnInteractAction, telling listeners the E key(in this case) has been pressed
    }

    public Vector2 GetMovementVectorNormalized() 
    {
        Vector2 inputVector = playerInputAction.Player.Move.ReadValue<Vector2>();

        
        inputVector = inputVector.normalized;

        //this makes sure the magnitude of the vector when you press X and Y at the same time it is still just 1
        //this makes the player move at the same speed on diagonals as well as on straights

       // Debug.Log(inputVector);
        return inputVector;
    }


    public string GetBindingText (Binding binding) {

        switch (binding){
            default:
            case Binding.Move_Up:
                return playerInputAction.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInputAction.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInputAction.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputAction.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return playerInputAction.Player.Interact.bindings[0].ToDisplayString(); // all of the keyboard bindings are defined on index 0, this comes from the order in the PlayerInputActions Panel
            case Binding.InteractAlternate:
                return playerInputAction.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputAction.Player.Pause.bindings[0].ToDisplayString();
            
            case Binding.Gamepad_Interact:
                return playerInputAction.Player.Interact.bindings[1].ToDisplayString();
            case Binding.Gamepad_InteractAlternate:
                return playerInputAction.Player.InteractAlternate.bindings[1].ToDisplayString();
            case Binding.Gamepad_Pause:
                return playerInputAction.Player.Pause.bindings[1].ToDisplayString();
            

        }


    }
    // timestamp: 9:33:40 in CodeMonkey Video for below
    public void RebindBinding(Binding binding, Action onActionRebound) {

        playerInputAction.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding) {
            default:
            case Binding.Move_Up:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInputAction.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternate:
                inputAction = playerInputAction.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputAction.Player.Pause;
                bindingIndex = 0;
                break;
            
            case Binding.Gamepad_Interact:
                inputAction = playerInputAction.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_InteractAlternate:
                inputAction = playerInputAction.Player.InteractAlternate;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_Pause:
                inputAction = playerInputAction.Player.Pause;
                bindingIndex = 1;
                break;

                
        }


        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback => {
                callback.Dispose();
                playerInputAction.Player.Enable(); 
                onActionRebound();

                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputAction.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
            })
            .Start();
    }


}
