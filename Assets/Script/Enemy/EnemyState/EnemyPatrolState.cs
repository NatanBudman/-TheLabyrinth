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
        if (aStar == null)
            aStar = new AStar<T>();
            
           newRoute();


    }
    public override void Execute()
    {
        base.Execute();

        if (Vector2.Distance(_agentController.IA.transform.position, _agentController.goalNode.transform.position) < 3)
        {
            newRoute();
        }

        Vector3 dir = Vector3.zero;
        if (_model.CurrentTimer >= 0)
        {
            Debug.Log("Patrol");
            _model.RunTimer();
            dir += _controller.Run();
            _model.RotateTowardsMovement();

        }

        Vector3 dirAvoidance = (dir + _obstacleAvoidance.GetDir() * 1).normalized;

        _model.Move(dirAvoidance);
        _model.LookDir(dirAvoidance);

    }
    public void AStarPlusRun()
    {
        var start = _agentController.startNode;
        if (start == null) return;
        var path = _agentController._ast.Run(start, _agentController.Satisfies, _agentController.GetConections, _agentController.GetCost, _agentController.Heuristic, 500);
        // mover el run al estado
        path = _agentController._ast.CleanPath(path, _agentController.InView);
        _agentController.IA.SetWayPoints(path);
        _agentController.box.SetWayPoints(path);
    }
   

    private void newRoute()
    {
        _agentController.buildingDictionary();
        _agentController.goalNode = RandomSystem.Roulette(_agentController.dicNodos);
        AStarPlusRun();
    }

}
