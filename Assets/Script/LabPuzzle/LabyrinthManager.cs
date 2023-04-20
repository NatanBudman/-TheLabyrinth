using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthManager : MonoBehaviour
{
    private Dictionary<Door, float> Dictionary = new Dictionary<Door, float>();

    public Door[] doors;

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
            Door door = RandomSystem.Roulette(Dictionary);
            door.isOpen =! door.isOpen;
            door = null;
            
        }
    }
}
