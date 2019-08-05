using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine.Support.EventAggregator;

public class DuringTurnState : StateMachineBehaviour
{
    float _timer;
    int _diedEnemy;
    GameManager _gameManager;
    Animator _animator;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        _timer = 0;
        _diedEnemy = 0;
        _animator = animator;
        _gameManager = ServiceLocator.Instance.GameManager;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        _timer += Time.deltaTime;
        if (_timer > ServiceLocator.Instance.GameManager.GameConfigData.TurnTime)
        {
            animator.SetTrigger("TimesUp");
        }
    }

}
