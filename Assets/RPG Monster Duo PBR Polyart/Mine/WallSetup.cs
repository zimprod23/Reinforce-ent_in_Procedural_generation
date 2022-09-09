using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSetup : MonoBehaviour
{
    [SerializeField] GameObject UpperLock;
   [SerializeField] GameObject BottomLock;
   public LayerMask GroundMask;
   public float openSpeed = 10.0f;
   
   public bool isOpened;
   public bool isGrounded;
   public string state;

   public void Open(){
     state = "open";
   }
   public void Close(){
     state = "close";
   }

   void FixedUpdate(){
        isOpened = Physics.CheckSphere(BottomLock.transform.position,0.05f,GroundMask);
        isGrounded = Physics.CheckSphere(UpperLock.transform.position,0.09f,GroundMask);
        if(state == "open"){  
            if(!isGrounded){
               Opening();
            }

        }else if(state == "close"){
           if(!isOpened){
            Closing();
           }
        }
   }

   void Opening(){
      this.transform.Translate(-Vector3.up * openSpeed * Time.deltaTime,Space.Self);
   }
   void Closing(){
     this.transform.Translate(Vector3.up * openSpeed * Time.deltaTime,Space.Self);
   }

}
