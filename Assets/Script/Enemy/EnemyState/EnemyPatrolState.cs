using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState<T> : EnemeyStateBase<T>
{
    AStar<T> aStar;

    public override void Awake()
    {
        base.Awake();
        if (_obstacleAvoidance == null)
            _obstacleAvoidance = new ObstacleAvoidance(_model.transform, _controller.enemyObstacle, 15, _controller.obstacleDetectionRadius, _controller.obstacleDetectionAngle);
        _controller.InicializateSeek();
        aStar = new AStar<T>();
       _agentController.newRoute();


    }
    public override void Execute()
    {
        base.Execute();

        if (Vector2.Distance(_agentController.crash.transform.position, _agentController.goalNode.transform.position) < 3)
        {
            //  Debug.Log("Cambio de ruta");
            _agentController.newRoute();

        }

        if (_model.CurrentTimer >= 0)
        {
            Debug.Log("Patrol");
            _model.RunTimer();
            _model.Run();
            _model.RotateTowardsMovement();

        }

        Vector3 dirAvoidance = _obstacleAvoidance.GetDir();

        _model.Move(dirAvoidance);
        _model.LookDir(dirAvoidance);

    }

   
}
