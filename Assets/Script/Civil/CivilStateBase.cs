using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilStateBase<T> : State<T>
{
    protected FlockingPrueba _model;
    protected FSM<T> _fsm;
    protected FlockingController _controller;
    protected FlockingManager _manager;
    public void InitializedState(FlockingPrueba model, FSM<T> fsm, FlockingController controller, FlockingManager manager)
    {
        _controller = controller;
        _model = model;
        _fsm = fsm;
        _manager = manager;
    }
}
