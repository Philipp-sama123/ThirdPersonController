using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // ToDo: Think of Required Field 
    InputManager InputManager;
    public Transform targetTransform; // The Object the Camera will follow (!) 
    public Transform cameraPivot; // The Object the camera uses to pivot (look up and down) (!)
    public Transform cameraTransform; // The transform of the actual camera object in the scene 
    private Vector3 cameraFollowVelocity = Vector3.zero;

    // ToDo: Think of Required Field 
    public LayerMask collisionLayers; // The layers we want our camera to collide with in the Enviroment  
    private float defaultPosition;
    private Vector3 cameraVectorPosition;

    public float cameraCllisionOffset = 2f; // how much the camera will jump off of objects its colliding with 
    public float minimumCollisionOffset = 0.2f;
    public float cameraCollisionRadius = 2f;
    public float cameraFollowSpeed = 0.2f;
    public float cameraLookSpeed = 2f;
    public float cameraPivotSpeed = 2f;
    public float lookAngle; // Camera Look up and down
    public float pivotAngle; // Camera Look left and right
    public float minimumPivotAngle = -35f;
    public float maximumPivotAngle = 35f;


    private void Awake()
    {
        // should be only one in a scene 
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        InputManager = FindObjectOfType<InputManager>();
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
    }
    // called in lateUpdate -- means it is called after each frame has been processed
    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();
    }
    private void FollowTarget()
    {
        // Vector3.SmoothDamp which is used to move something soft and between one location and another -- usally for cameras
        Vector3 targetPosition =
            Vector3.SmoothDamp(
            transform.position, targetTransform.position, ref (cameraFollowVelocity), cameraFollowSpeed);
        transform.position = targetPosition;
    }
    private void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRotation;

        lookAngle = lookAngle + (InputManager.cameraInputX * cameraLookSpeed);
        pivotAngle = pivotAngle - (InputManager.cameraInputY * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle); // to clap the maximum and minimum values of the pivot angle 

        rotation = Vector3.zero;
        rotation.y = lookAngle;

        // todo(understand)
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;

        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;

    }
    private void HandleCameraCollisions()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();


        // fires a raycast starting at cameraPivottransform
        if (Physics.SphereCast
            (cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
        {
            // if we hit something we wanna do stuff with hit
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition = -(distance - cameraCllisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
        {
            targetPosition =- minimumCollisionOffset;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, .2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }

  // changed(transform.position = targetRotation) to(transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, cameraLookAngleSpeed * Time.deltaTime);) as it seems to have help a bit.
}
