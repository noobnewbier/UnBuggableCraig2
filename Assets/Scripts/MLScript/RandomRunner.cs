using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//used in machine learning
public class RandomRunner : MonoBehaviour
{
    [SerializeField] UnitControler _unitControler;
    GameManager _gameManager;
    Vector3 _targetPosition;

    private void Awake()
    {
        _gameManager = ServiceLocator.Instance.GameManager;
    }

    private void Start()
    {
        RandomizeTargetPosition();
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(_targetPosition, transform.position) < 0.1f) //super near target
        {
            RandomizeTargetPosition();
        }
        transform.LookAt(_targetPosition);

        _unitControler.InputControl(0f, 1f);
    }

    void RandomizeTargetPosition()
    {
        _targetPosition = new Vector3(
            Random.Range(-_gameManager.XRange + 0.1f, _gameManager.XRange - 0.1f),
           transform.position.y,
            Random.Range(-_gameManager.ZRange + 0.1f, _gameManager.ZRange - 0.1f));
    }
}
