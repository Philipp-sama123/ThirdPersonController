using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    // ToDo: Think of Required Field 
    public Animator animator;
    PlayerManager playerManager;
    PlayerLocomotion playerLocomotion;
    private int vertical;
    private int horizontal;
    private int rotation;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerManager = GetComponent<PlayerManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();

        vertical = Animator.StringToHash("Horizontal");
        horizontal = Animator.StringToHash("Vertical");
        rotation = Animator.StringToHash("RotationMagnitude");
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting, bool useRootMotion = false)
    {
        animator.SetBool("IsInteracting", isInteracting);
        animator.SetBool("IsUsingRootMotion", useRootMotion);
        animator.CrossFade(targetAnimation, 0.2f);
    }
    public void UpdateAnimatorValues(float horivontalMovement, float verticalMovement, bool isSprinting)
    {

        //ToDo: Animation Snapping 
        // if you have a value - walk and you cant walk or run (rounds Value)
        // it snaps to the appropriate Animation

        float snappedHorizontal;
        float snappedVertical;

        #region SnappedHorizontal
        if (horivontalMovement > 0f && horivontalMovement < 0.55f)
        {
            snappedHorizontal = 0.5f;
        }
        else if (horivontalMovement > 0.55f)
        {
            snappedHorizontal = 1f;
        }
        else if (horivontalMovement < 0 && horivontalMovement > -0.55f)
        {
            snappedHorizontal = -0.5f;
        }
        else if (horivontalMovement < -0.55f)
        {
            snappedHorizontal = -1f;
        }
        else
        {
            snappedHorizontal = 0;
        }
        #endregion      
        #region SnappedVertical
        if (verticalMovement > 0f && verticalMovement < 0.55f)
        {
            snappedVertical = 0.5f;
        }
        else if (verticalMovement > 0.55f)
        {
            snappedVertical = 1f;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            snappedVertical = -0.5f;
        }
        else if (verticalMovement < -0.55f)
        {
            snappedVertical = -1f;
        }
        else
        {
            snappedVertical = 0;
        }
        #endregion

        if (isSprinting)
        {
            snappedHorizontal = horivontalMovement;
            snappedVertical = 2;
        }
        animator.SetFloat(vertical, snappedVertical, .1f, Time.deltaTime);
        animator.SetFloat(horizontal, snappedHorizontal, .1f, Time.deltaTime);
    }

    public void UpdateAnimatorRotationValues(float rotationInput)
    {
        float snappedRotation;

        #region SnappedRotation
        if (rotationInput > 0f && rotationInput < 0.3f)
        {
            snappedRotation = 0.25f;
        }
        else if (rotation > 0.3f && rotationInput < 0.55f)
        {
            snappedRotation = 0.5f;
        }
        else if (rotation > 0.55f && rotationInput < 0.8f)
        {
            snappedRotation = 0.75f;
        }
        else if (rotation > 0.8f)
        {
            snappedRotation = 1f;
        }
        else if (rotationInput < 0 && rotationInput > -0.3f)
        {
            snappedRotation = -0.25f;
        }
        else if (rotationInput < -0.55 && rotationInput > -0.8f )
        {
            snappedRotation = -0.75f;
        }
        else if (rotationInput < -0.8f)
        {
            snappedRotation = -1f;
        }
        else
        {
            snappedRotation = 0;
        }
        #endregion

        animator.SetFloat(rotation, rotationInput, .1f, Time.deltaTime);
    }
    //Callback for processing animation movements for modifying root motion.
    //This callback will be invoked at each frame after the state machines and the animations have been evaluated, but before OnAnimatorIK.
    private void OnAnimatorMove()
    {
        //use root Motion
        if (playerManager.isUsingRootMotion)
        {
            playerLocomotion.playerRigidbody.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / Time.deltaTime;
            playerLocomotion.playerRigidbody.velocity = velocity;
        }
    }
}
