using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState<T> : EnemeyStateBase<T>
{

    public override void Awake()
    {
        base.Awake();
    
    }
    public override void Execute()
    {
        Debug.Log("idle");
        base.Execute();
        if (_model.CurrentTimer >= 0)
        {
            _model.RunTimer();
        }
        if (_model.CurrentTimer > 10)
        {
            _model.CurrentTimer = 0;

        }
    }
    public override void Sleep()
    {
        base.Sleep();
       
    }
}