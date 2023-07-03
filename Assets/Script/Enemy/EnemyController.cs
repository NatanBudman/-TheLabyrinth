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
        InicializateSeek();
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

        list.Add(idle);
        list.Add(chase);
        list.Add(patrol);
        list.Add(attack);
      


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
        var sawPlayer = new TreeQuestion(Ischaseing, isTouching, isTimeOver);          
        var isAlive = new TreeQuestion(IsAlive, sawPlayer, null);

        _root = isAlive;
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

    bool SawPlayer()
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
