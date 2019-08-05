using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    UnitControler _playerControler;

    private void Start()
    {
        _playerControler = ServiceLocator.Instance.PlayerControler;
    }

    private void FixedUpdate()
    {
        _playerControler.InputControl(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
}
