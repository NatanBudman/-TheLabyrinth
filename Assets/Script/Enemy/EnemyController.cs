using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    EntityBase _model;
    FSM<EnemyStateEnum> _fsm;
     Patrol _patrol;
    EnemyController _controller;
    Persuit _seek;
    public ObstacleAvoidance _obstacleAvoidance;
    public AgentController _agentController;
    ITreeNode _root;
    public Transform target;
    
    private ISteering _steering;

    [Header("Path")]

    public LayerMask _layerMaskPath;
 
    [SerializeField] Transform[] patrolPoints;

    [Header("Detection Parameters")]
    [SerializeField] public LayerMask layerObstacle;
    [SerializeField] public LayerMask enemyObstacle;
    [SerializeField] public LayerMask obstruirVision;
    public float detectionRadius;
    public float detectionAngle;
    public float PlayerdetectionAngle;
    public float obstacleDetectionRadius;
    public float obstacleDetectionAngle;
    private void Awake()
    {
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
        var following = new EnemyFollowingState<EnemyStateEnum>();

        list.Add(idle);
        list.Add(chase);
        list.Add(patrol);
        list.Add(attack);
        list.Add(following);


        for (int i = 0; i < list.Count; i++)
        {
            list[i].InitializedState(_model, _fsm, _controller, _patrol, _seek, _obstacleAvoidance, _agentController);
        }

        idle.AddTransition(EnemyStateEnum.Chase, chase);
        idle.AddTransition(EnemyStateEnum.Patrol, patrol);
        idle.AddTransition(EnemyStateEnum.Attack, attack);

        chase.AddTransition(EnemyStateEnum.Patrol, patrol);
        chase.AddTransition(EnemyStateEnum.Attack, attack);
        chase.AddTransition(EnemyStateEnum.Idle, idle);
        chase.AddTransition(EnemyStateEnum.Following, following);

        patrol.AddTransition(EnemyStateEnum.Chase, chase);
        patrol.AddTransition(EnemyStateEnum.Idle, idle);

        following.AddTransition(EnemyStateEnum.Idle, idle);
        following.AddTransition(EnemyStateEnum.Patrol, patrol);

        _fsm.SetInit(idle);
    }

    public void InitializedTree()
    {
        //actions
        var idle = new TreeAction(ActionIdle);
        var patrol = new TreeAction(ActionPatrol);
        var chase = new TreeAction(ActionChase);
        var attack = new TreeAction(ActionAttack);
        var following = new TreeAction(ActionFollowing);

        //questions
        
        var isTimeOver = new TreeQuestion(IsTimeOver, patrol, idle);
       // var keepSeeing = new TreeQuestion(KeepSeeing, following, chase);
        var isTouching = new TreeQuestion(IsTouching, attack, chase);
        var sawPlayer = new TreeQuestion(KeepSeeing, isTouching, isTimeOver);          
        var isAlive = new TreeQuestion(IsAlive, sawPlayer, null);

        _root = isAlive;
    }


    bool KeepSeeing()
    {
        if (SawPlayer())
        {
            if(_model.currentKeep() > 0)
            {
                _model.resetTimer();
            }
        }
        if (!SawPlayer())
        {

            _model.KeepTimer();

        }

        return _model.currentKeep() >= _model.keepTimer;
    }
    bool IsTouching()
    {
        return _model.IsTouchPlayer;
    }

    bool isChaseingPlayer = false;

    bool Ischaseing()
    {
        if (SawPlayer())
        {
            
            isChaseingPlayer = true;
        }
        if (!SawPlayer())
        {
           
                isChaseingPlayer = false;
            
            
        }
        return isChaseingPlayer;
    }

    public bool SawPlayer()
    {
        bool isSeePlayer = false ;

        Vector3 diffPoint = target.transform.position - transform.position;

        float angleToPoint = Vector3.Angle(transform.forward, diffPoint);
        if(angleToPoint < PlayerdetectionAngle/2)
        {
            Vector3 diff = (target.position - transform.position);
            Vector3 dirToTarget = diff.normalized;
            float distTarget = diff.magnitude;

            RaycastHit hit;

           isSeePlayer= !Physics.Raycast(transform.position, dirToTarget, out hit, distTarget, obstruirVision);
        }
         
        return isSeePlayer;
    }
    bool IsTimeOver()
    {
        return _model.CurrentTimer > 0 && _model.CurrentTimer < 16;
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
    void ActionFollowing()
    {
        _fsm.Transitions(EnemyStateEnum.Following);
    }
   
    public Vector3 Run()
    {
        // Debug.Log("RunASTAR");
        var point = _model.waypoints[_model._nextPoint];

        var posPoint = point;
        posPoint.y = transform.position.y;
        Vector3 dir = posPoint - transform.position;
        if (dir.magnitude < 0.2f)
        {
            if (_model._nextPoint + 1 < _model.waypoints.Count)
                _model._nextPoint++;
            else
            {
                _model.readyToMove = false;

                return dir;
            }
        }
        return dir;
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
        
        Gizmos.color = Color.green;

        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, PlayerdetectionAngle / 2, 0) * transform.forward * detectionRadius);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, - PlayerdetectionAngle / 2, 0) * transform.forward * detectionRadius);
        
        Gizmos.color = Color.yellow;

        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, obstacleDetectionAngle / 2, 0) * transform.forward * obstacleDetectionRadius);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -obstacleDetectionAngle / 2, 0) * transform.forward * obstacleDetectionRadius);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, obstacleDetectionRadius);
    }
}
