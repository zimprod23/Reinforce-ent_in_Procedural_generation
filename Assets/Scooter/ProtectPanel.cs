using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectPanel : MonoBehaviour
{
    Quaternion originalRotation;

    void Start(){
       originalRotation = this.transform.rotation;
    }
   void OnCollisionEnter(Collision collision){
    if(collision.collider.gameObject.CompareTag("PlayGround")){
        this.transform.rotation = originalRotation;
    }
   }
}
