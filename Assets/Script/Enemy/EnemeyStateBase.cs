using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeyStateBase<T> : State<T>
{
    protected EntityBase _model;
    protected FSM<T> _fsm;
    public void InitializedState(EntityBase model, FSM<T> fsm)
    {
        _model = model;
        _fsm = fsm;
    }
}