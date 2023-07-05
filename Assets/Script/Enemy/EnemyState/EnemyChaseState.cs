using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState<T> : EnemeyStateBase<T>
{
    Persuit persuit;
    EnemyController enemyController;
    ISteering _steering;
    EntityBase _entity;
   
    public override void Awake()
    {
        base.Awake();
        
      if (_obstacleAvoidance == null)
            _obstacleAvoidance = new ObstacleAvoidance(_model.transform, _controller.layerObstacle, 15, _controller.obstacleDetectionRadius, _controller.obstacleDetectionAngle);
      if (persuit == null)
          persuit = new Persuit(_model.transform, _controller.target.GetComponent<PlayerModel>(), 3);
        

        _steering = persuit;

    }
    public override void Execute()
    {
        base.Execute();
        
            Vector3 dirAvoidance = _obstacleAvoidance.GetDir();
            Vector3 dir = (_steering.GetDir() + dirAvoidance * 0.8f).normalized;

            _model.Move(dir);
            _model.LookDir(dir);
      
    }
     
}
