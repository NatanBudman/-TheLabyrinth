using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPuzzle : MonoBehaviour
{
    public bool isOpen = true;

    public float VelDoor;

   float max;
   private float min;

   private void Awake()
   {
       max = transform.position.y;
       min = transform.position.y - 1;
   }

   private void Update()
    {
        if (isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }
    // Update is called once per frame

    private void OpenDoor()
    {
        if (transform.position.y < max )
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + VelDoor * Time.deltaTime,
                transform.position.z);
        }
        
    }

    private void CloseDoor()
    {
        if (transform.position.y >= min)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - VelDoor * Time.deltaTime,
                transform.position.z);
        }
    }

}
