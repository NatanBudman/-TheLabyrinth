using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AgentController : MonoBehaviour
{
    public Dictionary<diffNode, float> dicNodos = new Dictionary<diffNode, float>();
    public EntityBase IA;
    public Box box;
    public diffNode goalNode;
    public diffNode startNode;
    public float radius;
    Collider[] _colliders;
    public LayerMask maskNodes;
    public LayerMask maskObs;
    List<Vector3> lastPathTest;
    public PlayerModel _player;
    [Header("Vector")]
    public float range;

    [Header("Map")] 
    public GameObject map;
    private Collider[] CollNodes;
    List<diffNode> setDiffNodes;
    [Range(1,8)] public int minZonePatrol;
    [Range(1,8)] public int maxZonePatrol;
    public GameObject PruebaObjs;
   [HideInInspector] public AStar<diffNode> _ast;
    

    private void Awake()
    {
        setDiffNodes = new List<diffNode>();
         _ast = new AStar<diffNode>();
        _colliders = new Collider[10];
        CollNodes = new Collider[15];
    }
   
    

    private Vector3 RandomGeneratePos(int zone)
    {
        Vector3 pos = Vector3.zero;
        // Tomar el Ancho y larggo del mapa y que busque los nodos
        Vector3 _map = map.transform.position;
        int randomX = 0;
        int randomZ = 0;
        int scaleX = (int)map.transform.localScale.x;
        int scaleZ = (int)map.transform.localScale.z;
        switch (zone)
        {
            case 1:
                randomX = Random.Range(0 ,scaleX/2);
                randomZ = Random.Range(0 ,scaleZ/2);

                pos = new Vector3(_map.x - randomX ,_map.y, _map.z - randomZ );
                break;
            case 2:
                randomX =Random.Range(-Mathf.Abs(scaleX/2),0);
                randomZ = Random.Range(-Mathf.Abs(scaleZ/2),0);

                pos = new Vector3(_map.x - randomX ,_map.y, _map.z - randomZ );
                break;
            case 3:
                randomX = Random.Range(-Mathf.Abs(scaleX/2),scaleX/2);
                randomZ = Random.Range(-Mathf.Abs(scaleZ/2),0);

                pos = new Vector3(_map.x - randomX ,_map.y, _map.z - randomZ );
                break;
            case 4:
                randomX = Random.Range(-Mathf.Abs( scaleX/2),0);
                randomZ = Random.Range(-Mathf.Abs(scaleZ/2),scaleZ/2);

                pos = new Vector3(_map.x - randomX ,_map.y, _map.z - randomZ );
                break;
            case 5:
                randomX = Random.Range(0 ,scaleX - scaleX/2);
                randomZ = Random.Range(0 ,scaleZ - scaleZ/2);

                pos = new Vector3(_map.x + randomX ,_map.y, _map.z + randomZ );
                break;
            case 6:
                randomX = Random.Range(-Mathf.Abs(scaleX/2),0);
                randomZ = Random.Range(-Mathf.Abs( scaleZ/2),0);

                pos = new Vector3(_map.x + randomX ,_map.y, _map.z + randomZ );
                break;
            case 7:
                randomX = Random.Range(-Mathf.Abs(scaleX/2),scaleX - scaleX/2);
                randomZ = Random.Range(-Mathf.Abs(scaleZ/2),0);

                pos = new Vector3(_map.x + randomX ,_map.y, _map.z + randomZ );
                break;
            case 8:
                randomX = Random.Range(-Mathf.Abs( scaleX/2),0);
                randomZ = Random.Range(-Mathf.Abs( scaleZ/2),scaleZ - scaleZ/2);

                pos = new Vector3(_map.x + randomX ,_map.y, _map.z + randomZ );
                break;
            default:
                pos = new Vector3(_map.x + (randomX * randomZ),_map.y, _map.z + (randomX * randomZ));
                break;
        }            
        
        return pos;
    }
    
    public void buildingDictionary()
    {
        dicNodos.Clear();
        int random = Random.Range(minZonePatrol, maxZonePatrol);
        
        startNode = GetPosNode(transform.position);
        Vector3 pos = RandomGeneratePos(random);
        
        PruebaObjs.transform.position = pos;
        setDiffNodes = GetPosNodes( pos);
        
            for (int i = 0; i < setDiffNodes.Count; i++)
            {
                float dist = Vector2.Distance(setDiffNodes[i].transform.position, _player.transform.position);
                if (!dicNodos.ContainsKey(setDiffNodes[i]))
                {
                    dicNodos.Add(setDiffNodes[i], dist);
                    continue;
                }
                else
                {
                    dicNodos[setDiffNodes[i]] = dist;
                }
            }
        
    }
   
    public bool InView(diffNode from, diffNode to)
    {
       // Debug.Log("CLEAN");
        if (Physics.Linecast(from.transform.position, to.transform.position, maskObs)) return false;
        //Distance
        //Angle
        return true;
    }
   public bool InView(Vector3 from, Vector3 to)
    {
       // Debug.Log("CLEAN");
        if (Physics.Linecast(from, to, maskObs)) return false;
        return true;
    }
    public float Heuristic(diffNode curr)
    {
        float multiplierDistance = 2;
        float cost = 0;
        cost += Vector3.Distance(curr.transform.position, goalNode.transform.position) * multiplierDistance;
        return cost;
    }
   public float GetCost(diffNode parent, diffNode son)
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
    public List<diffNode> GetConections(diffNode curr)
    {
        return curr.neightbourds;
    }
    public bool Satisfies(diffNode curr)
    {
        return curr == goalNode;
    }
    
    List<diffNode> GetPosNodes(Vector3 pos)
    {
        List<diffNode> patrolNodes = new List<diffNode>();
        
        int count = Physics.OverlapSphereNonAlloc(pos, radius * 1.5f, CollNodes, maskNodes);
        for (int i = 0; i < count; i++)
        {
            Collider currColl = CollNodes[i];
            
            patrolNodes.Add(currColl.GetComponent<diffNode>());
        }

        return patrolNodes;
    }
    List<diffNode> GetPosNodes(Vector3 pos,float _radius)
    {
        List<diffNode> patrolNodes = new List<diffNode>();
        
        int count = Physics.OverlapSphereNonAlloc(pos, _radius, CollNodes, maskNodes);
        for (int i = 0; i < count; i++)
        {
            Collider currColl = CollNodes[i];
            
            patrolNodes.Add(currColl.GetComponent<diffNode>());
        }

        return patrolNodes;
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
            if (!InView(pos,currColl.transform.position))continue;
                
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
