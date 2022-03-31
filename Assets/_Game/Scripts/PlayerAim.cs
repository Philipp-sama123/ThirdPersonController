using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public float turnSpeed;
    public float rotationSpeed = 15f;
    private Camera mainCamera;
    private InputManager inputManager;
    private PlayerLocomotion playerLocomotion;

    private void Awake()
    {
        mainCamera = Camera.main;
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        // disables curso
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;
        // prevents rotation while jumping
        if (playerLocomotion.isJumping)
        {
            return;
        }

        // Always face the direction you are running to
        targetDirection = mainCamera.transform.forward * inputManager.verticalInput;
        targetDirection = targetDirection + mainCamera.transform.right * inputManager.horizontalInput;
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

}
