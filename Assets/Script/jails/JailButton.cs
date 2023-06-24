using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class JailButton : MonoBehaviour,IObstacles
{
    public GameObject player;
    public GameObject JailDoor;
    public GameObject Handler;

    public float heightOpen;
    private float checkHeight;
    public float velocity;

    public KeyCode Interactue = KeyCode.E;

    private delegate void OpenJail();

    private event OpenJail OnOpenJail;
    private event OpenJail OnHandlerAnim;


    private bool isOpenJail = false;
    private void Start()
    {
        checkHeight = JailDoor.transform.position.y + heightOpen;
        OnOpenJail += Open;
        OnHandlerAnim += HandlerAnimated;
    }

    public void Execute()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < 5)
        {
            if (Input.GetKeyDown(Interactue))
            {
                isOpenJail = true;
            }
        }

        if (isOpenJail)
        {
            if (OnHandlerAnim != null) HandlerAnimated();
            if (OnOpenJail != null) OnOpenJail();
            else
            {
                this.GetComponent<JailButton>().enabled = false;
            }
        }
    }

    void HandlerAnimated()
    {
        Handler.transform.Rotate(-Mathf.Abs(Handler.transform.rotation.x),
            0, 0);
        Debug.Log("entre");
        OnHandlerAnim -= HandlerAnimated;
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
