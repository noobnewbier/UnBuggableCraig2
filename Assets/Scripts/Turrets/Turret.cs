using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Engine.Support.EventAggregator;

public class TurretTargetChanEventArgs : EventArgs
{
    public Enemy Enemy { get; private set; }
    public Turret Turret { get; private set; }
    public TurretTargetChanEventArgs(Enemy enemy, Turret turret)
    {
        Enemy = enemy;
        Turret = turret;
    }
}


public class Turret : MonoBehaviour, IHandle<RangeEvent>, IHandle<EnemyDeathEvent>
{
    public float Damage { get { return _turretData.Bullet.Damage; } }
    public float Range { get { return _turretData.Bullet.Range; } }
    public TurretData TurretData { get { return _turretData; } }
    //public Status Status { get { return _turretData.Bullet.Status; } }

    [SerializeField] TurretRange _turrentRange;
    [SerializeField] TurretData _turretData;
    [SerializeField] Transform _cannon;//starting point of bullet
    [SerializeField] Transform _pylon;

    IEventAggregator _eventAggregator;
    AudioSource _audioSource;
    List<Enemy> _inRangeEnemies = new List<Enemy>();
    Enemy _closestEnemy;
    float _timer;

    // Use this for initialization
    void Start()
    {
        _eventAggregator = ServiceLocator.EventAggregator;
        _eventAggregator.Subscribe(this);

        _turrentRange.Range = _turretData.Bullet.Range + 1;//+1 to see a bit further than the real range
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _turretData.Frequency && _closestEnemy != null)
        {
            _timer = 0;
            Shoot();
        }
        if (_closestEnemy != null)
            _pylon.LookAt(new Vector3(_closestEnemy.transform.position.x, _closestEnemy.transform.position.y - 0.5f, _closestEnemy.transform.position.z));
    }

    private void OnDestroy()
    {
        _eventAggregator.Unsubscribe(this);
    }

    void Shoot()
    {
        if (!_audioSource.isPlaying)
            _audioSource.Play();

        Vector3 direction = _pylon.forward;
        Bullet newBullet = _turretData.Bullet.GetPooledInstance().GetComponent<Bullet>();
        newBullet.transform.LookAt(new Vector3(_closestEnemy.transform.position.x, _closestEnemy.transform.position.y - 0.1f, _closestEnemy.transform.position.z));
        newBullet.Direction = direction;
        newBullet.transform.position = _cannon.transform.position;
        newBullet.Origin = newBullet.transform.position;
    }


    //if too slow consider sort everytime
    void FindClosestEnemy()
    {
        //hack
        _inRangeEnemies.Where(e => e != null).ToList();
        if (!_inRangeEnemies.Any())
        {
            _closestEnemy = null;
        }
        else
        {
            //Enemy previousTarget = _closestEnemy;
            _closestEnemy = _inRangeEnemies.Aggregate((current, next) =>
            {
                return Vector3.Distance(transform.position, current.transform.position) <= Vector3.Distance(transform.position, next.transform.position) ?
                    current : next;
            });

            //maybe I don't need this. Should be removed once confirmed
            //if (previousTarget != _closestEnemy)
            //    EventManager.Instance.OnTurretSwitchTarget(new TurretTargetChanEventArgs(_closestEnemy, this));
        }
    }

    public void Handle(EnemyDeathEvent @event)
    {
        _inRangeEnemies.Remove(@event.Enemy);

        FindClosestEnemy();
    }

    public void Handle(RangeEvent @event)
    {
        if (@event.Origin != _turrentRange) return; //we don't care range event from other dudes

        switch (@event.RangeType)
        {
            case RangeEvent.Type.Exit:
                OnEnemyExit(@event);
                break;
            case RangeEvent.Type.Enter:
                OnEnmeyEnter(@event);
                break;
            default:
                throw new ArgumentException("Wtf is that event?");
        }

        FindClosestEnemy();
    }

    void OnEnemyExit(RangeEvent @event)
    {
        _inRangeEnemies.Remove(@event.Enemy);
    }

    void OnEnmeyEnter(RangeEvent @event)
    {
        _inRangeEnemies.Add(@event.Enemy);
    }
}
