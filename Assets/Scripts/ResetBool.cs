using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBool : StateMachineBehaviour
{
    public string isInteractingBool;
    public bool isÍnteractingStatus;

    public string isUsingRootMotiongBool;
    public bool isUsingRootMotiongStatus;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(isInteractingBool, isÍnteractingStatus);
        animator.SetBool(isUsingRootMotiongBool, isUsingRootMotiongStatus);
    }
}
