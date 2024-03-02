using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private Animator animator;

    [SerializeField] private PlayerController playerController;

    private void Awake()
    {
        animator = GetComponent<Animator>(); // this gets the Animator Component on THIS game object, and sets it to the animator variable.
        

    }

    private void Update()
    {
        animator.SetBool(IS_WALKING, playerController.IsWalking()); //every frame this tells the animator boolean "is this character walking? yes or no?"
    }

}
