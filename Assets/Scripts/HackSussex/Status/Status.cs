using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine.Support.EventAggregator;
using System;

public struct StatusFinishEvent
{
    public Status Status { get; private set; }
    public Enemy Enemy { get; private set; }

    public StatusFinishEvent(Status status, Enemy enemy) : this()
    {
        Status = status;
        Enemy = enemy;
    }

}

[Serializable]
public abstract class Status : ScriptableObject
{
    protected Enemy enemy;

    [SerializeField] float _duration;
    float _timer;
    IEventAggregator _eventAggregator;
    
    public virtual void BeginStatus(Enemy enemy, IEventAggregator eventAggregator)
    {
        this.enemy = enemy;
        _eventAggregator = eventAggregator;
    }

    public virtual void InStaus()
    {
        _timer += Time.deltaTime;
        if (_timer >= _duration)
            FinishStatus();
    }

    protected virtual void FinishStatus()
    {
        _eventAggregator.Publish(new StatusFinishEvent(this, enemy));
    }
}
