using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : MonoBehaviour,IFlocking
{
    public float predatorRange;
    public float multiplaire;

    private Collider[] colliders;
    public int MaxPredators;
    public LayerMask predatorMask;

    private void Awake()
    {
        colliders = new Collider[MaxPredators];
    }

    public Vector3 GetDir(List<IBoid> boids, IBoid selft)
        {
            Vector3 dir = Vector3.zero;
            int count = Physics.OverlapSphereNonAlloc(selft.Position, predatorRange, colliders, predatorMask);
            for (int i = 0; i < count; i++)
            {
                Vector3 diff = selft.Position - colliders[i].transform.position;
                dir += diff.normalized * (predatorRange - diff.magnitude);
            }
            return dir.normalized * multiplaire;
        }
}
