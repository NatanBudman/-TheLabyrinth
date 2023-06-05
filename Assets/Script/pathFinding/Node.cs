using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour 
{
    
    public bool walkable;
    public float CooldownWalkable = 5;

    public Node[] NeighboardNodes;

    private void Update()
    {
        if (!walkable)
        {
            StartCoroutine(RaturnWalkable());
        }
    }
    

    IEnumerator RaturnWalkable()
    {
        
       yield return  new WaitForSeconds(CooldownWalkable);
        walkable = true;
        StopCoroutine( RaturnWalkable());
    }


    [SerializeField] private LayerMask NodeLayers;
    private float _radius = 45;
    private Collider[] Nodes;
    public LayerMask IgnoreLayer;

    public void GetNeigboards() 
    {

        List<GameObject> objectsInRange = GetObjectsInRadius(transform.position, _radius);
        List<GameObject> objectsWithComponent = new List<GameObject>();
        foreach (GameObject obj in objectsInRange)
        {
            if (obj.GetComponent<Node>() != null && !CheckCollision(transform.position, obj.transform.position) && obj != this.gameObject)
            {
                objectsWithComponent.Add(obj);
            }
        }

        NeighboardNodes = new Node[objectsWithComponent.Count];

        for (int i = 0; i < objectsWithComponent.Count; i++) 
        {
            NeighboardNodes[i] = objectsWithComponent[i].GetComponent<Node>();
        }
    }

    public List<GameObject> GetObjectsInRadius(Vector3 center, float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(center, radius);
        List<GameObject> objectsInRange = new List<GameObject>();
        foreach (Collider col in colliders)
        {
            objectsInRange.Add(col.gameObject);
        }
        return objectsInRange;
    }
    bool CheckCollision(Vector3 start, Vector3 end)
    {
        RaycastHit hit;
        if (Physics.Linecast(start, end, out hit, IgnoreLayer))
        {
            return true;
        }
        return false;
    }
}

