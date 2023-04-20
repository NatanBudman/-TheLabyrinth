using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public float speed;
    Rigidbody _rb;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public void Move(Vector3 dir)
    {
        Vector3 dirSpeed = dir * speed;
        dirSpeed.y = _rb.velocity.y;
        _rb.velocity = dirSpeed;
    }
    public void LookDir(Vector3 dir)
    {
        if (dir == Vector3.zero) return;
        transform.forward = dir;
    }
}
