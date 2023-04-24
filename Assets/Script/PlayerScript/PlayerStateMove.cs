using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMove<T> : PlayerStateBase<T>
{
    T _inputIdle;
    public PlayerStateMove(T inputIdle)
    {
        _inputIdle = inputIdle;
    }
    public override void Awake()
    {
        base.Awake();
      
    }
    public override void Execute()
    {
        base.Execute();
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        if (h == 0 && v == 0)
        {
            _fsm.Transitions(_inputIdle);
            return;
        }

        Vector3 dir = new Vector3(h, 0, v).normalized;

        _model.PlayerMove();
        _model.MouseRotation();
    }
    public override void Sleep()
    {
        base.Sleep();
        _model.PlayerMove();
        _model.MouseRotation();
    }
}
