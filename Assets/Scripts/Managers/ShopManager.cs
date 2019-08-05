using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine.Support.EventAggregator;

public struct MoneyEvent
{
    public float Money { get; private set; }

    public MoneyEvent(float money) : this()
    {
        Money = money;
    }
}

public class ShopManager : MonoBehaviour
{
    [SerializeField] List<GameObject> _availableTurretsPrefabs;

    PlayerData _playerData;
    IEventAggregator _eventAggregator;

    public List<GameObject> AvailableTurretsPrefabs { get { return _availableTurretsPrefabs; } }

    void Start()
    {
        _playerData = ServiceLocator.Instance.GameManager.GameConfigData.PlayerData;
        _eventAggregator = ServiceLocator.EventAggregator;
    }

    public bool Purchase(TurretData turretData)
    {
        if (_playerData.Money < turretData.Cost)
            return false; //couldn't buy you dickhead

        _playerData.Money -= turretData.Cost;
        _eventAggregator.Publish(_playerData.Money);

        return true;
    }
}
