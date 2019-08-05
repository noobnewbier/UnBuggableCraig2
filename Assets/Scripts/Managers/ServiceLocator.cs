using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine.Support.EventAggregator;
using System;

public class ServiceLocator : MonoBehaviour
{

    public static ServiceLocator Instance;
    static Lazy<IEventAggregator> _eventAggregator = new Lazy<IEventAggregator>(() => new EventAggregator());

    [SerializeField] UnitControler _playerControler;
    [SerializeField] GameManager _gameManager;
    [SerializeField] TurretManager _turretManager;
    [SerializeField] EnemySpawner _enemySpawner;
    [SerializeField] ShopManager _shopManager;
    [SerializeField] TurretPlacer _turretPlacer;

    public static IEventAggregator EventAggregator { get; } = _eventAggregator.Value;
    public UnitControler PlayerControler { get { return _playerControler; } }
    public GameManager GameManager { get { return _gameManager; } }
    public EnemySpawner EnemySpawner { get { return _enemySpawner; } }
    public ShopManager ShopManager { get { return _shopManager; } }
    public TurretManager TurretManager { get { return _turretManager; } }
    public TurretPlacer TurretPlacer { get { return _turretPlacer; } }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }
}
