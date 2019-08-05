using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine.Support.EventAggregator;

[CreateAssetMenu(menuName = "Assets/GameConfigData")]
public class GameConfigData : ScriptableObject
{
    [SerializeField] PlayerData _playerBaseData; //keeping two instance of it to avoid modifying the original instance
    PlayerData _playerData;

    [SerializeField] float _prepareTime;
    [SerializeField] float _turnTime;
    [SerializeField] float _killReward;
    [SerializeField] float _moneyGainPerUpdate;
    [SerializeField] int _initialEnemyAmount;
    [SerializeField] int _incrementInEnemyAmount;
    [SerializeField] bool _isTraining;

    public PlayerData PlayerData
    {
        get
        {
            if (_playerData == null)
                _playerData = Instantiate(_playerBaseData);

            return _playerData;
        }
    }
    public float PrepareTime { get { return _prepareTime; } }
    public float TurnTime { get { return _turnTime; } }
    public int InitialEnemyAmount { get { return _initialEnemyAmount; } }
    public float MoneyGainPerUpdate { get { return _moneyGainPerUpdate; } }
    public int IncrementInEnemyAmount { get { return _incrementInEnemyAmount; } }
    public bool IsTraining => _isTraining;
    public float KillReward => _killReward;
}
