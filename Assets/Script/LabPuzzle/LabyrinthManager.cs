using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthManager : MonoBehaviour
{
    private Dictionary<Skewers, float> Dictionary = new Dictionary<Skewers, float>();

    public Skewers[] doors;

    private void Awake()
    {
        
        
        for (int i = 0; i < doors.Length; i++)
        {
            Dictionary[doors[i]] = i;
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Skewers skewers = RandomSystem.Roulette(Dictionary);
            skewers.isActive =! skewers.isActive;
            skewers = null;
            
        }
    }
}
