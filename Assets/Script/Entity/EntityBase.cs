using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
      Rigidbody _rb;

    public float _speed;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 dir)
    {
        Vector3 dirrection = dir * _speed;
        dirrection.y = _rb.velocity.y;
        _rb.velocity = dirrection;
    }

    public void LookDir(Vector3 dir)
    {
        if (dir == Vector3.zero) return; 
        dir.y = 0;
        transform.forward = dir;
    }

    public Vector3 GetForward => transform.forward;

    public float GetVelocity => _rb.velocity.magnitude;
}
