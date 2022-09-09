using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellBehaviour : MonoBehaviour
{
    public float speed = 70.0f;
    public Camera mainCam;
    MeshRenderer bulletMesh;
    void Start(){
        bulletMesh = this.GetComponent<MeshRenderer>();
    }
    void Update()
    {
        LaunchProjectile();
    }
    void LaunchProjectile(){
        // bulletMesh.enabled = false;
        this.transform.localRotation = GameObject.Find("Player").transform.localRotation;
        Vector3 projectile = this.transform.forward * speed;
        this.transform.position += projectile * Time.deltaTime;
        projectile = Vector3.ClampMagnitude(projectile,100);
    }
    void OnCollisionEnter(Collision collision){
       Destroy(this.gameObject);
    }
}
