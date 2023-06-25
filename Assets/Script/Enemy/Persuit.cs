using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persuit : ISteering
{
    private Transform _origin;
    private PlayerModel _target;
    private float _time;
    public Persuit(Transform origin,PlayerModel target, float time)
    {
        _origin = origin;
        _target = target;
        _time = time;
    }
    public virtual Vector3 GetDir()
    {
        float distance = Vector3.Distance(_origin.position, _target.transform.position);
        
        Vector3 point = _target.transform.position + _target.GetFarword * 
            Mathf.Clamp(_target.GetVelocity * _time,0,distance);
        return (point - _origin.position).normalized;
    }
}
