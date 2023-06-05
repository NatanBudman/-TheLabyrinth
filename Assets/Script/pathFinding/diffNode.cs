using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class diffNode : MonoBehaviour
{
    public List<diffNode> neightbourds;
    public bool hasTrap;
    Material mat;
    private void Start()
    {
        mat = GetComponent<Renderer>().material;
       
    }
    private void Update()
    {
        if (hasTrap)
            mat.color = Color.red;
        else
            mat.color = Color.white;
    }
}
