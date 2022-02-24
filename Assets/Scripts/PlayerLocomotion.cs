using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    // ToDo: Think of Required Field 
    public Rigidbody playerRigidbody;
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    InputManager inputManager;
    Transform cameraObject;

    Vector3 moveDirection;

    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float rayCastHeightOffset = 0.5f;
    public LayerMask groundLayer;

    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;         // ToDo: think about it: Brackeys isGroundedCheck: isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask); 
    public bool isJumping;

    [Header("Movement Speeds")]
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 5f;
    public float sprintingSpeed = 7f;
    public float rotationSpeed = 15f;
    public float crouchingSpeedReducer = 5f;

    [Header("Jump Speeds")]
    public float jumpHeight = 3;
    public float gravityIntensity = -15;

    private bool isCrouching = false;


    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }
    public void HandleAllMovements()
    {
        HandleFallingAndLanding();
        if (playerManager.isInteracting)
        {
            return;
        }
        HandleMovement();
        HandleRotation();
    }
    private void HandleMovement()
    {
        isCrouching = animatorManager.animator.GetBool("IsCrouching");
        // prevents movement while jumping
        // Todo: maybe remove (!)
        if (isJumping)
        {
            return;
        }
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0; // prevent going up


        if (isSprinting)
        {
            moveDirection *= sprintingSpeed;
        }
        else
        {
            if (inputManager.moveAmount >= 0.5f)
            {
                moveDirection *= runningSpeed;
            }
            else
            {
                moveDirection *= walkingSpeed;
            }
        }

        if (isCrouching)
        {
            moveDirection /= crouchingSpeedReducer;
        }

        Vector3 movementVelocity = moveDirection;
        playerRigidbody.velocity = movementVelocity;

    }
    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;
        // prevents rotation while jumping
        if (isJumping)
        {
            return;
        }

        // Always face the direction you are running to
        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }
        //look towards target Rotation
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);  // Quaternions are used to calculate rotations in Unity 
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); // Should you use when you want behavior independent of the Frame Rate(!)  -- framerate will always differ!

        // ToDo: Handle rotation in Animator
    //    animatorManager.UpdateAnimatorRotationValues(targetRotation.z);

        transform.rotation = playerRotation;
    }
    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position;
        Vector3 targetPosition = transform.position; // for the feet
        raycastOrigin.y = raycastOrigin.y + rayCastHeightOffset;

        if (!isGrounded && !isJumping)
        {
            if (!playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Falling", true); // ToDo -- maybe find something for the Animator Stuff string references
            }
            animatorManager.animator.SetBool("IsUsingRootMotion", false);

            inAirTimer += Time.deltaTime;
            playerRigidbody.AddForce(transform.forward * leapingVelocity);  // step of the ledge
            playerRigidbody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
            //  -Vector3.up     --      means it pulls you downwards 
            //  * inAirTimer    --      the longer you are in the air the quicker you fall
        }
        if (Physics.SphereCast(raycastOrigin, 0.2f, -Vector3.up, out hit, groundLayer))
        {
            if (!isGrounded && !playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Land", true); // ToDo -- maybe find something for the Animator Stuff string references
            }

            Vector3 raycastHitPoint = hit.point; // where the raycast hits the ground 
            targetPosition.y = raycastHitPoint.y; // assign the point where the raycast hits the ground to target position

            inAirTimer = 0;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        // this is responsible for setting the feet over the ground when the player is grounded
        if (isGrounded && !isJumping)
        {
            if (playerManager.isInteracting || inputManager.moveAmount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                transform.position = targetPosition;
            }
        }
    }
    public void HandleJumping()
    {
        if (isGrounded)
        {
            animatorManager.animator.SetBool("IsJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false);
            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = moveDirection;

            playerVelocity.y = jumpingVelocity; // adds jumping velocity to movement
            playerRigidbody.velocity = playerVelocity;
        }
    }
    public void HandleSlide()
    {
        if (playerManager.isInteracting)
        {
            return;
        }
        animatorManager.PlayTargetAnimation("Slide Under", true, true);
        // toggle invulnerable bool here
    }

    public void HandleCrouchInput(bool isCrouching)
    {
        // todo fix
        animatorManager.animator.SetBool("IsCrouching", isCrouching);
    }
}
