using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeyStateBase<T> : State<T>
{
    protected EntityBase _model;
    protected FSM<T> _fsm;
    protected EnemyController _controller;
    protected Patrol _patrol;
    protected Persuit _seek;
    protected ObstacleAvoidance _obstacleAvoidance;
    protected AgentController _agentController;
    public void InitializedState(EntityBase model, FSM<T> fsm, EnemyController controller, Patrol patrol, Persuit persuit, ObstacleAvoidance obstacleAvoidance, AgentController agentController)
    {
        _controller = controller;
        _model = model;
        _fsm = fsm;
        _patrol = patrol;
        _seek = persuit;
        _obstacleAvoidance = obstacleAvoidance;
        _agentController = agentController;
    }
}