using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    EntityBase _model;
    FSM<PlayerStateEnum> _fsm;
    List<PlayerStateBase<PlayerStateEnum>> _states;
    void InitializedFSM()
    {
        _fsm = new FSM<PlayerStateEnum>();
        _states = new List<PlayerStateBase<PlayerStateEnum>>();
        var idle = new PlayerStateIdle<PlayerStateEnum>(PlayerStateEnum.Walking);
        var move = new PlayerStateMove<PlayerStateEnum>(PlayerStateEnum.Idle);

        _states.Add(idle);
        _states.Add(move);

        idle.AddTransition(PlayerStateEnum.Walking, move);

        move.AddTransition(PlayerStateEnum.Idle, idle);


        for (int i = 0; i < _states.Count; i++)
        {
            _states[i].InitializedState(_model, _fsm);
        }
        _states = null;

        _fsm.SetInit(idle);
    }
    private void Awake()
    {
        _model = GetComponent<EntityBase>();
        InitializedFSM();
    }
    private void Update()
    {
        _fsm.OnUpdate();
    }
}
