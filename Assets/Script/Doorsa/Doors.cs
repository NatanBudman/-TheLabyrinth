using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    public Animator anim;

    [SerializeField] private Collider _doorCollider;

    public bool isOpening;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player") 
        {
            if (Input.GetKey(KeyCode.E)) 
            {
                isOpening = !isOpening;
            }
            _doorCollider.isTrigger = isOpening;
            anim.SetBool("isOpen", isOpening);
        }
    }

}
