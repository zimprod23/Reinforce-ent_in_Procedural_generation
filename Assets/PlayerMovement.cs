using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    float drag = 0.98f;
    private Vector3 moveForce;
    float steerAngle = 20.0f;
    public float traction = 1;

    // Update is called once per frame
    void FixedUpdate()
    {
        moveForce += this.transform.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime;
        this.transform.position += moveForce * Time.deltaTime;
        
        float SteerInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * SteerInput * moveForce.magnitude * steerAngle * Time.deltaTime);

        moveForce *= drag;
        moveForce = Vector3.ClampMagnitude(moveForce,15);

        Debug.DrawRay(this.transform.position,this.transform.forward * 10,Color.blue);
        Debug.DrawRay(this.transform.position,moveForce.normalized * 3);
        moveForce = Vector3.Lerp(moveForce.normalized,this.transform.forward , traction * Time.deltaTime) * moveForce.magnitude;
    }
}
