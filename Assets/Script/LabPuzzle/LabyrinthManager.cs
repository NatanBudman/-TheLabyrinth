using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthManager : MonoBehaviour
{
    private Dictionary<DoorPuzzle, float> Dictionary = new Dictionary<DoorPuzzle, float>();

    public DoorPuzzle[] doors;

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
            DoorPuzzle doorPuzzle = RandomSystem.Roulette(Dictionary);
            doorPuzzle.isOpen =! doorPuzzle.isOpen;
            doorPuzzle = null;
            
        }
    }
}
