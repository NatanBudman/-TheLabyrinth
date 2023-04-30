using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
    public Rigidbody _rb;

    PlayerModel _lastPlayerTouch;

    public float _speed;
    public float _speedRotate = 100;

    public float jumpforce = 10f;

    float _timer;
    public float maxTime;


    bool _touchPlayer;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public  void Move(Vector3 dir)
    {
        Vector3 dirrection = dir * _speed;
        dirrection.y = _rb.velocity.y;
        _rb.velocity = dirrection;
        //agregar Obstacle avoidance
    }

   

    public float GetRandomTime()
    {
        return UnityEngine.Random.Range(0, maxTime);
    }
    public void RunTimer()
    {
        _timer += Time.deltaTime;
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

    public void Jump ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);

        }

    }
    public void Attack(PlayerModel player)
    {
        if (player != null)
            Destroy(player.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerModel player = collision.gameObject.GetComponent<PlayerModel>();
        if (player != null)
        {
            _touchPlayer = true;
            _lastPlayerTouch = player;
        }
        
    }
    public bool IsTouchPlayer => _touchPlayer;

    public Vector3 GetForward => transform.forward;

    public PlayerModel LastPlayerTouch => _lastPlayerTouch;


    public float GetVelocity => _rb.velocity.magnitude;

     public float CurrentTimer
     {
        set
        {
            _timer = value;
        }
        get
        {
            return _timer;
        }
     }
}
