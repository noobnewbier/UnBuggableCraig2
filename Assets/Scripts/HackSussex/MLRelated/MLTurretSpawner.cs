//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Linq;

//public class MLTurretSpawner : MonoBehaviour
//{
//    public static MLTurretSpawner Instance { get; private set; }

//    [SerializeField] List<GameObject> _prefabs;
//    [SerializeField] GameObject _floor;

//    void Awake()
//    {
//        Instance = this;
//    }

//    public void GenerateMap()
//    {
//        for (int i = 0; i < GameConfig.MAX_TURRET_NUM; i++)
//        {
//            GameObject newTurret = _prefabs[Random.Range(0, _prefabs.Count - 1)];
//            Vector3 position = new Vector3(Random.Range(-GameManager.Instance.XRange, GameManager.Instance.XRange),
//                _floor.transform.position.y, Random.Range(-GameManager.Instance.ZRange, GameManager.Instance.ZRange));

//            TurretManager.Instance.CreateTurret(newTurret, position);
//        }
//    }
//}
