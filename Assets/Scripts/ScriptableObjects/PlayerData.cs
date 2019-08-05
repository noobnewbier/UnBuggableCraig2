using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/PlayerData")]
public class PlayerData : ScriptableObject, IHasSpeed
{
    [SerializeField] float _money;
    [SerializeField] float _forwardSpeed;
    [SerializeField] float _backwardSpeed;
    [SerializeField] float _rotateSpeed;
    [SerializeField] float _health;
    [SerializeField] float _maxHealth;


    public float Money { get { return _money; } set { _money = value; } }
    public float Health { get { return _health; } set { _health = value; } }
    public float MaxHealth => _maxHealth;

    public float ForwardSpeed
    {
        get
        {
            return _forwardSpeed;
        }

        set
        {
            _forwardSpeed = value;
        }
    }

    public float BackwardSpeed
    {
        get
        {
            return _backwardSpeed;
        }

        set
        {
            _backwardSpeed = value;
        }
    }

    public float RotateSpeed
    {
        get
        {
            return _rotateSpeed;
        }

        set
        {
            _rotateSpeed = value;
        }
    }
}
