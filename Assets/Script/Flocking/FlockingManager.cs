using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour
{
    public int maxBoids;
    public LayerMask maskBoids;
    private IBoid _selft;
    private IFlocking[] _flockings;
    private Collider[] _collider;
    private List<IBoid> _boids;
    private void Awake()
    {
       _flockings = GetComponents<IFlocking>();
       _selft = GetComponent<IBoid>();
       _boids = new List<IBoid>();
       _collider = new Collider[maxBoids];
    }

    private void Update()
    {
        StarFlocking();
    }
    public void StarFlocking()
    {
        _boids.Clear();
        
        int count= Physics.OverlapSphereNonAlloc(_selft.Position, _selft.Radius,_collider);

        for (int i = 0; i < count; i++)
        {
            var curr = _collider[i];
            IBoid boid = curr.GetComponent<IBoid>();
            if (boid == null) continue;
            
            _boids.Add(boid);
        }

        Vector3 dir = Vector3.zero;
        for (int i = 0; i < _flockings.Length; i++)
        {
            var currFlocking = _flockings[i];
            dir += currFlocking.GetDir(_boids, _selft);
        }

        dir = dir.normalized;
        _selft.Move(dir);
        _selft.LookDir(dir);
        
    }
    public Vector3 FlockingDir()
    {
        _boids.Clear();
        
        int count= Physics.OverlapSphereNonAlloc(_selft.Position, _selft.Radius,_collider);

        for (int i = 0; i < count; i++)
        {
            var curr = _collider[i];
            IBoid boid = curr.GetComponent<IBoid>();
            if (boid == null) continue;
            
            _boids.Add(boid);
        }

        Vector3 dir = Vector3.zero;
        for (int i = 0; i < _flockings.Length; i++)
        {
            var currFlocking = _flockings[i];
            dir += currFlocking.GetDir(_boids, _selft);
        }

        return dir.normalized;
        
    }
}
