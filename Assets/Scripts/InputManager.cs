using UnityEngine;

public class InputManager : MonoBehaviour
{
    ThirdPersonInputActionsAsset playerControls;
    PlayerLocomotion playerLocomotion;  // ToDo: Think of Required Field 
    AnimatorManager animatorManager;   // ToDo: Think of Required Field 
    PlayerCombatManager playerCombatManager; // ToDo: Think of Required Field 
    SwitchVirtualCamera virtualCameraSwitcher; // ToDo: Think of Required Field 

    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    public float moveAmount;
    public float horizontalInput;
    public float verticalInput;

    public bool crouch_input;

    public bool b_Input;
    public bool x_Input;
    public bool jump_input;

    public bool right_trigger_input;
    public bool left_trigger_input;

    public bool right_button_hold_input; // todo think of sth better
    public bool left_button_hold_input; // todo think of sth better

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
        // should be just one 
        virtualCameraSwitcher = FindObjectOfType<SwitchVirtualCamera>();
    }
    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new ThirdPersonInputActionsAsset();

            // ToDo: Describe this Shit!
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>(); // if you move the mouse or the right joystick it will then send it to the camera input // more explain needed

            playerControls.PlayerActions.B.performed += i => b_Input = true; // b hit --> when holding it 
            playerControls.PlayerActions.B.canceled += i => b_Input = false;

            playerControls.PlayerActions.X.performed += i => x_Input = true; // set true when pressed 
            playerControls.PlayerActions.X.canceled += i => x_Input = false;

            playerControls.PlayerActions.Jump.performed += i => jump_input = true; // set true when pressed 
            playerControls.PlayerActions.Jump.canceled += i => jump_input = false;

            playerControls.PlayerActions.RT.performed += i => right_trigger_input = true;
            playerControls.PlayerActions.RT.canceled += i => right_trigger_input = false;

            playerControls.PlayerActions.LT.performed += i => left_trigger_input = true;
            playerControls.PlayerActions.LT.canceled += i => left_trigger_input = false;

            playerControls.PlayerActions.RB_Hold.performed += i => right_button_hold_input = true;
            playerControls.PlayerActions.RB_Hold.canceled += i => right_button_hold_input = false;

            playerControls.PlayerActions.LB_Hold.performed += i => left_button_hold_input = true;
            playerControls.PlayerActions.LB_Hold.canceled += i => left_button_hold_input = false;

            playerControls.PlayerMovement.ToggleCrouching.performed += i => crouch_input = true;
            playerControls.PlayerMovement.ToggleCrouching.canceled += i => crouch_input = false;
        }
        playerControls.Enable();
    }
    private void OnDisable()
    {
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

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput)); // clamp value between 0 and 1 // Abs - Absolute Value

        //ToDo: set animator values for rotation
        animatorManager.UpdateAnimatorValues(0, moveAmount, playerLocomotion.isSprinting);
    }
    private void HandleSprintingInput()
    {
        if (b_Input == true && moveAmount > 0.5f)
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
        if (jump_input)
        {
            jump_input = false;
            playerLocomotion.HandleJumping();
        }
    }
    private void HandleSlideInput()
    {
        if (x_Input)
        {
            playerLocomotion.HandleSlide();
        }
    }
    private void HandleAttackInput() // maybe wrap in action input#
    {
        if (right_trigger_input || right_button_hold_input)
        {
            playerCombatManager.HandleAttack(right_trigger_input, right_button_hold_input);
        }
    }
    private void HandleDefenseInput()
    {
        if (left_button_hold_input)
        {
            playerCombatManager.HandleDefense();
        }
    }

    private void HandleCrouchInput()
    {
        playerLocomotion.HandleCrouchInput(crouch_input);
    }
}

