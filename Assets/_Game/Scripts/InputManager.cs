using UnityEngine;

public class InputManager : MonoBehaviour
{
    ThirdPersonInputActionsAsset playerControls;
    PlayerLocomotion playerLocomotion; // ToDo: Think of Required Field 
    AnimatorManager animatorManager; // ToDo: Think of Required Field 
    PlayerCombatManager playerCombatManager; // ToDo: Think of Required Field 
    SwitchVirtualCamera switchVirtual;

    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    public float moveAmount;
    public float horizontalInput;
    public float verticalInput;

    public bool crouchInput;

    public bool bInput;
    public bool xInput;
    public bool jumpInput;

    public bool rightTriggerInput;
    public bool leftTriggerInput;

    public bool rightButtonHoldInput; // todo think of sth better
    public bool leftButtonHoldInput; // todo think of sth better

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
        switchVirtual = FindObjectOfType<SwitchVirtualCamera>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new ThirdPersonInputActionsAsset();


            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed +=
                i => cameraInput =
                    i.ReadValue<Vector2>(); // if you move the mouse or the right joystick it will then send it to the camera input // more explain needed

            playerControls.PlayerActions.B.performed += i => bInput = true;
            playerControls.PlayerActions.B.canceled += i => bInput = false;

            playerControls.PlayerActions.X.performed += i => xInput = true;
            playerControls.PlayerActions.X.canceled += i => xInput = false;

            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
            playerControls.PlayerActions.Jump.canceled += i => jumpInput = false;

            playerControls.PlayerActions.RT.performed += i => rightTriggerInput = true;
            playerControls.PlayerActions.RT.canceled += i => rightTriggerInput = false;

            playerControls.PlayerActions.RB_Hold.performed += i => rightButtonHoldInput = true;
            playerControls.PlayerActions.RB_Hold.canceled += i => rightButtonHoldInput = false;

            playerControls.PlayerActions.LB_Hold.performed += i => leftButtonHoldInput = true;
            playerControls.PlayerActions.LB_Hold.canceled += i => leftButtonHoldInput = false;

            playerControls.PlayerMovement.ToggleCrouching.performed += i => crouchInput = true;
            playerControls.PlayerMovement.ToggleCrouching.canceled += i => crouchInput = false;

            playerControls.PlayerActions.LT.performed += _ => HandleAimingInput(true);
            playerControls.PlayerActions.LT.canceled += _ => HandleAimingInput(false);
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        //    playerControls.PlayerActions.LT.performed -= _ => HandleAimingInput(true);
        //    playerControls.PlayerActions.LT.canceled -= _ => HandleAimingInput(false);

        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
        HandleSlideInput();

        HandleAttackInput();
        HandleDefenseInput();
        HandleCrouchInput();

        //    HandleActionInput(); 
    }

    private void HandleMovementInput()
    {
        horizontalInput = movementInput.x;
        verticalInput = movementInput.y;

        cameraInputX = cameraInput.x; // take input from joystick and then pass it to move the camera 
        cameraInputY = cameraInput.y;

        moveAmount =
            Mathf.Clamp01(Mathf.Abs(horizontalInput) +
                          Mathf.Abs(verticalInput)); // clamp value between 0 and 1 // Abs - Absolute Value
        
        animatorManager.UpdateAnimatorValues(horizontalInput, moveAmount, playerLocomotion.isSprinting);
    }

    private void HandleSprintingInput()
    {
        if (bInput == true && moveAmount > 0.5f)
        {
            playerLocomotion.isSprinting = true;
        }
        else
        {
            playerLocomotion.isSprinting = false;
        }
    }

    private void HandleJumpingInput()
    {
        if (jumpInput)
        {
            jumpInput = false;
            playerLocomotion.HandleJumping();
        }
    }

    private void HandleSlideInput()
    {
        if (xInput)
        {
            playerLocomotion.HandleSlide();
        }
    }

    private void HandleAttackInput() // maybe wrap in action input#
    {
        if (rightTriggerInput || rightButtonHoldInput)
        {
            playerCombatManager.HandleAttack(rightTriggerInput, rightButtonHoldInput);
        }
    }

    private void HandleDefenseInput()
    {
        if (leftButtonHoldInput)
        {
            playerCombatManager.HandleDefense();
        }
    }

    private void HandleCrouchInput()
    {
        playerLocomotion.HandleCrouchInput(crouchInput);
    }

    private void HandleAimingInput(bool isAiming)
    {
        if (isAiming)
        {
            switchVirtual.StartAiming();
        }
        else
        {
            switchVirtual.CancelAiming();
        }
    }
}