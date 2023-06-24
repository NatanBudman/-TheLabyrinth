using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour 
{
    
    public bool walkable;
    public float CooldownWalkable = 5;

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
}

