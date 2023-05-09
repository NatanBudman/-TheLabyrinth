using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Skewers : MonoBehaviour,IObstacles
{
    public bool isActive = true;

    public float VelDoor;

   [SerializeField]float maxUp;
   [SerializeField]private float minDown;
   [SerializeField] private GameObject SkewerObject;
   [SerializeField] private Collider SkewerCol;
   
   [Header("floats")]

   [SerializeField] private float CoolwdownToActive;
   [SerializeField] private float minCoolwdownToActive;
   [SerializeField] private float MaxCoolwdownToActive;
   
   [Header("Sound")]
   [SerializeField] private AudioSource SkewersActiveSound;
   private float _currentToActive;

   private void Awake()
   {
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
        if (SkewerObject.transform.position.y < maxUp )
        {
            SkewerObject.transform.position = new Vector3(SkewerObject.transform.position.x, SkewerObject.transform.position.y + VelDoor * Time.deltaTime,
                SkewerObject.transform.position.z);

            if (SkewersActiveSound != null) SkewersActiveSound.Play();

            if (SkewerCol.isTrigger)  SkewerCol.isTrigger = false;

        }
        
    }

    private void CloseSkewers()
    {
        if (SkewerObject.transform.position.y >= minDown)
        {
            SkewerObject.transform.position = new Vector3(SkewerObject.transform.position.x, SkewerObject.transform.position.y - VelDoor * Time.deltaTime,
                SkewerObject.transform.position.z);

            if (!SkewerCol.isTrigger)   SkewerCol.isTrigger = true;

          


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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
           Destroy(collision.gameObject); 
        }
    }
}
