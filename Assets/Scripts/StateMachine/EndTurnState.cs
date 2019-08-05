using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EndTurnEvent { }

public class EndTurnState : StateMachineBehaviour
{
    float _timer;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        _timer = 0;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        _timer += Time.deltaTime;
        if (_timer > ServiceLocator.Instance.GameManager.GameConfigData.PrepareTime)
        {
            ServiceLocator.EventAggregator.Publish(new EndTurnEvent());
            animator.SetTrigger("FinishTurnWait");
        }
    }
}
