using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CiivilChaseState<T> : CivilStateBase<T>
{
    public override void Awake()
    {
        base.Awake();
        if (_obstacleAvoidance == null)
            _obstacleAvoidance = new ObstacleAvoidance(_model.transform, _controller.layerObstacle, 15, _controller.obstacleDetectionRadius, _controller.obstacleDetectionAngle);
    }
    public override void Execute()
    {
        base.Execute();
        Vector3 dirAvoidance = _obstacleAvoidance.GetDir();

        Debug.Log("CivilSigue");
        _model.Move(_manager.FlockingDir()+ dirAvoidance);
        _model.LookDir(_manager.FlockingDir()+ dirAvoidance);
       

        
        
    }
   
}
