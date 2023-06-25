using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState<T> : EnemeyStateBase<T>
{
    ObstacleAvoidance _obstacleAvoidance;
    Persuit persuit;
    EnemyController enemyController;
    ISteering _steering;

    public override void Awake()
    {
        base.Awake();
        _obstacleAvoidance = new ObstacleAvoidance(_controller.target, _controller.layerObstacle, 5, _controller.obstacleDetectionRadius, _controller.obstacleDetectionAngle);
        persuit = new Persuit(_model.transform, _controller.target.GetComponent<PlayerModel>(),1);
        enemyController = new EnemyController();
        _steering = persuit;
    }
    public override void Execute()
    {
        base.Execute();
        if (seeObstacle())
        {
              Debug.Log(_obstacleAvoidance);

            Vector3 pepe = (_obstacleAvoidance.GetDir() + persuit.GetDir() * 2f).normalized;

            _model.Move(pepe);
            _model.LookRotate(pepe);

            Debug.Log("obstaculovisto");
          


        }
        else
        {
            Vector3 dirAvoidance = _obstacleAvoidance.GetDir();
            Vector3 dir = (_steering.GetDir() + dirAvoidance * 1).normalized;

            _model.Move(dir);
            _model.LookDir(dir);

        }
        

        //Debug.Log("Chase");

    }
    bool seeObstacle()
    {
        bool isSeePlayer = false;

        Vector3 diffPoint = _controller.target.transform.position - _model.transform.position;

        float distDetect = Vector3.Distance(_controller.target.transform.position, _model.transform.position);

        float angleToPoint = Vector3.Angle(_model.transform.forward, diffPoint);
        if (angleToPoint < _controller.obstacleDetectionAngle / 2 && distDetect < _controller.obstacleDetectionRadius)
        {
            Vector3 diff = (_controller.target.position - _model.transform.position);
            Vector3 dirToTarget = diff.normalized;
            float distTarget = diff.magnitude;

            RaycastHit hit;

            isSeePlayer = !Physics.Raycast(_model.transform.position, dirToTarget, out hit, distTarget, _controller.layerObstacle);
        }


        return isSeePlayer;
    }
}