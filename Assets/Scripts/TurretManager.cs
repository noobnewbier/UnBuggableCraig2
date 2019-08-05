using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurretManager : MonoBehaviour
{
    public List<GameObject> Turrets { get; private set; }

    private void Awake()
    {
        Turrets = new List<GameObject>();
    }

    public void CreateTurret(GameObject prefab, Vector3 position)
    {
        GameObject newTurret = Instantiate(prefab);
        newTurret.transform.position = position;
        Turrets.Add(newTurret);
    }

    public void ClearTurrets()
    {
        Turrets.ForEach(t => Destroy(t));
        Turrets.Clear();
    }

}
