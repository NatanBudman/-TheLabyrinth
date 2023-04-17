using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
    public Rigidbody _rb;

    public float _speed;
    public float _speedRotate;

    private void Awake()
    {
        _rb.GetComponent<Rigidbody>();
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
    
        
    public void LookRotate(Vector3 dir)
    {

        Quaternion targetRotation = Quaternion.LookRotation(dir);

        transform.transform.rotation = Quaternion.RotateTowards(transform.transform.rotation,
            targetRotation, _speedRotate * Time.deltaTime);
    }

    public Vector3 GetForward => transform.forward;

    public float GetVelocity => _rb.velocity.magnitude;
}
