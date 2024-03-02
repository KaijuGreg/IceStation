using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;



public class PlayerController : MonoBehaviour, IKitchenObjectParent {
    
    public static PlayerController Instance { get; private set; } // the 'static' keyword means that this 'PlayerController' 'field'or 'member variable' belongs the class itself and not any instance of the PlayerController



    public event EventHandler OnPickedSomething; // this is to fire off the sounds for picking up something
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged; // this event let's all subscribers know that we have changed counters selected
    public class OnSelectedCounterChangedEventArgs: EventArgs { // here we are extending the c# EventHandler event with EventArgs so we can pass in more data from the standard event
                                                                   // then we put pass it through the original EventHandler 'OnSelectedCounterChanged' above

        public BaseCounter selectedCounter;

    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;


    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

     
    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one PlayerController instance.");
        }
        Instance = this; // this assigns 'this' PlayerController Class to the only Instance of itself
    }


    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction; // Here we INITIALISE THE SUBSCRIBER/ EVENT HANDLER
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e) {
        
        if(!GameManagerKitchen.Instance.IsGamePlaying()) return; // this means you won't be able to interact with anything whilst not in the GamePlaying state

        if (selectedCounter != null) {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e) { //Here is the listener
        
        if (!GameManagerKitchen.Instance.IsGamePlaying()) return;

        if (selectedCounter != null) {
            selectedCounter.Interact(this);
        }
               
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();   

    }


    public bool IsWalking() // this is a method of return type bool, and returns the isWalking bool.
    {
        return isWalking;
    }


    private void HandleInteractions() {

        Vector2 inputVector = gameInput.GetMovementVectorNormalized(); // this is refactored to just get the Vector from another class GameInput

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        //below is how we retain the last Vector3 obtained, so that the raycast for the Interaction isn't lost when we are not moving
        if (moveDir != Vector3.zero) // this will execute when some direction or movement, TRUE when moveDir IS NOT zero. FALSE when moveDir is zero.
        {
            lastInteractDir = moveDir; // we use the lastInteractDir below in the raycast, for when the character stops we still have a Vector3 input
                                        // this stores the moveDir into lastInteractDir, so that when player is not moving we can still raycast in the direction of the last moveDir.
        }
        
        
        float interactDistance = 2f;

        // STEP 2: TRY to find a Counter in front, or specifically try to find the ClearCounter component
        // this raycast uses LayerMask and only works on objects which are assigned that Layer. We define a LayerMask at the top of the code, that can be assigned in the Inspector

        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask)){
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)){ // TryGetComponent returns a boolean. This works like a raycast 
                //Has ClearCounter component
                
                if (baseCounter != selectedCounter){ //we check if it is different Counter from the current selected
                    SetSelectedCounter(baseCounter); //this is a refactored method below
                

                }


               // clearCounter.Interact();
            } else  {
                SetSelectedCounter(null); // this sets to null/nothing IF it doesn't find ClearCounter class. That is, there is something but not a Counter.

               
            }
            
        } else {
            
            SetSelectedCounter (null); // this sets it to null/nothing IF there is nothing in front of the player when selected 

          

        }

      
      
    
    }

    private void HandleMovement() 
    {

        // STEP 1: Get the Input
        Vector2 inputVector = gameInput.GetMovementVectorNormalized(); // this is refactored to just get the Vector from another class GameInput

        //STEP 2: MOVE the object
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y); //when 'casting' , it casts x and y directly so the player character use the X,Y plane - which is vertical and wrong.
                                                                         //so we make our own Vector3 (moveDir) so that it puts the it on the X,Z plane - so that it moves horizontal which is correct.
                                                                         //We 'cast' inputVector which is a Vector2, so that it can be added to the transform which is Vector3

        //STEP 3: TEST to see if anything is in front of Player character

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f; // this used for the size of the Raycast projected out, from the transform.position, in the direction of moveDir below...
        float playerHeight = 2f;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance); // the left side returns a boolean. True if HITs object. Therefore we 'canMove' if it is False.

        
        if (!canMove)
        {
            //This is to solve the problem when the player moves diagonally and hits a wall.
            //Cannot move towards moveDir
            //Attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = (moveDir.x < -.5f || moveDir.x > .5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance); // the moveDir.x !=0, is so that we can move and face the counter when approached from the side

            if (canMove)
            {
                // Can move only on the X
                moveDir = moveDirX;
                
            }
            else
            {
                //Cannot move on the X
                //Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -.5f || moveDir.z > .5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);


                if (canMove)
                {
                    //Can move only on the Z
                    moveDir = moveDirZ;
                }
                else
                {
                    //Cannot move in any direction
                }

            }



        }
      

        if (canMove)
        {
            transform.position += moveDir * moveDistance;   // when doing a move on the Update you don't want it to be move depending on framerate, but base on time - hence Time.deltaTime variable
                                                             // transform.position += (Vector)inputVector

        }                                                               

        isWalking = (moveDir != Vector3.zero);    //!= or == is Equality operator and compares two things of the same type ( this case Vector3 ) and returns a boolean parameter

        float rotateSpeed = 7f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed); // this is so that the character faces forward and rotates smoothly [video timestamp: 1:20:41 ]
                                                                                                     // this means that the character will move towards the target direction moveDir, over Time

    }



    private void SetSelectedCounter (BaseCounter selectedCounter) {

        this.selectedCounter = selectedCounter; // then make the Counter you just 'hit' , that is 'clearCounter', make it the selectedCounter

        //Here we fire off the event that the selected Counter has changed
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform() { // this method gets the kitchenObjectHoldPoint transform
        return kitchenObjectHoldPoint;

    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null) {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }

    }

    public KitchenObject GetKitchenObject() {

        return kitchenObject;
    }

    public void ClearKitchenObject() {
        kitchenObject = null;
    }

    public bool HasKitchenObject() { // this is handy method to return the kitchenObject to tell us what the kitchenObject is
        return kitchenObject != null;
    }


}
