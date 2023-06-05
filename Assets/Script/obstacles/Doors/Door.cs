using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour,IObstacles
{
    public bool isOpen = true;

    public float VelDoor;

   float max;
   private float min;

   [SerializeField] private float CoolwdownToActive;
   [SerializeField] private float minCoolwdownToActive;
   [SerializeField] private float MaxCoolwdownToActive;
   private float _currentToActive;

   private Action OnExecuted;
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

   private void _Door()
   {
       isOpen = !isOpen;
   }

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

    public void Execute()
    {
        _currentToActive += Time.deltaTime;

        
        if (_currentToActive > CoolwdownToActive)
        {
            OnExecuted += _Door;
            
            OnExecuted.Invoke();

            _currentToActive = 0;
            
            CoolwdownToActive = RandomSystem.Range(minCoolwdownToActive, MaxCoolwdownToActive);
        }
       
    }
}
