using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState<T> : EnemeyStateBase<T>
{
    public override void Awake()
    {
        base.Awake();
        _controller.InicializateSeek();


    }
    public override void Execute()
    {
        base.Execute();
       
        _model.Move(_patrol.Patrullaje());
      //  _model.LookRotate(_patrol.Patrullaje());
        if (_model.CurrentTimer >= 0)
        {
            _model.RunTimer();
        }
        
    }
}
