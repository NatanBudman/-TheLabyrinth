using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Skewers : MonoBehaviour,IObstacles
{
    public bool isActive = true;

    public float VelDoor;

   float max;
   private float min;

   [SerializeField] private float CoolwdownToActive;
   [SerializeField] private float minCoolwdownToActive;
   [SerializeField] private float MaxCoolwdownToActive;
   private float _currentToActive;

   private void Awake()
   {
       max = transform.position.y;
       min = transform.position.y - 1;
   }

   private void Update()
   {
       if (isActive)
       {
           OpenSkewers();
       }
       else
       {
           CloseSkewers();
       }
   }

   private void skewers()
   {
       isActive = !isActive;
   }

   private void OpenSkewers()
    {
        if (transform.position.y < max )
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + VelDoor * Time.deltaTime,
                transform.position.z);

        }
        
    }

    private void CloseSkewers()
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
            skewers();

            _currentToActive = 0;
            
            CoolwdownToActive = RandomSystem.Range(minCoolwdownToActive, MaxCoolwdownToActive);
        }
       
    }
}
