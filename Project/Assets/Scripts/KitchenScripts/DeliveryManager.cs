using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour {


    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;


    public static DeliveryManager Instance { get; private set; } 

    [SerializeField] private RecipeListSO recipeListSO; //this is a reference to the master recipeListSO, that contains all the possible customer orders

    private List<RecipeSO> waitingRecipeSOList; // this is where we are containing all the recipes that the customer is waiting for/ generated to.
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;
    private int successfulRecipesAmount;


    private void Awake() {
        
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO> (); // initialise the list...
    }


    private void Update() {
        
        spawnRecipeTimer -= Time.deltaTime; // ...countdown the timer

        if (spawnRecipeTimer <= 0f) { //...when it reaches zero
            spawnRecipeTimer = spawnRecipeTimerMax;  //...reset the timer

            if (waitingRecipeSOList.Count < waitingRecipesMax) {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)]; //...grab a random recipeSO from the list
              
                waitingRecipeSOList.Add(waitingRecipeSO); // add it to the list above

                OnRecipeSpawned?.Invoke (this, EventArgs.Empty);

            }
        }
    }


    public void DeliverRecipe(PlateKitchenObject plateKitchenObject) { //let's receive a plate in the parameter, because the plate is going to have the ingredients on it/ or not

        for (int i = 0; i < waitingRecipeSOList.Count; i++) {

            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            // Do the plate ingredients match the orders (waitingRecipeSOList), that is what we are asking here...

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count) { // first, check the count of the ingredients on the plate match
                                                                                                                  // Has the same number of ingredients, as a first check

                bool plateContentsMatchesRecipe = true;

                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList) {
                    // Cycling through all the ingredients in the recipe...

                    bool ingredientFound = false; // this is to keep track of matches

                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
                        // Cycling through all the ingredients on the plate...

                        if (plateKitchenObjectSO == recipeKitchenObjectSO) {
                            //...Then ingredient matches!
                            ingredientFound = true;
                            break;
                        }
                    }

                    if (!ingredientFound) {
                        // This recipe ingredient was not found on the plate
                        plateContentsMatchesRecipe = false;
                    }
                }

                if (plateContentsMatchesRecipe) {
                    // Player delivered the correct recipe!
                    successfulRecipesAmount++;

                    Debug.Log("Player DID delivered the correct recipe!");
                    waitingRecipeSOList.RemoveAt(i);

                    OnRecipeCompleted?.Invoke( this, EventArgs.Empty );
                    OnRecipeSuccess?.Invoke( this, EventArgs.Empty );
                    return; // this takes us out of the check
                }

            }

        }
        // when it reaches the end of this FOR, then NO matches have been found!
        // player did not deliver a correct recipe
        Debug.Log("Player DID NOT deliver the correct recipe!");
        OnRecipeFailed?.Invoke( this, EventArgs.Empty );
    }


    public List<RecipeSO> GetWaitingRecipeSOList() {
        return waitingRecipeSOList;
    }

    public int GetSuccessfulRecipesAmount() {

        return successfulRecipesAmount;
    }


}
