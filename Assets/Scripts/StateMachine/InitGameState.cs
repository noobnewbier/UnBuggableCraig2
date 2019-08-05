using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitGameState : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        //dummy ones, we will figure out the rest when we need to
        animator.SetTrigger("FinishInitGame");
    }
}
