using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : MonoBehaviour,IFlocking
{
    public float Multiplier;
    public Transform target;

    public Vector3 GetDir(List<IBoid> boids, IBoid selft)
    {
        return (target.position - selft.Position).normalized * Multiplier;
    }
}
