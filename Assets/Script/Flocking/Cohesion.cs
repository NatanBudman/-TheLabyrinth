using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cohesion : MonoBehaviour,IFlocking
{
    public float Multiplier;
    public Vector3 GetDir(List<IBoid> boids, IBoid selft)
    {
        Vector3 center = Vector3.zero;
        Vector3 dir = Vector3.zero;
        
        for (int i = 0; i < boids.Count; i++)
        {
            center += boids[i].Position;
        }

        if (boids.Count > 0)
        {
            center /= boids.Count;
             dir = center - selft.Position;
        }
        return dir.normalized * Multiplier;
        
    }
}
