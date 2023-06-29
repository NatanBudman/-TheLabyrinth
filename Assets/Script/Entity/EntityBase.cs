using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour, IPoints
{
    public Rigidbody _rb;

    PlayerModel _lastPlayerTouch;

    public float _speed;
    public float _speedRotate = 100;

    public float jumpforce = 10f;
    public float rotSpeed = 5;

    float _timer;
    public float maxTime;

    bool _touchPlayer;

    public List<Vector3> waypoints;
    public float speedP = 2;
    public float speedRot = 10;
    public bool readyToMove;
    int _nextPoint = 0;

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

    public void MoveP(Vector3 dir)
    {
        dir.y = 0;
        transform.position += Time.deltaTime * dir * speedP; ;
        transform.forward = Vector3.Lerp(transform.forward, dir, speedRot * Time.deltaTime);
    }
    public void RotateTowardsMovement()
    {
        Vector3 forward = GetForward;
        forward.y = 0f; 

        if (forward != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(forward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _speedRotate * Time.deltaTime);
        }
    }
    public void SetWayPoints(List<diffNode> newPoints)
    {
        Debug.Log("seteados");
        var list = new List<Vector3>();
        for (int i = 0; i < newPoints.Count; i++)
        {
            list.Add(newPoints[i].transform.position);
        }
        SetWayPoints(list);
    }
    public void SetWayPoints(List<Vector3> newPoints)
    {
        _nextPoint = 0;
        if (newPoints.Count == 0) return;
        waypoints = newPoints;
        var pos = waypoints[_nextPoint];
        pos.y = transform.position.y;
        transform.position = pos;
        readyToMove = true;
    }
    public void Run()
    {
        Debug.Log("RunASTAR");
        var point = waypoints[_nextPoint];

        var posPoint = point;
        posPoint.y = transform.position.y;
        Vector3 dir = posPoint - transform.position;
        if (dir.magnitude < 0.2f)
        {
            if (_nextPoint + 1 < waypoints.Count)
                _nextPoint++;
            else
            {
                readyToMove = false;

                return;
            }
        }
        MoveP(dir.normalized);
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
        transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * rotSpeed);
    }

    public Transform getPosition => transform;

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
            player.gameObject.SetActive(false);
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
