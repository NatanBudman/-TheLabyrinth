using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    public Animator anim;

    [SerializeField] private Collider _doorCollider;

    public bool isOpening;

    private float _cooldown = 0.3f;
    private float _currentCooldown;

    private void Update()
    {
        anim.SetBool("isOpen", isOpening);
        
        if (_currentCooldown <= _cooldown) _currentCooldown += Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            if (Input.GetKey(KeyCode.E)) 
            {
                if (_currentCooldown > _cooldown)
                {
                    isOpening = !isOpening;
                    _doorCollider.isTrigger = isOpening;
                    
                    _currentCooldown = 0;
                }
            }
           
        }
    }

}
