using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class SlimeController : Agent
{
    
    // Start is called before the first frame update
    Animator anim;
    [SerializeField] GameObject center;
    [SerializeField] GameObject edge;
    [SerializeField] GameObject predator;
    [SerializeField] Material winMaterial;
    [SerializeField] Material loseMaterial;
    [SerializeField] Material HitWalMarMaterial;
    [SerializeField] GameObject index;
    [SerializeField] GameObject Door;
    [SerializeField] GameObject OpenDoor;
    [SerializeField] GameObject CloseDoor;

    [SerializeField] GameObject[] Walls;
    [SerializeField] GameObject[] Triggers;
    [SerializeField] GameObject LazerSource;
    public LayerMask WallsLayer;
    Material originalMaterial;
    MeshRenderer indexMesh;

    float speed = 10.0f;
    Vector3 maxDistAway;
    Quaternion SlimeoriginalRotation;
    Quaternion PredoriginalRotation;
    Rigidbody SlimeRb;
    Transform predOT;
    Vector3 MoveForce;

    void Awake(){
        SlimeoriginalRotation = this.transform.localRotation;
        PredoriginalRotation = predator.transform.localRotation;
        predOT = predator.transform;
    }

    void Start()
    {
        //Testing Car
        // anim = GetComponent<Animator>();
        maxDistAway = edge.transform.position - center.transform.position;
        SlimeRb = GetComponent<Rigidbody>();
        indexMesh = index.GetComponent<MeshRenderer>();
        originalMaterial = index.GetComponent<MeshRenderer>().material;
        SpawnThing();
    }
    public void SpawnThing(){
        SpawnBottons();
        predator.transform.localPosition = new Vector3(Random.Range(-0.48f,-0.08f),0.8f,Random.Range(-0.48f,0.47f));
        // predator.transform.localRotation = PredoriginalRotation;
        this.transform.localPosition = new Vector3(Random.Range(-0.48f,-0.08f),1f,Random.Range(-0.48f,0.47f));
        this.transform.localRotation = SlimeoriginalRotation;
        // Debug.Log(Vector3.Distance(this.transform.localPosition , predator.transform.localPosition));
    }
    
    public void SpawnBottons(){
        float prev = Random.Range(7.2f,12.1f);
        float curr = Random.Range(0.24f,0.43f);
        OpenDoor.transform.localPosition = new Vector3(curr,0.8f,Random.Range(-0.45f,0.16f));
    }

    public override void OnEpisodeBegin()
    {
        SpawnThing();
    }

    bool canAttack(){
        return Vector3.SignedAngle(this.transform.forward,predator.transform.position - this.transform.position,this.transform.up)<12.0f;
    }
    float distance(){
        return Vector3.Distance(this.transform.position,predator.transform.position);
    }
    bool inRealDanger(){
        return distance()<4.0f;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        // sensor.AddObservation(this.transform.position);
        sensor.AddObservation(this.transform.localPosition.x);
        sensor.AddObservation(this.transform.localPosition.z);
        sensor.AddObservation(predator.transform.localPosition.x);
        sensor.AddObservation(predator.transform.localPosition.z);
        sensor.AddObservation(Door.transform.localPosition.x);
        sensor.AddObservation(Door.transform.localPosition.z);
        sensor.AddObservation(OpenDoor.transform.localPosition.x);
        sensor.AddObservation(OpenDoor.transform.localPosition.z);
        // Debug.Log(OpenDoor.transform.localPosition);
        // Debug.Log(OpenDoor.transform.position);
        foreach(GameObject wall in Walls){
            sensor.AddObservation(wall.transform.localPosition.x);
            sensor.AddObservation(wall.transform.localPosition.z);
        }
        foreach(GameObject tr in Triggers){
            sensor.AddObservation(tr.transform.localPosition.x);
            sensor.AddObservation(tr.transform.localPosition.z);
        }
        sensor.AddObservation(Door.GetComponent<WallSetup>().isGrounded?1:0);
        sensor.AddObservation(canAttack()?1:0);
        sensor.AddObservation(inRealDanger()?1:0);
    }

    public void SlimeRayCast(LayerMask targetMask){
      Ray ray = new Ray(LazerSource.transform.localPosition,LazerSource.transform.forward);
      RaycastHit hitInfo;
      if(Physics.Raycast(ray,out hitInfo,100,targetMask,QueryTriggerInteraction.Collide)){
        Debug.DrawLine(ray.origin,hitInfo.point,Color.green);
        // return hitInfo.collider.transform;
      }else{
         Debug.DrawLine(ray.origin,ray.origin + ray.direction * 1000,Color.red);
        //  return null;
      }
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
    MoveSlime(actions.ContinuousActions[0],Mathf.Clamp01(actions.ContinuousActions[1]));
     
     bool inDanger = actions.DiscreteActions[2]==1;
     bool canAttackPred = actions.DiscreteActions[3]==1;
       
    //  if(inDanger){
    //      Collider[] colliderArray = Physics.OverlapBox(transform.position,Vector3.one * 1.5f);
    //      foreach(Collider collider in colliderArray){
    //             if(collider.transform.TryGetComponent<Predator>(out Predator prf)){
    //                 if(canAttackPred){
    //                     indexMesh.material = winMaterial;
    //                     anim.SetTrigger("RunTr");
    //                     AddReward(2.5f);
    //                 }
    //             }
    //      }
    //  }

 
     
  }

   
    public void handleLazer(){
        string res = LazerSource.GetComponent<TestRayCast>().HitPointInfo();
        switch(res){
            case "wall": AddReward(0.0095f);break;
            case "twall": AddReward(-1.8f);break;
        }
        switch(res){
            case "wall": Debug.Log("Hitted the wall");break;
            case "twall":Debug.Log("Hitted the trigger"); AddReward(-0.8f);break;
        }
    }
    void FixedUpdate()
    {
        // handleLazer();
      
        // if(inRealDanger() == false){
        //     AddReward(1/MaxStep);
        // }else{
        //      AddReward(-1/MaxStep);
        // }

        // if(Input.GetKey(KeyCode.Space)){anim.SetTrigger("Sense");}
        // if(Input.GetKey(KeyCode.LeftShift))anim.SetTrigger("RunTr");
        //  Move();
        // Debug.Log(Mathf.RoundToInt(Input.GetAxisRaw("Vertical")));
    }

    void Move(){
        float mouseX = Input.GetAxis("Horizontal");
        float mouseZ = Input.GetAxis("Vertical");
        anim.SetFloat("Vertical",mouseZ);
        anim.SetFloat("Horizontal",mouseX);

         Vector3 Movement = this.transform.forward * mouseZ + this.transform.right * mouseX;

        this.transform.Translate(Movement*speed*Time.deltaTime,Space.Self);
        //  this.transform.Rotate(0,mouseX*speed*0.05f,0);
    }
     
     void OnCollisionEnter(Collision collision){
         if(collision.collider.transform.name == "Predator"){
             Debug.Log("Hunted");
             indexMesh.material = loseMaterial;
             AddReward(-1.0f);
            //  predator.GetComponent<Predator>().enabled = false;
             EndEpisode();
        }else if(collision.collider.transform.name == "OpenDoor"){
             Debug.Log("knock knock");
             indexMesh.material = winMaterial;
             AddReward(10.0f);
             EndEpisode();
        }
        else if(collision.collider.transform.CompareTag("MovingWall")){
             Debug.Log("Collided with moving Walls");
              indexMesh.material = HitWalMarMaterial;
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
             indexMesh.material = loseMaterial;
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
