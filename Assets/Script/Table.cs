using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public float speed = 15;

    public Vector3 currentRot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentRot = GetComponent<Transform>().eulerAngles;

        if((Input.GetAxisRaw("Horizontal") > 0 ) && (currentRot.z <= 10 || currentRot.z >= 348))         
        {


            transform.Rotate(0, 0, 1 * speed * Time.deltaTime);
            Debug.Log("derecha");
        
        }

        if ((Input.GetAxisRaw("Horizontal") < -0) && (currentRot.z >= 349 || currentRot.z <= 11)) 
        {


            transform.Rotate(0, 0, -1 * speed * Time.deltaTime);
            Debug.Log("izquierda");
        }


        if ((Input.GetAxisRaw("Vertical")  > 0) && (currentRot.x <= 10 || currentRot.x >= 348)) 
        {


            transform.Rotate(speed * Time.deltaTime * 1, 0, 0);

        }

        if ((Input.GetAxisRaw("Vertical") < -0) && (currentRot.x >= 349 || currentRot.x <= 11)) 
        {


            transform.Rotate (speed * Time.deltaTime * -1, 0, 0) ;

        }
    }
}
