using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class ScooterRL : Agent
{

    // Animator anim;
    // [SerializeField] GameObject center;
    // [SerializeField] GameObject edge;
    // [SerializeField] GameObject predator;
    // [SerializeField] Material winMaterial;
    // [SerializeField] Material loseMaterial;
    // [SerializeField] Material HitWalMarMaterial;
    // [SerializeField] GameObject index;
    // [SerializeField] GameObject Door;
    // [SerializeField] GameObject OpenDoor;
    // [SerializeField] GameObject CloseDoor;

    // [SerializeField] GameObject[] Walls;
    // [SerializeField] GameObject[] Triggers;
    // [SerializeField] GameObject LazerSource;
    // public LayerMask WallsLayer;
    // Material originalMaterial;
    // MeshRenderer indexMesh;

    // float speed = 45.0f;
    // Vector3 maxDistAway;
    // Quaternion SlimeoriginalRotation;
    // Quaternion PredoriginalRotation;
    // Rigidbody SlimeRb;
    // Transform predOT;
    Vector3 MoveForce;
    Vector3 ScooterOrigPos;

     bool StartAutoDrive = false;

    void Awake(){
        ScooterOrigPos = this.transform.position;
    }

    public void StartAutoDriveW(){
        StartAutoDrive =! StartAutoDrive;
    }
    public override void OnEpisodeBegin()
    {
        this.transform.position = ScooterOrigPos;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
           //   ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        ActionSegment<int> discretesActions = actionsOut.DiscreteActions;
         ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        
        switch(Mathf.RoundToInt(Input.GetAxisRaw("Vertical"))){
        case 0: discretesActions[0]=0;break;
        case -1: discretesActions[0]=1;break;
        case 1: discretesActions[0]=2;break;
        }
    switch(Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"))){
        case 0: discretesActions[1]=0;break;
        case -1: discretesActions[1]=1;break;
        case 1: discretesActions[1]=2;break;
        }

        discretesActions[2] = Input.GetKey(KeyCode.E)?1:0; 
        discretesActions[3] = Input.GetKey(KeyCode.F)?1:0; 
        discretesActions[4] = Random.Range(1,3); 
        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");

    //   discretesActions[1] = Input.GetKey(KeyCode.M)?1:0; 
    }

 public void MoveSlime(float x,float z,float moveSpeed = 120.0f,float drag = 0.91f,float traction = 0.9f){

    float steerAngle =15.0f;
     MoveForce += this.transform.forward * z * moveSpeed * Time.deltaTime;
        this.transform.position += MoveForce * Time.deltaTime;
        
        float SteerInput =x;
        transform.Rotate(Vector3.up * SteerInput * MoveForce.magnitude * steerAngle * Time.deltaTime);

        MoveForce *= drag;
        MoveForce = Vector3.ClampMagnitude(MoveForce,70);

        Debug.DrawRay(this.transform.position,this.transform.forward * 10,Color.blue);
        Debug.DrawRay(this.transform.position,MoveForce.normalized * 3);
        MoveForce = Vector3.Lerp(MoveForce.normalized,this.transform.forward , traction * Time.deltaTime) * MoveForce.magnitude;
 }


 public override void OnActionReceived(ActionBuffers actions){
    // Debug.Log(actions.DiscreteActions[0]+","+actions.DiscreteActions[1]);
     float moveZ = actions.DiscreteActions[0];
     float moveX = actions.DiscreteActions[1];
      Vector3 addForce = new Vector3(0,0,0);
      Quaternion rotation = Quaternion.Euler(0,0,0);

      switch(moveZ){
        case 0: addForce.z=0f;break;
        case 1: addForce.z=-1f;break;
        case 2: addForce.z=1f;break;
      }
      switch(moveX){
        case 0: addForce.x=0f;break;
        case 1: addForce.x=-1f;break;
        case 2: addForce.x=1f;break;
      }
    //  Debug.Log(moveX + " | "+moveZ);
     float moveSpeed = (actions.DiscreteActions[4] + 2) * 7.0f;
    //  Debug.Log(moveSpeed);
    //  Move(addForce);
    // SlimeRb.velocity = addForce * moveSpeed + new Vector3(0,SlimeRb.velocity.y,0);
     if(StartAutoDrive)MoveSlime(actions.ContinuousActions[0],Mathf.Clamp01(actions.ContinuousActions[1]));
     
     bool inDanger = actions.DiscreteActions[2]==1;
     bool canAttackPred = actions.DiscreteActions[3]==1;
       
  }

     
     void OnCollisionEnter(Collision collision){
         if(collision.collider.transform.name == "Predator"){
             Debug.Log("Hunted");
            //  indexMesh.material = loseMaterial;
             AddReward(-1.0f);
            //  predator.GetComponent<Predator>().enabled = false;
             EndEpisode();
        }else if(collision.collider.transform.name == "OpenDoor"){
             Debug.Log("knock knock");
            //  indexMesh.material = winMaterial;
             AddReward(10.0f);
             EndEpisode();
        }
        else if(collision.collider.transform.CompareTag("MovingWall")){
             Debug.Log("Collided with moving Walls");
            //   indexMesh.material = HitWalMarMaterial;
             AddReward(-9.2f);
        }
        else if(collision.collider.transform.CompareTag("Wall")){
             Debug.Log("Collided wall ");
             AddReward(-0.6f);
        }
     }

     void OnCollisionExit(Collision collision){
        if(collision.collider.transform.CompareTag("PlayGround")){
             Debug.Log("Ouuuuuuuuuuut");
            //  indexMesh.material = loseMaterial;
             AddReward(-5.0f);
             EndEpisode();
        }
     }
     void OnTriggerEnter(Collider collider){
          if(collider.transform.CompareTag("TriggerWall")){
            Debug.Log("Passed The Trigger");
             AddReward(10.0f/MaxStep);
          }
     }
}
