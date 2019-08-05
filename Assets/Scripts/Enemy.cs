using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine.Support.EventAggregator;
using System.Linq;


public struct EnemyTakeDamageEvent
{
    public float Damage { get; private set; }
    public EnemyTakeDamageEvent(float damage)
    {
        Damage = damage;
    }
}

//public class TakeStatusEventArgs : EventArgs
//{
//    public Status Status { get; private set; }
//    public TakeStatusEventArgs(Status status)
//    {
//        Status = status;
//    }
//}

public struct SuccessfulAttackEvent
{
    public Enemy Attacker { get; private set; }

    public SuccessfulAttackEvent(Enemy attacker) : this()
    {
        Attacker = attacker;
    }
}

public struct EnemyDeathEvent
{
    public Enemy Enemy { get; private set; }
    public DamageCause Cause { get; private set; }
    public EnemyDeathEvent(Enemy enemy, DamageCause cause)
    {
        Enemy = enemy;
        Cause = cause;
    }
}

public enum DamageCause
{
    FALL,
    SHOT,
    BURN,
    ATTACK,
    SYSTEM
}

//hold nothing more than data, leave the rest to the agent
public class Enemy : MonoBehaviour, IHasSpeed, IHandle<StatusFinishEvent>
{
    [SerializeField] EnemyData _enemyData;

    public float MaxHealth { get { return _enemyData.MaxHealth; } }
    public float NormHealth { get { return _enemyData.Health / MaxHealth; } }

    public float Damage => _enemyData.Damage;
    public float ForwardSpeed
    {
        get
        {
            return _enemyData.ForwardSpeed;
        }
        set
        {
            _enemyData.ForwardSpeed = value;
        }
    }

    public float BackwardSpeed
    {
        get
        {
            return _enemyData.BackwardSpeed;
        }
        set
        {
            _enemyData.BackwardSpeed = value;
        }
    }

    public float RotateSpeed => _enemyData.RotateSpeed;

    List<Status> _statuses = new List<Status>();
    IEventAggregator _eventAggregator;


    private void Awake()
    {
        _enemyData = Instantiate(_enemyData);
        _eventAggregator = ServiceLocator.EventAggregator;
        _eventAggregator.Subscribe(this);
    }

    private void TakeDamage(float amount, DamageCause cause)
    {
        _enemyData.Health = _enemyData.Health - amount;
        _eventAggregator.Publish(new EnemyTakeDamageEvent(_enemyData.Health));

        if (_enemyData.Health <= 0)
        {
            Debug.Log(cause.ToString());
            Die(cause);
        }
    }

    private void FixedUpdate()
    {
        _statuses.ToList().ForEach(s => s.InStaus());

        if (transform.position.y < -1f)
        {
            TakeDamage(MaxHealth, DamageCause.FALL);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.BULLET))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            TakeDamage(bullet.Damage, DamageCause.SHOT);
            if (bullet.Status != null)
            {
                _statuses.Add(bullet.Status);
                bullet.Status.BeginStatus(this, _eventAggregator);
            }
        }

        if (other.CompareTag(Tags.DISCIPLE))
        {
            _eventAggregator.Publish(new SuccessfulAttackEvent(this));
            Die(DamageCause.ATTACK);
        }
    }

    //need tweaks for ml agent?
    void Die(DamageCause cause)
    {
        _eventAggregator.Publish(new EnemyDeathEvent(this, cause));
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _eventAggregator.Unsubscribe(this);
    }

    public void Handle(StatusFinishEvent @event)
    {
        if (@event.Enemy == this)
            _statuses.Remove(@event.Status);
    }
}
