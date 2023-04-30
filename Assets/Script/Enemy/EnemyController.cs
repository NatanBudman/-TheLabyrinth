using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    EntityBase _model;
    FSM<EnemyStateEnum> _fsm;
     Patrol _patrol;
    EnemyController _controller;
    Seek _seek;
    ObstacleAvoidance _obstacleAvoidance;
    ITreeNode _root;
    [SerializeField]Transform target;
    
    private ISteering _steering;

    [Header("Path")]

    public LayerMask _layerMaskPath;
 
    [SerializeField] Transform[] patrolPoints;

    [Header("Detection Parameters")]
    [SerializeField] private LayerMask layerObstacle;
    public float detectionRadius;
    public float detectionAngle;
    private void Awake()
    {
        InicializateSeek();
        _obstacleAvoidance = new ObstacleAvoidance(target, layerObstacle, 20, detectionRadius, detectionAngle);
        _seek = new Seek(transform, target); 
        _model = GetComponent<EntityBase>();
        
        _controller = GetComponent<EnemyController>();
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
        var avoidance = new EnemyAvoidanceState<EnemyStateEnum>();

        list.Add(idle);
        list.Add(chase);
        list.Add(patrol);
        list.Add(attack);
        list.Add(avoidance);


        for (int i = 0; i < list.Count; i++)
        {
            list[i].InitializedState(_model, _fsm, _controller, _patrol, _seek, _obstacleAvoidance);
        }

        idle.AddTransition(EnemyStateEnum.Chase, chase);
        idle.AddTransition(EnemyStateEnum.Patrol, patrol);
        idle.AddTransition(EnemyStateEnum.Attack, attack);

        chase.AddTransition(EnemyStateEnum.Patrol, patrol);
        chase.AddTransition(EnemyStateEnum.Attack, attack);
        chase.AddTransition(EnemyStateEnum.Avoidance, avoidance);
        chase.AddTransition(EnemyStateEnum.Idle, idle);

        patrol.AddTransition(EnemyStateEnum.Chase, chase);
        patrol.AddTransition(EnemyStateEnum.Idle, idle);
        patrol.AddTransition(EnemyStateEnum.Avoidance, avoidance);


        _fsm.SetInit(idle);
    }

    public void InitializedTree()
    {
        //actions
        var idle = new TreeAction(ActionIdle);
        var patrol = new TreeAction(ActionPatrol);
        var chase = new TreeAction(ActionChase);
        var attack = new TreeAction(ActionAttack);
        var avoidance = new TreeAction(ActionAvoidance);

        //questions
        var isObstacle = new TreeQuestion(IsObstacle, avoidance, chase);
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
    bool IsObstacle()
    {
        bool isSeePlayer = false;

       
        
            Vector3 diff = (target.position - transform.position);
            Vector3 dirToTarget = diff.normalized;
            float distTarget = diff.magnitude;

            RaycastHit hit;

            isSeePlayer = !Physics.Raycast(transform.position, dirToTarget, out hit, distTarget, layerObstacle);
        


        return isSeePlayer;

    }

    bool SawPlayer()
    {
        bool isSeePlayer = false ;

        Vector3 diffPoint = target.transform.position - transform.position;

        float angleToPoint = Vector3.Angle(transform.forward, diffPoint);
        if(angleToPoint < detectionAngle/2)
        {
            Vector3 diff = (target.position - transform.position);
            Vector3 dirToTarget = diff.normalized;
            float distTarget = diff.magnitude;

            RaycastHit hit;

           isSeePlayer= !Physics.Raycast(transform.position, dirToTarget, out hit, distTarget, layerObstacle);
        }


        return isSeePlayer;
    }
    bool IsTimeOver()
    {
        return _model.CurrentTimer > 0 && _model.CurrentTimer < 5;
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
    void ActionAvoidance()
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

    public void InicializateSeek()
    {

       
        RandomSystem.Shuffle(patrolPoints);
        _patrol = new Patrol(transform, patrolPoints, patrolPoints[0], _layerMaskPath, layerObstacle, 20, detectionRadius, detectionAngle, 20);
    
    }

    private void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;

        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, detectionAngle / 2, 0) * transform.forward * detectionRadius);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, - detectionAngle / 2, 0) * transform.forward * detectionRadius);
    }
}
