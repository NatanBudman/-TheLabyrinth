using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AgentController : MonoBehaviour
{
    private Dictionary<diffNode, float> dicNodos = new Dictionary<diffNode, float>();
    public EntityBase crash;
    public Box box;
    public diffNode goalNode;
    public diffNode startNode;
    public float radius;
    Collider[] _colliders;
    public LayerMask maskNodes;
    public LayerMask maskObs;
    List<Vector3> lastPathTest;
    private diffNode[] listaNodos;
    public PlayerModel _player;
    [Header("Vector")]
    public float range;

    [Header("Map")] 
    public GameManager map;
    private Collider[] CollNodes;
    private List<diffNode> setDiffNodes = new List<diffNode>();
    public float LimitRange;
    AStar<diffNode> _ast;
    

    private void Awake()
    {
        listaNodos = FindObjectsOfType<diffNode>();
         _ast = new AStar<diffNode>();
        _colliders = new Collider[10];
        CollNodes = new Collider[15];
    }

    public void Start()
    {
        newRoute();

    }
    /*
    public void Update()
    {
        if(Vector2.Distance(crash.transform.position, goalNode.transform.position)< 1)
        {
            Debug.Log("Cambio de ruta");
            newRoute();

        }
    }
    */
    public void AStarPlusRun()
    {
        var start = startNode;
        if (start == null) return;
        var path = _ast.Run(start, Satisfies, GetConections, GetCost, Heuristic, 500);
        // mover el run al estado
        path = _ast.CleanPath(path, InView);
        crash.SetWayPoints(path);
        box.SetWayPoints(path);

    }

    private Vector3 RandomGeneratePos(int zone)
    {
        Vector3 pos = Vector3.zero;
        // Tomar el Ancho y larggo del mapa y que busque los nodos
        Vector3 _map = map.transform.position;
        float randomX = 0;
        float randomZ = 0;
        switch (zone)
        {
            case 0:
                 randomX = Random.Range(-(map.transform.localScale.x / 2) ,0);
                 randomZ = Random.Range(-( map.transform.localScale.z / 2)  , 0);
                break;
            case 1:
                randomX = Random.Range(0 ,(map.transform.localScale.x / 2));
                randomZ = Random.Range(0 ,(map.transform.localScale.z / 2));
                break;
            case 2:
                randomX = Random.Range((map.transform.localScale.x / 2) ,(map.transform.localScale.x / 2));
                randomZ = Random.Range(( map.transform.localScale.z / 2) , ( map.transform.localScale.z / 2));
                break;
            case 3 :
                randomX = Random.Range((map.transform.localScale.x / 2) ,(map.transform.localScale.x / 2));
                randomZ = Random.Range(0 , ( map.transform.localScale.z / 2));
                break;
            case 4 :
                randomX = Random.Range(0 ,(map.transform.localScale.x / 2));
                randomZ = Random.Range(( map.transform.localScale.z / 2) , ( map.transform.localScale.z / 2));
                break;
        }            
        
        pos = new Vector3(_map.x + (randomX * randomZ),_map.y, _map.z + (randomX * randomZ));

        return pos;
    }
    
    private void buildingDictionary()
    {
        dicNodos.Clear();
        setDiffNodes.Clear();
        
        int random = Random.Range(0, 4);
        _player.transform.position = RandomGeneratePos(random);
        startNode = GetPosNode(transform.position);
        setDiffNodes = GetPosNodes( RandomGeneratePos(random));
        
            for (int i = 0; i < setDiffNodes.Count; i++)
            {
                float dist = Vector2.Distance(setDiffNodes[i].transform.position, _player.transform.position);
                if (!dicNodos.ContainsKey(setDiffNodes[i]))
                {
                    dicNodos.Add(setDiffNodes[i], dist);
                }
                else
                {
                    dicNodos[setDiffNodes[i]] = dist;
                }
            }
        
    }
    public void newRoute()
    {
        buildingDictionary();
        goalNode = RandomSystem.Roulette(dicNodos);
        AStarPlusRun();

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
    
    List<diffNode> SetNodes = new List<diffNode>();
    List<diffNode> GetPosNodes(Vector3 pos)
    {
        SetNodes.Clear();
        
        int count = Physics.OverlapSphereNonAlloc(pos, radius, CollNodes, maskNodes);
        for (int i = 0; i < count; i++)
        {
            Collider currColl = _colliders[i];
            //InView-- SI no continue
            if (Physics.Linecast(pos,currColl.transform.position,maskObs)) continue;
            
            SetNodes.Add(currColl.GetComponent<diffNode>());
        }

        return SetNodes;
    }

    diffNode GetPosNode(Vector3 pos)
    {
        int count = Physics.OverlapSphereNonAlloc(pos, radius, _colliders, maskNodes);
        float bestDistance = 0;
        Collider bestCollider = null;
        for (int i = 0; i < count; i++)
        {
            Collider currColl = _colliders[i];
            float currDistance = Vector3.Distance(pos, currColl.transform.position);
            //InView-- SI no continue
            if (Physics.Linecast(pos,currColl.transform.position,maskObs)) continue;
                
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
