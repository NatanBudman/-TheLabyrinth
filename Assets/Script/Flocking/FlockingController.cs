using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingController : MonoBehaviour
{
    FlockingPrueba _model;
    FSM<CivilStateEnum> _fsm;
    ITreeNode _root;
    public Transform target;
    [SerializeField] public LayerMask obstruirVision;
    [SerializeField] public LayerMask layerObstacle;
    public float PlayerdetectionAngle;
    FlockingController _controller;
    FlockingManager _manager;
    ObstacleAvoidance _obstacleAvoidance;
    public float obstacleDetectionRadius;
    public float obstacleDetectionAngle;
    void Awake()
    {
        _model = GetComponent<FlockingPrueba>();
        _controller = GetComponent<FlockingController>();
        _manager = GetComponent<FlockingManager>();

        IntializedFSM();
        InitializedTree();

    }
    public void IntializedFSM()
    {
        var list = new List<CivilStateBase<CivilStateEnum>>();
        _fsm = new FSM<CivilStateEnum>();

        var idle = new CivilIdleState<CivilStateEnum>();
        var chase = new CiivilChaseState<CivilStateEnum>();


        list.Add(idle);
        list.Add(chase);




        for (int i = 0; i < list.Count; i++)
        {
            list[i].InitializedState(_model, _fsm, _controller, _manager, _obstacleAvoidance);
        }

        idle.AddTransition(CivilStateEnum.Chase, chase);

        chase.AddTransition(CivilStateEnum.Idle, idle);

        _fsm.SetInit(idle);
    }
    public void InitializedTree()
    {
        //actions
        var idle = new TreeAction(ActionIdle);
        var chase = new TreeAction(ActionChase);


        //questions

        var sawPlayer = new TreeQuestion(Ischaseing, chase , idle);
        var isAlive = new TreeQuestion(IsAlive, sawPlayer, null);

        _root = isAlive;
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
    bool IsAlive()
    {
        return true;
    }
    bool SawPlayer()
    {
        bool isSeePlayer = false;

        Vector3 diffPoint = target.transform.position - transform.position;

        float angleToPoint = Vector3.Angle(transform.forward, diffPoint);
        if (angleToPoint < PlayerdetectionAngle / 2)
        {
            Vector3 diff = (target.position - transform.position);
            Vector3 dirToTarget = diff.normalized;
            float distTarget = diff.magnitude;

            RaycastHit hit;

            isSeePlayer = !Physics.Raycast(transform.position, dirToTarget, out hit, distTarget, obstruirVision);
        }

        return isSeePlayer;
    }
    void ActionIdle()
    {
        _fsm.Transitions(CivilStateEnum.Idle);
    }
    void ActionChase()
    {
        _fsm.Transitions(CivilStateEnum.Chase);
    }
    private void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();
    }
}
