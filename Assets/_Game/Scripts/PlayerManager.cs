using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // ToDo: Think of Required Fields
    private Animator _animator;
    private InputManager _inputManager;
    private PlayerLocomotion _playerLocomotion;
    private PlayerAim _playerAim;

    public bool isInteracting;
    public bool isUsingRootMotion;
    private static readonly int IsInteracting = Animator.StringToHash("IsInteracting");
    private static readonly int IsUsingRootMotion = Animator.StringToHash("IsUsingRootMotion");
    private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerAim = GetComponent<PlayerAim>();
        _inputManager = GetComponent<InputManager>();
        _playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void Update()
    {
        _inputManager.HandleAllInputs();
    }

    // When working with Rigidbodies everything behaves much nicer in fixedUpdate -- Unity specific 
    // everything moving related should happen here 
    private void FixedUpdate()
    {
        _playerLocomotion.HandleAllMovements();
        _playerAim.HandleRotation();
    }

    //afer Frame ended
    private void LateUpdate()
    {
        isInteracting = _animator.GetBool(IsInteracting);
        isUsingRootMotion = _animator.GetBool(IsUsingRootMotion);
        _playerLocomotion.isJumping = _animator.GetBool(IsJumping);

        _animator.SetBool(IsGrounded, _playerLocomotion.isGrounded);
    }
}