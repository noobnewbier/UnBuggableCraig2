using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/EnemyData")]
public class EnemyData : ScriptableObject, IHasSpeed
{
    [SerializeField] float _health;
    [SerializeField] float _maxHealth;
    [SerializeField] float _forwardSpeed;
    [SerializeField] float _backwardSpeed;
    [SerializeField] float _rotateSpeed;
    [SerializeField] int _damage;

    public float Health { get { return _health; } set { _health = value; } }
    public float MaxHealth { get { return _maxHealth; } }
    public int Damage => _damage;

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
