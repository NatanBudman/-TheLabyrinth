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
    private void Start()
    {
       _flockings = GetComponents<IFlocking>();
       _selft = this.GetComponent<IBoid>();
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
            if (boid == null || boid == _selft) continue;
            
            _boids.Add(boid);
        }

        Vector3 dir = Vector3.zero;
        for (int i = 0; i < _flockings.Length; i++)
        {
            var currFlocking = _flockings[i];
            dir += currFlocking.GetDir(_boids, _selft);
        }

        _selft.LookDir(dir.normalized);
        _selft.Move(_selft.Front);
        
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
