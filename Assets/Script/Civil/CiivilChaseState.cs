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
        Vector3 dir = (_manager.FlockingDir() + dirAvoidance * 0.8f).normalized;
        Debug.Log("CivilSigue");
        _model.Move(dir);
        _model.LookDir(dir);
       

        
        
    }
   
}
