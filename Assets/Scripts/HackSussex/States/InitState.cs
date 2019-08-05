using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitState : StateMachineBehaviour {
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        //we have nothing to init atm
        animator.SetTrigger("FinishInit");
    }
}
