using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // ToDo: Think of Required Fields
    Animator animator;
    InputManager inputManager;
    PlayerLocomotion playerLocomotion;
    PlayerAim playerAim;

    public bool isInteracting;
    public bool isUsingRootMotion; 

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerAim = GetComponent<PlayerAim>();
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }
    private void Update()
    {
        inputManager.HandleAllInputs();
    }
    // When working with Rigidbodies everything behaves much nicer in fixedUpdate -- Unity specific 
    // everything moving related should happen here 
    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovements();
        playerAim.HandleRotation(); 
    }
    //afer Frame ended
    private void LateUpdate()
    {
       // camera.HandleAllCameraMovement();

        isInteracting = animator.GetBool("IsInteracting");
        isUsingRootMotion = animator.GetBool("IsUsingRootMotion");
        animator.SetBool("IsGrounded", playerLocomotion.isGrounded);

        playerLocomotion.isJumping = animator.GetBool("IsJumping");
    }
}
