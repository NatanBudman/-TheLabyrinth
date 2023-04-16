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

    private void InicializateSeek()
    {
        var seek = new Seek(transform, _target.transform);
        var flee = new Flee(transform, _target.transform);
        var persuit = new Persuit(transform, _target,time);
        var evade = new Evade(transform, _target,time);
        _steering = seek;
    }

    private void Awake()
    {
        InicializateSeek();
    }

    private void Update()
    {
        Vector3 dir = _steering.GetDir();
        _entityBase.Move(dir);
        _entityBase.LookDir(dir);
    }
}
