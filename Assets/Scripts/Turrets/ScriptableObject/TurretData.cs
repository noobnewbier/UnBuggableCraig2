using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/TurretData")]
public class TurretData : ScriptableObject
{
    [SerializeField] string _turretName;
    [SerializeField] float _shootFrequency;
    [SerializeField] int _cost;
    [SerializeField] Bullet _bullet;

    public string Turretname { get { return _turretName; } }
    public float Frequency { get { return _shootFrequency; } }
    public Bullet Bullet { get { return _bullet; } }
    public int Cost { get { return _cost; } }
}
