using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PooledMonoBehaviour
{

    [SerializeField] BulletData _bulletData;

    public Status Status => _bulletData.Status;
    public Vector3 Direction { set { _rigidBody.velocity = value * _bulletData.Speed; } }
    public float Damage { get { return _bulletData.Damage; } }
    public float Range { get { return _bulletData.Range; } }
    public Vector3 Origin { private get; set; }

    Rigidbody _rigidBody;

    private void OnEnable()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(Origin, transform.position) > _bulletData.Range)
        {
            SelfDestroy();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        SelfDestroy();
    }

    void SelfDestroy() { ReturnToPool(); }

}
