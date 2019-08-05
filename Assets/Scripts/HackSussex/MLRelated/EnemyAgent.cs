using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Engine.Support.EventAggregator;
using MLAgents;

public struct AgentDoneEvent
{

    public EnemyAgent Agent { get; private set; }

    public AgentDoneEvent(EnemyAgent agent)
    {
        Agent = agent;
    }
}

public class EnemyAgent : Agent, IHandle<EnemyDeathEvent>, IHandle<EndTurnEvent>
{
    static Transform _target;

    //todo : consider nearby dudes
    //NearByUnitDetector _nearByUnitDetector;
    [SerializeField] UnitControler _controler;
    [SerializeField] Enemy _enemy;
    [SerializeField] Rigidbody _rigidBody;
    IEventAggregator _eventAggregator;
    GameManager _gameManager;

    //todo: find a better way to do it
    Vector3 _previousPos;
    Vector3 _prevForward;

    int _turretsInRangeNum = 0;
    bool _fallOfEdgeFlag;
    bool _attackBagelFlag;
    bool _deadFlag;
    float _previousTokenDamage;
    float _timer;

    public override void InitializeAgent()
    {
        base.InitializeAgent();

        if (_target == null)
            _target = GameObject.FindGameObjectWithTag(Tags.DISCIPLE).transform;

        Initialize();

        //EventManager.Instance.OnDeathEvent += OnDeath;
    }

    void Initialize()
    {
        _eventAggregator = ServiceLocator.EventAggregator;
        _gameManager = ServiceLocator.Instance.GameManager;
        _eventAggregator.Subscribe(this);
        //_enemy.OnTakeDamageSubscribers += OnTakeDamage;
        //_enemy.OnTakeStatusSubscribers += OnTakeStatus;

        _enemy.transform.LookAt(_target.transform);//To nudge the agent

        _previousPos = _enemy.transform.position;
        _prevForward = _enemy.transform.forward;
        _deadFlag = false;
        _fallOfEdgeFlag = false;
        _attackBagelFlag = false;
        _timer = 0;
        //_previousTakenStatuses = null;
        //_previousTokenDamage = 0;
    }

    public override void AgentOnDone()
    {
        _eventAggregator.Unsubscribe(this);

        //_enemy.OnTakeDamageSubscribers -= OnTakeDamage;
        //_enemy.OnTakeStatusSubscribers -= OnTakeStatus;

        _eventAggregator.Publish(new AgentDoneEvent(this));
        Destroy(gameObject);
    }

    public override void CollectObservations()
    {
        CurrentPositionObservation();
        //AllFriendlyUnitsObservation();
        //AllTurretObservation();
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        _timer += Time.fixedDeltaTime;
        float h = Mathf.Clamp(vectorAction[0], -1f, 1f);
        float v = Mathf.Clamp(vectorAction[1], -1f, 1f);

        if (float.IsNaN(h) || float.IsNaN(v))
            return;

        _controler.InputControl(h, v);

        //assign awards

        //end reward
        NotFallOffTheEdge();
        GetToGoal();

        if (_deadFlag)
            return;
        //HealthControl();
        //TurretsWithinRange();
        //CurrentStatus();
        TimeConstraint();
        //NearByFriendlyUnits();
        //DistanceToGoal();
        IsWalkingToGoal();
        IsFacingGoal();
        _previousPos = _enemy.transform.position;
        _prevForward = _enemy.transform.forward;

        //Monitor.Log("reward", GetCumulativeReward(), _enemy.transform, Camera.main);
    }

    void NotFallOffTheEdge()
    {
        if (_deadFlag && _fallOfEdgeFlag)
        {
            SetReward(-1f);
            Done();
        }
    }

    void GetToGoal()
    {
        if (_deadFlag && _attackBagelFlag)
        {
            SetReward(1f);
            Done();
        }
    }

    //void HealthControl()
    //{
    //    if (_previousTokenDamage > 0)
    //    {
    //        _previousTokenDamage = 0;
    //        AddReward((-_previousTokenDamage / _enemy.MaxHealth) * 0.0001f);
    //    }
    //}

    //void TurretsWithinRange()
    //{
    //    float penalty = _turretsInRangeNum * -0.01f;
    //    AddReward(penalty);
    //}

    //void CurrentStatus()
    //{
    //    float penalty = _previousTakenStatuses.Count * -0.025f;
    //    _previousTakenStatuses.Clear();
    //    AddReward(penalty);
    //}

    void TimeConstraint()
    {
        //higher penalty when time past, avoid staying around the bagel forever
        AddReward(-0.001f);
    }

    //void NearByFriendlyUnits()
    //{
    //    AddReward(_nearByUnitDetector.Allies.Count * 0.025f);
    //}

