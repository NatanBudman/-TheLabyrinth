using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    Rigidbody _rb;
    public Animator anim;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        var vel = _rb.velocity;
        vel.y = 0;
        anim.SetFloat("Velocity", vel.magnitude);
    }
}
