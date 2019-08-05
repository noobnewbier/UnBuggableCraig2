using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearByUnitDetector : MonoBehaviour
{
    public List<Enemy> Allies { get; private set; }
    Collider _collider;

    private void Awake()
    {
        Allies = new List<Enemy>();
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.ENEMY))
            Allies.Add(other.GetComponent<Enemy>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.ENEMY))
            Allies.Remove(other.GetComponent<Enemy>());
    }
}
