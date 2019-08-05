using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine.Support.EventAggregator;
using System;

public struct RangeEvent
{
    public enum Type
    {
        Exit,
        Enter
    }
    public TurretRange Origin { get; private set; }
    public Enemy Enemy { get; private set; }
    public Type RangeType { get; private set; }
    public RangeEvent(TurretRange orign, Enemy enemy, Type type)
    {
        Origin = orign;
        Enemy = enemy;
        RangeType = type;
    }
}


public class TurretRange : MonoBehaviour
{

    SphereCollider _collider;

    public float Range { set { transform.localScale = new Vector3(value, value, value); } }
    IEventAggregator _eventAggregator;
    

    // Use this for initialization
    void Awake()
    {
        _eventAggregator = ServiceLocator.EventAggregator;
        _collider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.ENEMY))
        {
            RangeEvent eventArgs = new RangeEvent(this, other.GetComponent<Enemy>(), RangeEvent.Type.Enter);

            _eventAggregator.Publish(eventArgs);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.ENEMY))
        {
            RangeEvent eventArgs = new RangeEvent(this, other.GetComponent<Enemy>(), RangeEvent.Type.Exit);

            _eventAggregator.Publish(eventArgs);
        }
    }

}
