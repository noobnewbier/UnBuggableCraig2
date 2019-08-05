using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine.Support.EventAggregator;

public struct FinishSpawnEvent { }

public class EnemySpawner : MonoBehaviour
{
    static int _count;

    [SerializeField] Transform _enemyTransformParent;
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] Transform[] _spawnPoints;

    readonly float _spawnInterval = 0.25f; //avoid enemies spawning at the same pos
    IEventAggregator _eventAggregator;

    private void Start()
    {
        _eventAggregator = ServiceLocator.EventAggregator;
    }
    public void SpawnEnemy()
    {
        StartCoroutine(SpawnEnemyCoroutine());
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        yield return new WaitForSeconds(_spawnInterval);
        Transform randomSpawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length - 1)];

        GameObject enemy = Instantiate(_enemyPrefab, _enemyTransformParent);
        enemy.name = "enemy" + _count++;
        enemy.transform.position = randomSpawnPoint.position;
        enemy.transform.LookAt(ServiceLocator.Instance.PlayerControler.transform);

        _eventAggregator.Publish(new FinishSpawnEvent());
        yield break;
    }
}
