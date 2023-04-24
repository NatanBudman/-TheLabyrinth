using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAvoidanceState<T> : EnemeyStateBase<T>
{

    public override void Awake()
    {
        base.Awake();

    }
    public override void Execute()
    {
        base.Execute();
        _model.Move(_obstacleAvoidance.GetDir());
        _model.LookRotate(_obstacleAvoidance.GetDir());
        Debug.Log("avoid");

    }

}