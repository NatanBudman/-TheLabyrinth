using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState<T> : EnemeyStateBase<T>
{

    public override void Awake()
    {
        base.Awake();

    }
    public override void Execute()
    {
        base.Execute();              
        _model.Move(_seek.GetDir());
        Debug.Log("Chase");

    }

}