using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/BulletData")]
public class BulletData : ScriptableObject
{
    [SerializeField] float _range;
    [SerializeField] float _damage;
    [SerializeField] float _bulletSpeed;
    [SerializeField] Status _status;
    Status _statusCopy;

    public float Range { get { return _range; } }
    public float Damage { get { return _damage; } }
    public float Speed { get { return _bulletSpeed; } }
    public Status Status
    {
        get
        {
            if (_statusCopy == null && _status != null) _statusCopy = Instantiate(_status);
            return _statusCopy;
        }
    }
}
