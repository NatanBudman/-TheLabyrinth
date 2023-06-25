using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aligment : MonoBehaviour,IFlocking
{
    public float Multiplier;
    public Vector3 GetDir(List<IBoid> boids, IBoid selft)
    {
        Vector3 front = Vector3.zero;
        for (int i = 0; i < boids.Count; i++)
        {
            front += boids[i].Front;
        }
        return front.normalized * Multiplier;
    }
}
