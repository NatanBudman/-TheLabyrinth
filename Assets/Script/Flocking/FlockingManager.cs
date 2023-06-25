using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour
{
    private IBoid _selft;
    private IFlocking[] _flockings;

    private void Awake()
    {
       _flockings = GetComponents<IFlocking>();
       _selft = GetComponent<IBoid>();
    }

    void StartFlocking()
    {
        
    }
}
