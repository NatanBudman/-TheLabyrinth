using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOV_Testing : MonoBehaviour
{
    private Rigidbody _rb;

    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (x != 0 || y != 0)
        {
            Vector3 dir = transform.forward * y + transform.right * x;
            
            _rb.MovePosition(transform.position + dir * speed * Time.deltaTime);
        }
    }
}
