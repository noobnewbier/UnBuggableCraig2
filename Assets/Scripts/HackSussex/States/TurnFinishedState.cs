//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class TurnFinishedState : StateMachineBehaviour
//{

//    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
//    {
//        EventManager.Instance.TurnEnd();
//        if (GameManager.Instance.Health <= 0)
//        {
//            animator.SetBool("GameDone", true);
//        }
//    }
//}
