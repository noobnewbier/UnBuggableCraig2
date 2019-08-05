using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPlacer : MonoBehaviour
{
    ShopManager _shopManager;
    TurretManager _turretManager;

    private void Awake()
    {
        _turretManager = ServiceLocator.Instance.TurretManager;
        _shopManager = ServiceLocator.Instance.ShopManager;
    }

    public void PlaceTurret(GameObject turretPrefab)
    {
        if (_shopManager.Purchase(turretPrefab.GetComponent<Turret>().TurretData))
        {
            _turretManager.CreateTurret(turretPrefab, new Vector3(transform.position.x, 1f, transform.position.z) + transform.forward);
        }
    }
}
