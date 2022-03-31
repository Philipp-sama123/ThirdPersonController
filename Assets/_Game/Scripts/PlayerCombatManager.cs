using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    public AnimatorManager animatorManager;
    public PlayerManager playerManager;
    public PlayerLocomotion playerLocomotion;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerManager = GetComponent<PlayerManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }
    public void HandleAttack(bool isWeakAttack, bool isStrongAttack)
    {
        if (playerManager.isInteracting)
        {
            return;
        }

        // ToDo: Handle Attacks better when attacking or blocking
        if (playerLocomotion.isGrounded)
        {
            if (isStrongAttack)
                animatorManager.PlayTargetAnimation("Strong Attack 1", true, true);
            if (isWeakAttack)
                animatorManager.PlayTargetAnimation("Weak Attack 1", true, true);
        }
    }
    public void HandleDefense()
    {
        // ToDo: Handle Attacks better when attacking or blocking
        if (playerLocomotion.isGrounded)
        {
            animatorManager.PlayTargetAnimation("Defense", true, true);
        }
    }
}

