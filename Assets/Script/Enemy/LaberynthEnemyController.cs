using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaberynthEnemyController : MonoBehaviour
{
    [SerializeField]private EntityBase _entityBase;
    public float time;
    public EntityBase _target;
    private ISteering _steering;

    [Header("Obstacles Evade")]
    public LayerMask _layerMask;
    public float _radius;
    public float _angle;
    public int mutliplier;

    private void InicializateSeek()
    {
        var seek = new Seek(transform, _target.transform);
        var flee = new Flee(transform, _target.transform);
        var persuit = new Persuit(transform, _target,time);
        var evade = new Evade(transform, _target,time);
        var obsAvoid = new ObstacleAvoidance(transform,_layerMask, 20,_radius,_angle);
        _steering = obsAvoid;
    }

    private void Awake()
    {
        InicializateSeek();
    }

    private void Update()
    {
        Vector3 obstacleAvoid = _steering.GetDir();
        Vector3 dir = (_target.transform.position - transform.position).normalized;
        Vector3 direction = (dir + obstacleAvoid * mutliplier).normalized;
        
        _entityBase._rb.velocity = transform.forward * _entityBase._speed;
       _entityBase.LookRotate(direction);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,_radius);
        Gizmos.color = Color.red;
        
        Gizmos.DrawRay(transform.position,Quaternion.Euler(0,_angle / 2,0)*transform.forward * _radius);
        Gizmos.DrawRay(transform.position,Quaternion.Euler(0,-_angle / 2,0)*transform.forward * _radius);
    }
}
