using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoadPoint
{
    private Transform _origin;
    private Transform _target;
    private LayerMask _pointsLayer;
    private LayerMask _ObstacleLayers;
    private float _radius;
    private float _angle;
    private Collider[] points;
    [SerializeField] private Collider[] pointsTraveled;
    private int _index;
    
    
    
    
    private Collider _collider;
    
    
    
    
    [SerializeField] private Vector3 dir = Vector3.zero;

    public RoadPoint(Transform origin, Transform target, LayerMask pointsLayer, LayerMask ObstacleLayers,
        int pointsLenght, float radius, float angle, int maxPointUsed)
    {
        _origin = origin;
        _target = target;
        _pointsLayer = pointsLayer;
        _ObstacleLayers = ObstacleLayers;
        _radius = radius;
        _angle = angle;
        points = new Collider[pointsLenght];
        pointsTraveled = new Collider[maxPointUsed];
    }

    public Vector3 GetDir()
    {
        int countObstacle = Physics.OverlapSphereNonAlloc(_origin.position, _radius, points, _pointsLayer);
        
        Vector3 dirToPoint = dir;

        int detectedPoint = 0;
        bool isFindPoint = false;

        for (int i = 0; i < countObstacle; i++)
        {
         
            Collider col = points[i];

            Vector3 diffPoint = col.transform.position - _origin.position;

            float angleToPoint = Vector3.Angle(_origin.forward, diffPoint);

            float distance = Vector3.Distance(_target.position, col.transform.position);
            
            float diffTargetAndOrigin;

            if (_collider != null)
            {
                diffTargetAndOrigin = Vector3.Distance(_target.position, _collider.transform.position);
            }
            else
            {
                diffTargetAndOrigin = Vector3.Distance(_target.position, _origin.transform.position);
            }


            if (angleToPoint > _angle / 2) continue;

            if (col.GetComponent<Node>().walkable)
            {
                if (CheckView(col.transform,_ObstacleLayers) && !CheckView(_target.transform,_ObstacleLayers))
                {
                    Collider before = _collider;
                    if (distance < diffTargetAndOrigin || !isFindPoint )
                    {
                        dir = (col.transform.position - _origin.transform.position).normalized;
                        _collider = col;
                        isFindPoint = true;
                        col.GetComponent<Node>().walkable = false;
                    }else if (distance < diffTargetAndOrigin && _collider != col)
                    {
                        dir = (col.transform.position - _origin.transform.position).normalized;
                        _collider = col;
                        isFindPoint = true;
                        col.GetComponent<Node>().walkable = false;
                    }
                }
               
            }

           

            dirToPoint = dir;
        }
        
        dirToPoint.y = 0;
        return dirToPoint.normalized;
    }

    private bool CheckView(Transform target, LayerMask mask)
    {
        Vector3 diff = (target.position - _origin.position);
        Vector3 dirToTarget = diff.normalized;
        float distTarget = diff.magnitude;

        RaycastHit hit;

        return !Physics.Raycast(_origin.position, dirToTarget, out hit, distTarget, mask);
    }

    private void UsePoint(Collider col)
    {
        bool isHasColl = false;
        
        for (int j = 0; j < pointsTraveled.Length; j++)
        {
            if (col == pointsTraveled[j])
            {
                isHasColl = true;
            }
            if (!isHasColl && j == pointsTraveled.Length - 1)
            {
                if (_index < pointsTraveled.Length - 1) _index++;
                else _index =0;
                
                pointsTraveled[_index] = col;
                
                isHasColl = true;
            }  
        }

    }
    private bool isUsePoint(Collider col)
    {
        bool isHasColl = false;
        
        for (int j = 0; j < pointsTraveled.Length; j++)
        {
            if (col == pointsTraveled[j])
            {
                isHasColl = true;
            }
           
        }

        return isHasColl;
    }
}
