using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [Header("SpeedMov")]
    public float speed;
    Rigidbody _rb;
    public float _speedRotate;
    
    
    [Header("Camera")]
    public float MinRotateCameraY;
    public float MaxRotateCameraY;
    float _rotationY = 0;
    public Camera Camera;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public void Move(Vector3 dir)
    {
        Vector3 dirSpeed = dir * speed;
        dirSpeed.y = _rb.velocity.y;
        _rb.velocity = dirSpeed;
    }
    float rotationX;
    float rotationY;
    public void MouseRotation()
    {
        rotationX += Input.GetAxis("Mouse X") * _speedRotate * Time.deltaTime;
        rotationY += Input.GetAxis("Mouse Y") * _speedRotate * Time.deltaTime;
        
        if (rotationY > MaxRotateCameraY) {
            rotationY = MaxRotateCameraY;
        }
        if (rotationY < MinRotateCameraY) {
            rotationY = MinRotateCameraY;
        }
        
        Camera.transform.rotation = Quaternion.Euler(-rotationY, rotationX, 0);
        transform.rotation = Quaternion.Euler(0, rotationX, 0);

    }
    public void LookRotate(Vector3 dir)
    {

        Quaternion targetRotation = Quaternion.LookRotation(dir);

        transform.transform.rotation = Quaternion.RotateTowards(transform.transform.rotation,
            targetRotation, _speedRotate * Time.deltaTime);
    }
    public void PlayerMove()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = (vertical * transform.forward) + (horizontal * transform.right);
        Vector3 movement = direction * speed * Time.deltaTime;

        transform.position += movement;
    }
    public void LookDir(Vector3 dir)
    {
        if (dir == Vector3.zero) return;
        transform.forward = dir;
    }
}
