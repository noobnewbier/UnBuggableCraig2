using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine.Support.EventAggregator;

[CreateAssetMenu(menuName = "Assets/SlowStatus")]
public class SlowStatus : Status
{
    [SerializeField] float _speedAffect;

    public override void BeginStatus(Enemy enemy, IEventAggregator eventAggregator)
    {
        base.BeginStatus(enemy, eventAggregator);
        enemy.BackwardSpeed -= _speedAffect;
        enemy.ForwardSpeed -= _speedAffect;
    }

    protected override void FinishStatus()
    {
        base.FinishStatus();
        enemy.BackwardSpeed += _speedAffect;
        enemy.ForwardSpeed += _speedAffect;
    }
}
