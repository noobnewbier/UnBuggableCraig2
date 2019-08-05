using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine.Support.EventAggregator;


public struct SurvivedTimeEvent
{
    public float Time { get; private set; }
    public SurvivedTimeEvent(float time) : this()
    {
        Time = time;
    }
}

public struct PlayerTakeDamageEvent
{
    public PlayerData PlayerData { get; private set; }
    public PlayerTakeDamageEvent(PlayerData playerData) : this()
    {
        PlayerData = playerData;
    }
}

public class GameManager : MonoBehaviour, IHandle<SuccessfulAttackEvent>, IHandle<EnemyDeathEvent>
{
    [SerializeField] GameConfigData _configData;
    [SerializeField] Animator _stateMachine;
    public float SurvivedTime { get; private set; }
    public GameConfigData GameConfigData { get { return _configData; } }
    public float XRange { get; private set; }
    public float ZRange { get; private set; }

    IEventAggregator _eventAggregator;

    private void Awake()
    {
        Screen.fullScreen = true;
        Screen.SetResolution(2560, 1440, true);
        _eventAggregator = ServiceLocator.EventAggregator;
        _eventAggregator.Subscribe(this);

        Renderer ground = GameObject.FindGameObjectWithTag(Tags.GROUND).GetComponent<Renderer>();
        XRange = ground.bounds.size.x / 2f;
        ZRange = ground.bounds.size.z / 2f;
    }

    private void Update()
    {
        SurvivedTime += Time.deltaTime;
        _configData.PlayerData.Money += GameConfigData.MoneyGainPerUpdate;

        _eventAggregator.Publish(new MoneyEvent(_configData.PlayerData.Money));
        _eventAggregator.Publish(new SurvivedTimeEvent(SurvivedTime));
    }

    public void Handle(SuccessfulAttackEvent @event)
    {
        _configData.PlayerData.Health -= @event.Attacker.Damage;
        _eventAggregator.Publish(new PlayerTakeDamageEvent(_configData.PlayerData));
        if(_configData.PlayerData.Health <= 0)
        {
            _stateMachine.SetTrigger("PlayerDie");
        }
    }

    private void OnDestroy()
    {
        _eventAggregator.Unsubscribe(this);
    }

    public void Handle(EnemyDeathEvent @event)
    {
        _configData.PlayerData.Money += _configData.KillReward;
    }
}
