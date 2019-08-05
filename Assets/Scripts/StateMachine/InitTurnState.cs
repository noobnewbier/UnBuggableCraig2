using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine.Support.EventAggregator;

public struct StartNewTurnEvent { }

public class InitTurnState : StateMachineBehaviour, IHandle<FinishSpawnEvent>
{
    int _turn;
    int _spawnedEnemyCount;
    int _expectedEnemyCount;
    Animator _stateMachine;
    EnemySpawner _enemySpawner;
    GameConfigData _configData;
    IEventAggregator _eventAggregator;

    public void Handle(FinishSpawnEvent @event)
    {
        _spawnedEnemyCount++;
        if (_spawnedEnemyCount >= _expectedEnemyCount)
        {
            _stateMachine.SetTrigger("FinishInitTurn");
        }
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        _turn++;
        _eventAggregator = ServiceLocator.EventAggregator;
        _enemySpawner = ServiceLocator.Instance.EnemySpawner;
        _stateMachine = animator;
        _configData = ServiceLocator.Instance.GameManager.GameConfigData;

        _spawnedEnemyCount = 0;
        _eventAggregator.Subscribe(this);
        _expectedEnemyCount = _configData.InitialEnemyAmount + _turn * _configData.IncrementInEnemyAmount;

        for (int i = 0; i < _expectedEnemyCount; i++)
        {
            _enemySpawner.SpawnEnemy();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        _eventAggregator.Unsubscribe(this);
        _eventAggregator.Publish(new StartNewTurnEvent());
    }
}
