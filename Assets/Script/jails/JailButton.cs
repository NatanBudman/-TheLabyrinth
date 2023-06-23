using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class JailButton : MonoBehaviour,IObstacles
{
    public GameObject player;
    public GameObject JailDoor;

    public float heightOpen;
    private float checkHeight;
    public float velocity;

    public KeyCode Interactue = KeyCode.E;

    private delegate void OpenJail();

    private event OpenJail OnOpenJail;


    private bool isOpenJail = false;
    private void Start()
    {
        checkHeight = JailDoor.transform.position.y + heightOpen;
        OnOpenJail += Open;
    }

    public void Execute()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < 3)
        {
            if (Input.GetKeyDown(Interactue))
            {
                isOpenJail = true;
            }
        }

        if (isOpenJail)
        {
            if (OnOpenJail != null) OnOpenJail();
            else
            {
                this.GetComponent<JailButton>().enabled = false;
            }
        }
    }
    
    

    void Open()
    {
        if (JailDoor.transform.position.y < checkHeight)
        {
            var position = JailDoor.transform.position;
            position = new Vector3(position.x, position.y + velocity * Time.deltaTime, position.z);
            JailDoor.transform.position = position;
        }
        else
        {
            OnOpenJail -= Open; 
        }


    }
}
