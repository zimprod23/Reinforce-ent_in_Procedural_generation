using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderControll : MonoBehaviour
{
    float speed = 30.0f;
    Rigidbody SpiderRb;

      void Start()
    {
        SpiderRb = GetComponent<Rigidbody>();
    }

    void Move(){
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 AddForce = new Vector3(moveX,0,moveZ);
        SpiderRb.velocity = AddForce * speed  + new Vector3(0,SpiderRb.velocity.y,0);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
