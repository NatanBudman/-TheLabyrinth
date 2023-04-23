using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeyStateBase<T> : State<T>
{
    protected EntityBase _model;
    protected FSM<T> _fsm;
    protected EnemyController _controller;
    protected Patrol _patrol;
    protected Seek _seek;
    public void InitializedState(EntityBase model, FSM<T> fsm, EnemyController controller, Patrol patrol, Seek seek)
    {
        _controller = controller;
        _model = model;
        _fsm = fsm;
        _patrol = patrol;
        _seek = seek;
    }
}