using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRayCast : MonoBehaviour
{
    public LayerMask mask;
    // public GameObject Lazer;
   public Transform SlimeRayCast(LayerMask targetMask){
      Ray ray = new Ray(this.transform.position,this.transform.forward);
      RaycastHit hitInfo;
      if(Physics.Raycast(ray,out hitInfo,100,targetMask,QueryTriggerInteraction.Collide)){
        Debug.DrawLine(ray.origin,hitInfo.point,Color.green);
         return hitInfo.collider.transform;
      }else{
         Debug.DrawLine(ray.origin,ray.origin + ray.direction * 1000,Color.red);
         return null;
      }
    }

    public string HitPointInfo(){
        Transform hit =  SlimeRayCast(mask);
        if(hit){
            if(hit.CompareTag("MovingWall")){
               return "wall";
            }else if(hit.CompareTag("TriggerWall")){
               return "twall";
            }
        }
        return "";
    }

    // Update is called once per frame
    // void Update()
    // {
    //     SlimeRayCast(mask);
    // }
}
