using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    EntityBase _model;
    FSM<EnemyStateEnum> _fsm;
    ITreeNode _root;
    private void Awake()
    {
        _model = GetComponent<EntityBase>();
        IntializedFSM();
        InitializedTree();
    }

    public void IntializedFSM()
    {
        var list = new List<EnemeyStateBase<EnemyStateEnum>>();
        _fsm = new FSM<EnemyStateEnum>();

        var idle = new EnemyIdleState<EnemyStateEnum>();
        var chase = new EnemyChaseState<EnemyStateEnum>();
        var patrol = new EnemyPatrolState<EnemyStateEnum>();
        var attack = new EnemyAttackState<EnemyStateEnum>();

        list.Add(idle);
        list.Add(chase);
        list.Add(patrol);
        list.Add(attack);

        for (int i = 0; i < list.Count; i++)
        {
            list[i].InitializedState(_model, _fsm);
        }

        idle.AddTransition(EnemyStateEnum.Chase, chase);
        idle.AddTransition(EnemyStateEnum.Patrol, patrol);
        idle.AddTransition(EnemyStateEnum.Attack, attack);

        chase.AddTransition(EnemyStateEnum.Patrol, patrol);
        chase.AddTransition(EnemyStateEnum.Attack, attack);

        patrol.AddTransition(EnemyStateEnum.Chase, chase);
        patrol.AddTransition(EnemyStateEnum.Idle, idle);

        _fsm.SetInit(idle);
    }

    public void InitializedTree()
    {
        //actions
        var idle = new TreeAction(ActionIdle);
        var patrol = new TreeAction(ActionPatrol);
        var chase = new TreeAction(ActionChase);
        var attack = new TreeAction(ActionAttack);

        //questions
        var isTimeOver = new TreeQuestion(IsTimeOver, patrol, idle);
        var isTouching = new TreeQuestion(IsTouching, attack, chase);
        var sawPlayer = new TreeQuestion(SawPlayer, isTouching, isTimeOver);          
        var isAlive = new TreeQuestion(IsAlive, sawPlayer, null);

        _root = isAlive;
    }

    bool IsTouching()
    {
        return _model.IsTouchPlayer;
    }
    bool SawPlayer()
    {
        return _model.IsTouchPlayer;
    }
    bool IsTimeOver()
    {
        return _model.CurrentTimer < 0;
    }
    bool IsAlive()
    {
        return true;
    }
    void ActionIdle()
    {
        _fsm.Transitions(EnemyStateEnum.Idle);
    }
    void ActionPatrol()
    {
        _fsm.Transitions(EnemyStateEnum.Patrol);
    }
    void ActionChase()
    {
        _fsm.Transitions(EnemyStateEnum.Chase);
    }
    void ActionAttack()
    {
        _fsm.Transitions(EnemyStateEnum.Attack);
    }
    private void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();
    }
}
