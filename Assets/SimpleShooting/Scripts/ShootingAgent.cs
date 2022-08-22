using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class ShootingAgent : Agent
{
    public Transform ShootingPoint;
    public int damage = 100;
    public int minShotBetweenShots = 50;
    public bool ShotAvailable = true;
    private int stepsUntilShotAvailable = 0;

    private void Shoot(Vector3 dir){
        if(!ShotAvailable)return;
       int layerMask = 1 << LayerMask.NameToLayer("Enemy");
       Debug.DrawRay(ShootingPoint.position,dir,Color.red,2f);

       if(Physics.Raycast(ShootingPoint.position,dir,out var hit,200f,layerMask)){
        hit.transform.GetComponent<EnemyScript>().GetShot(damage);
       }
       ShotAvailable = false;
       stepsUntilShotAvailable = minShotBetweenShots;  
    }

     private void OnMouseDown(){
        Shoot(transform.right);
     }

     public override void OnActionReceived(ActionBuffers actions){
       if(Mathf.RoundToInt(actions.DiscreteActions[0]) > 1){
        Shoot(transform.forward);
       } 
     }

    public override void OnEpisodeBegin()
    {
      
    }
    public override void CollectObservations(VectorSensor sensor){
      
    }

public override void Heuristic(in ActionBuffers actionsOut){
     
}

void FixedUpdate(){
    if(!ShotAvailable){
        stepsUntilShotAvailable--;
      if(stepsUntilShotAvailable <= 0)ShotAvailable = true;
    }
}
 
}
