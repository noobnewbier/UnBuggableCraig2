using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasSpeed
{
    float ForwardSpeed { get; }
    float BackwardSpeed { get; }
    float RotateSpeed { get; }
}

public class UnitControler : MonoBehaviour
{
    [SerializeField] Object _hasSpeedObject; //cannot serialize an interface
    [SerializeField] Rigidbody _rigidbody;
    float _h;
    float _v;
    public IHasSpeed HasSpeed
    {
        get;
        private set;
    }

    //should be called everyframe in fixed update
    public void InputControl(float h, float v)
    {
        _h = h;
        _v = v;
    }

    private void FixedUpdate()
    {
        if (_h == 0 && _v == 0) return;
        // 以下、キャラクターの移動処理
        Vector3 Velocity = new Vector3(0, 0, _v);        // 上下のキー入力からZ軸方向の移動量を取得

        Velocity = transform.TransformDirection(Velocity);
        //以下のvの閾値は、Mecanim側のトランジションと一緒に調整する
        if (_v > 0.1)
        {
            Velocity *= HasSpeed.ForwardSpeed;       // 移動速度を掛ける
        }
        else if (_v < -0.1)
        {
            Velocity *= HasSpeed.BackwardSpeed;  // 移動速度を掛ける
        }


        // 上下のキー入力でキャラクターを移動させる
        _rigidbody.MovePosition(transform.position + Velocity * Time.fixedDeltaTime);

        // 左右のキー入力でキャラクタをY軸で旋回させる
        transform.Rotate(0, _h * HasSpeed.RotateSpeed, 0);
    }

    private void Awake()
    {
        HasSpeed = (IHasSpeed)_hasSpeedObject;
    }
}

