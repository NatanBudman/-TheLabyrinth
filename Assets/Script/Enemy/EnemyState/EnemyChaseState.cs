using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState<T> : EnemeyStateBase<T>
{
    Persuit persuit;
    EnemyController enemyController;
    ISteering _steering;
    EntityBase _entity;
    AStar<T> aStar;
    private bool isGenerateNewRoute;
    public override void Awake()
    {
        base.Awake();
        isGenerateNewRoute = false;
      if (_obstacleAvoidance == null)
            _obstacleAvoidance = new ObstacleAvoidance(_model.transform, _controller.layerObstacle, 15, _controller.obstacleDetectionRadius, _controller.obstacleDetectionAngle);
      if (persuit == null)
          persuit = new Persuit(_model.transform, _controller.target.GetComponent<PlayerModel>(), 3);
      if (aStar == null)
          aStar = new AStar<T>();   

        _steering = persuit;

    }
    public override void Execute()
    {
        base.Execute();
        if (_agentController.InView(_agentController.IA.transform.position,
                _agentController._player.transform.position))
        {
            Vector3 dirAvoidance = _obstacleAvoidance.GetDir();
            Vector3 dir = (_steering.GetDir() + dirAvoidance * 0.8f).normalized;

            _model.Move(dir);
            _model.LookDir(dir);
            isGenerateNewRoute = false;
        }
        else
        {
            Debug.Log(_agentController.IA.name);
            if (!isGenerateNewRoute)
            {
                newRoute();
                isGenerateNewRoute = true;
            }
            Vector3 dir = Vector3.zero;
            if (_model.CurrentTimer >= 0)
            {
                _model.RunTimer();
                dir += _controller.Run();
                _model.RotateTowardsMovement();

            }

            Vector3 dirAvoidance = (dir + _obstacleAvoidance.GetDir() * 1).normalized;

            _model.Move(dirAvoidance);
            _model.LookDir(dirAvoidance);
        }



    }
    
    public void AStarPlusRun()
    {
        var start = _agentController.startNode;
        if (start == null) return;
        var path = _agentController._ast.Run(start, _agentController.Satisfies, _agentController.GetConections, _agentController.GetCost, _agentController.Heuristic, 500);
        // mover el run al estado
        path = _agentController._ast.CleanPath(path, _agentController.InView);
        _agentController.IA.SetWayPoints(path);
    }

    private void SetNodes()
    {
        _agentController.startNode = _agentController.GetPosNode(_agentController.IA.transform.position);
        _agentController.goalNode = _agentController.GetNodePlayerTransform(_agentController._player.transform.position,15);
    }



    private void newRoute()
    {
        SetNodes();
        AStarPlusRun();
    }
     
}