    void IsFacingGoal()
    {
        float normAngle = Vector3.Dot((_target.transform.position - _enemy.transform.position).normalized, _enemy.transform.forward);
        AddReward(normAngle * 0.001f);
    }

    void IsWalkingToGoal()
    {
        float maxClosedDistance = _controler.HasSpeed.ForwardSpeed * Time.fixedDeltaTime;
        float walkedDistance = Vector3.Distance(_enemy.transform.position, _previousPos);
        float movingForwardDot = Vector3.Dot((_enemy.transform.position - _previousPos).normalized, (_target.position - _enemy.transform.position).normalized);

        float reward = 0.03f * walkedDistance / maxClosedDistance;
        if (movingForwardDot < 0)
        {
            movingForwardDot *= 1.5f; //amplifying the penalty
        }

        AddReward(movingForwardDot * 0.03f * walkedDistance / maxClosedDistance);
    }

    void CurrentPositionObservation()
    {
        float normAngle;
        Vector3 relativePos;
        if (_enemy != null)
        {
            normAngle = Vector3.Dot((_target.transform.position - _enemy.transform.position).normalized, _enemy.transform.forward);
            relativePos = _enemy.transform.position - _target.position;
        }
        else
        {
            normAngle = Vector3.Dot((_target.transform.position - _previousPos).normalized, _prevForward);
            relativePos = _previousPos - _target.position;
        }
        relativePos.x /= _gameManager.XRange;
        relativePos.z /= _gameManager.ZRange;

        AddVectorObs(new float[3] { relativePos.x, relativePos.z, normAngle });
        //AddVectorObs(new float[1] { normAngle }); 
    }

    //void AllFriendlyUnitsObservation()
    //{
    //    for (int i = 0; i < GameConfig.MAX_CHAN_NUM; i++)
    //    {
    //        float[] obs = new float[4] { 0, 0, 0, 0 };
    //        if (i < ChanSpawner.Instance.AliveChans.Count)
    //        {
    //            Vector3 allyPos = ChanSpawner.Instance.AliveChans[i]._enemy.transform.position;
    //            Vector3 relativePos = NormalizeXZ((_enemy.transform.position - allyPos).x, (_enemy.transform.position - allyPos).z);
    //            obs[0] = relativePos.x;
    //            obs[1] = relativePos.y;
    //            obs[2] = relativePos.z;
    //            obs[3] = ChanSpawner.Instance.AliveChans[i].GetComponent<Enemy>().NormHealth;
    //        }
    //        AddVectorObs(obs);
    //    }
    //}

    //void AllTurretObservation()
    //{
    //    foreach (var item in TrainChanSpawnerAgent.Instance.Observes)
    //    {
    //        AddVectorObs(item);
    //    }
    //}


    //void OnTakeDamage(object sender, TakeDamageEventArgs eventArgs)
    //{
    //    _previousTokenDamage += eventArgs.Damage;
    //}

    //void OnTakeStatus(object sender, TakeStatusEventArgs eventArgs)
    //{
    //    _previousTakenStatuses.Add(eventArgs.Status);
    //}
    //void OnTargetedByTurret(object sender, TurretTargetChanEventArgs eventArgs)
    //{
    //    if (eventArgs.Enemy != _enemy)
    //        return;

    //    AddReward(-0.05f);
    //}
    //void OnTargetRangeEvent(object sender, RangeEventArgs eventArgs)
    //{
    //    if (eventArgs.Enemy != _enemy)
    //        return;

    //    switch (eventArgs.RangeType)
    //    {
    //        case RangeEventArgs.Type.Exit:
    //            _turretsInRangeNum--;
    //            break;
    //        case RangeEventArgs.Type.Enter:
    //            _turretsInRangeNum++;
    //            break;
    //    }
    //}

    Vector3 NormalizeXZ(float x, float z)
    {
        return new Vector3((x + _gameManager.XRange) / (2f * _gameManager.XRange), 0, (z + _gameManager.ZRange) / (2 * _gameManager.ZRange));
    }

    public void Handle(EnemyDeathEvent @event)
    {
        if (@event.Enemy != _enemy)
            return;

        switch (@event.Cause)
        {
            case DamageCause.FALL:
                _fallOfEdgeFlag = true;
                break;
            case DamageCause.SHOT:
                //do nothing
                break;
            case DamageCause.BURN:
                //do nothing
                break;
            case DamageCause.ATTACK:
                _attackBagelFlag = true;
                break;
        }
        _deadFlag = true;
    }

    public void Handle(EnemyTakeDamageEvent @event)
    {
        //todo implement this shit
        //throw new NotImplementedException();
    }

    public void Handle(EndTurnEvent @event)
    {
        if (_gameManager.GameConfigData.IsTraining)
        {
            Done();
        }
    }
}
