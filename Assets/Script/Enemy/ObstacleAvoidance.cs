using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : ISteering
{
    private Transform _origin;
    private LayerMask _layerMask;
    private float _radius;
    private float _angle;
    private Collider[] _colliders;
    
    public ObstacleAvoidance(Transform origin,LayerMask layerMask, int ObstacleLenght, float radius, float angle)
    {
        _origin = origin;
        _layerMask = layerMask;
        _radius = radius;
        _angle = angle;
        _colliders = new Collider[ObstacleLenght];
    }

    public Vector3 GetDir()
    {
        int countObstacle = Physics.OverlapSphereNonAlloc(_origin.position, _radius,_colliders, _layerMask);
        Vector3 dirToAvoid = Vector3.zero;
        int detectedObs = 0;
        for (int i = 0; i < countObstacle; i++)
        {
            Collider currObs = _colliders[i];
            Vector3 closePoint = currObs.ClosestPointOnBounds(_origin.position);
            Vector3 diffPoint = closePoint - _origin.position;
            float angleToObs =  Vector3.Angle(_origin.forward, diffPoint);

            if (angleToObs > _angle/2) continue;
            float dist = diffPoint.magnitude;
            detectedObs++;
            dirToAvoid += -(diffPoint).normalized * (_radius - dist);
        }

        if (countObstacle > 0)
        {
            dirToAvoid /= countObstacle;

        }
        return dirToAvoid.normalized;
        
    }
}
