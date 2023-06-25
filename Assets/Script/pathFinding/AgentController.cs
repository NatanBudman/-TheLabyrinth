using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    public EntityBase crash;
    public Box box;
    public diffNode goalNode;
    public diffNode startNode;
    public float radius;
    Collider[] _colliders;
    public LayerMask maskNodes;
    public LayerMask maskObs;
    List<Vector3> lastPathTest;

    [Header("Vector")]
    public float range;

 
    AStar<diffNode> _ast;
    

    private void Awake()
    {
       
        _ast = new AStar<diffNode>();
        _colliders = new Collider[10];
    }

    public void Start()
    {
        AStarPlusRun();
    }

    public void AStarPlusRun()
    {
        var start = startNode;
        if (start == null) return;
        var path = _ast.Run(start, Satisfies, GetConections, GetCost, Heuristic, 500);
        path = _ast.CleanPath(path, InView);
        crash.SetWayPoints(path);
        box.SetWayPoints(path);
    }

    bool InView(diffNode from, diffNode to)
    {
        Debug.Log("CLEAN");
        if (Physics.Linecast(from.transform.position, to.transform.position, maskObs)) return false;
        //Distance
        //Angle
        return true;
    }
    float Heuristic(diffNode curr)
    {
        float multiplierDistance = 2;
        float cost = 0;
        cost += Vector3.Distance(curr.transform.position, goalNode.transform.position) * multiplierDistance;
        return cost;
    }
    float GetCost(diffNode parent, diffNode son)
    {
        float multiplierDistance = 1;
        //float multiplierEnemies = 20;
        float multiplierTrap = 20;

        float cost = 0;
        cost += Vector3.Distance(parent.transform.position, son.transform.position) * multiplierDistance;
        if (son.hasTrap)
            cost += multiplierTrap;
        //cost += 100 * multiplierEnemies;
        return cost;
    }
    List<diffNode> GetConections(diffNode curr)
    {
        return curr.neightbourds;
    }
    bool Satisfies(diffNode curr)
    {
        return curr == goalNode;
    }
    diffNode GetStartNode()
    {
        int count = Physics.OverlapSphereNonAlloc(crash.transform.position, radius, _colliders, maskNodes);
        float bestDistance = 0;
        Collider bestCollider = null;
        for (int i = 0; i < count; i++)
        {
            Collider currColl = _colliders[i];
            float currDistance = Vector3.Distance(crash.transform.position, currColl.transform.position);
            if (bestCollider == null || bestDistance > currDistance)
            {
                bestDistance = currDistance;
                bestCollider = currColl;
            }
        }
        if (bestCollider != null)
        {
            return bestCollider.GetComponent<diffNode>();
        }
        else
        {
            return null;
        }
    }
    private void OnDrawGizmos()
    {
        if (lastPathTest != null)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < lastPathTest.Count - 2; i++)
            {
                Gizmos.DrawLine(lastPathTest[i], lastPathTest[i + 1]);
            }
        }
    }
}
