//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ResetState : StateMachineBehaviour
//{

//    Animator _animator;


//    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
//    {
//        _animator = animator;
//        EventManager.Instance.OnFinishSetUpEvent += OnFinishedSetUp;
//        EventManager.Instance.NextRound();
//    }

//    void OnFinishedSetUp(object sender, EventArgs eventArgs)
//    {
//        EventManager.Instance.OnFinishSetUpEvent -= OnFinishedSetUp;
//        _animator.SetTrigger("FinishSetUp");
//    }
//}
