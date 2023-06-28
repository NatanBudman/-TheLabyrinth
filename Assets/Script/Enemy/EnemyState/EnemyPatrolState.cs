using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState<T> : EnemeyStateBase<T>
{
    AStar<T> aStar;
    public override void Awake()
    {
        base.Awake();
        _controller.InicializateSeek();
        aStar = new AStar<T>();

    }
    public override void Execute()
    {
        base.Execute();

        if (_model.CurrentTimer >= 0)
        {
            Debug.Log("Patrol");
            _model.RunTimer();
            _model.Run();
            _model.RotateTowardsMovement();

        }

    }
}
