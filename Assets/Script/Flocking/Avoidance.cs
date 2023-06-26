using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avoidance : MonoBehaviour,IFlocking
{
    public float multiplier;
    public float personalRange;
    public Vector3 GetDir(List<IBoid> boids, IBoid selft)
    {
        Vector3 dir = Vector3.zero;
        ;
        for (int i = 0; i < boids.Count; i++)
        {
            Vector3 diff = selft.Position - boids[i].Position;
            float distance = diff.magnitude;
            if (distance > personalRange) continue;
            dir = diff.normalized * (personalRange - distance);

        }
        return dir.normalized * multiplier;
    }
}
