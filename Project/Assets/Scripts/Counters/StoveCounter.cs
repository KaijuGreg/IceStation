using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress {


    public event EventHandler<IHasProgress.OnProgressChangedEventsArgs> OnProgressChanged;// this is the event to handle the FX for hob On, and particles

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs {
        public State state;
    }

    public enum State {
        Idle,
        Frying,
        Fried,
        Burned,

    }
                                               
                                               
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
    

    private State state;
    private float fryingTimer;
    private FryingRecipeSO fryingRecipeSO;
    private float burningTimer;
    private BurningRecipeSO burningRecipeSO;


    private void Start() {
            state = State.Idle;
    }


    private void Update() {

        if (HasKitchenObject()) {
            switch (state) {
                case State.Idle:
                    break;

                case State.Frying:

                    fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventsArgs {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax

                    });


                    if (fryingTimer > fryingRecipeSO.fryingTimerMax) {
                        //Fried
                                           
                        GetKitchenObject().DestroySelf(); // get rid of raw version
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this); // this replaces with cooked version

                        //Debug.Log("Object fried!");
                        
                        state = State.Fried;
                        burningTimer = 0f;

                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                            state = state
                        }) ;
                    }

                    break;

                case State.Fried:
                    burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventsArgs {
                        progressNormalized = burningTimer / burningRecipeSO.burningTimerMax

                    });

                    if (burningTimer > burningRecipeSO.burningTimerMax) {
                        //Fried

                        GetKitchenObject().DestroySelf(); // get rid of raw version
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this); // this replaces with cooked version

                        //Debug.Log("Object burned!");
                        state = State.Burned;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                            state = state
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventsArgs {
                            progressNormalized = 0f // this is to turn off the progress bar

                        });

                    }
                    break;

                case State.Burned:
                    break;

            }
            

        }

      
    }


    public override void Interact(PlayerController player) {

        if (!HasKitchenObject()) {
            //There IS NO KitchenObject here on counter
            if (player.HasKitchenObject()) { //if not, then let's check the player itself for kitchenObject
                //Player is carrying something
                //Check if it has an input, if so it can be placed by the player onto the cuttingCounter
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    //Here the player is carrying something that can be Fried.
                    player.GetKitchenObject().SetKitchenObjectParent(this); // this sets this ClearCounter as parent and puts it on the counter
                    
                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO()); //We get the FryingRecipeSO, when we drop something on the stove.

                    state = State.Frying; // this puts the state machine to Frying
                    fryingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { //this is for the StoveCounterVisual to turn on the FX
                        state = state
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventsArgs {
                        progressNormalized = fryingTimer/ fryingRecipeSO.fryingTimerMax
                    
                    });
                }
            }
            else {
                //Player not carrying anything. Therefore, can't place anything
            }

        }
        else {
            //There IS a KitchenObject here on this Counter
            if (player.HasKitchenObject()) {
                //Player is carrying something. Therefore, don't give them anything, we don't want to give the player two items
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) { // so we try to get the plate...

                    //Player is holding a Plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {   // try to add IngredientSO the object to the plate
                        GetKitchenObject().DestroySelf(); // then destroy it from the counter

                        state = State.Idle; // this returns the stove to idle state once something has been taken off. And this resets the logic of the state machine.

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { //this is for the StoveCounterVisual to turn off the FX
                            state = state
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventsArgs {
                            progressNormalized = 0f // this is to turn off the progress bar, when the player takes kitchenObject off the stove

                        });

                    }
                }

            }
            else {
                //Player is not carrying anything, so give the player the kitchenObject
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle; // this returns the stove to idle state once something has been taken off. And this resets the logic of the state machine.

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { //this is for the StoveCounterVisual to turn off the FX
                    state = state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventsArgs {
                    progressNormalized = 0f // this is to turn off the progress bar, when the player takes kitchenObject off the stove

                });

            }
        }


    }

    //this validates whether the kitchen object has a frying recipe, with the fryingRecipeSOArray, returns a bool TRUE or FALSE
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) { 
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    //below, gives the cooked version, ie the 'output' property of the fryingRecipeSO, ie the output KitchenObjectSO
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {// take this inputKitchenObjectSO and find a match with in the RecipeSO...
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null) {
            return fryingRecipeSO.output; // Here it gives the fried version of the recipe, ie the output KitchenObjectSO
        }
        else {
            return null;
        }

    }

    //This method returns only the FryingRecipeSO, based on the KitchenObjectSO...
    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) { // recieve the KitchenObjectSO for input...
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray) { //go through the recipes on this counter, find a match, return a cuttingRecipeSO
            if (fryingRecipeSO.input == inputKitchenObjectSO) {
                return fryingRecipeSO;
            }
        }
        return null;
    }


    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) { // recieve the KitchenObjectSO for input...
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray) { //go through the recipes on this counter, find a match, return a cuttingRecipeSO
            if (burningRecipeSO.input == inputKitchenObjectSO) {
                return burningRecipeSO;
            }
        }
        return null;
    }


}
