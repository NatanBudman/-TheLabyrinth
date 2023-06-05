using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NODOSLINKED : MonoBehaviour
{
   public Node[] nodes;

    public void GetNeighboard() 
    {
        nodes = FindObjectsOfType<Node>();

        int lenght = nodes.Length;

        for(int i = 0; i < lenght; i++) 
        {
            nodes[i].GetNeigboards();
        }
       


    }
}
