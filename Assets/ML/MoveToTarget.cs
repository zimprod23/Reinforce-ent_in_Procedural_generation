using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;


public class MoveToTarget : Agent
{
    [SerializeField] Transform targetTransform;
    [SerializeField] Material winMat;
    [SerializeField] Material loseMat;
    [SerializeField] MeshRenderer floorMesh;

    [SerializeField] Material buttonMaterial;

    [SerializeField] GameObject target;
    [SerializeField] GameObject platForm;
    [SerializeField] GameObject child;
    Rigidbody Agentrb;
    Material originalMat;
    Vector3 originalButtTransform;
    private bool _isOccupied = false;
    GameObject Butt;
    Vector3 dimSize;
   public float speed = 15.0f;

    int []dim = {0,0};
    public bool isOccupied {
        get{
         return _isOccupied;
        }
        private set{
         _isOccupied = value;
        }
    }


    Vector3 originalPos ;//new Vector3(222.6548f,106.4f,-693.4116f);
    Quaternion originalrotation;
    void Awake(){
         originalPos = this.transform.localPosition;
         originalrotation = this.transform.localRotation;
         dimSize =  new Vector3(Random.Range(-6f,-1f),child.transform.localPosition.y,Random.Range(-2f,2f));
    }
    
    void Start(){
      Invoke("OnCreateObj",1.0f);
       Butt = GameObject.Find("Button");
       originalMat = Butt.GetComponent<MeshRenderer>().material;
       originalButtTransform = Butt.transform.localPosition;
       Agentrb = GetComponent<Rigidbody>();
    }

    public  void OnCreateObj(){
       child.SetActive(true);
       Butt.GetComponent<MeshRenderer>().material = originalMat;
       Butt.transform.localPosition = originalButtTransform;
       child.transform.localPosition = new Vector3(Random.Range(3.25f,4f),child.transform.localPosition.y,Random.Range(-2f,2f));
    }

   public void OnCreateTarget(){
       target.SetActive(true);
       target.transform.localPosition = new Vector3(Random.Range(-6.5f,4.5f),2,Random.Range(-2f,2f));
       if((target.transform.position - child.transform.position).magnitude < 3)OnCreateTarget();
   }

    public Vector3 OnCreateTargetPos(){
          return  new Vector3(Random.Range(-6.5f,4.5f),0,Random.Range(-2f,2.5f)) + dimSize;
    }
    public override void OnEpisodeBegin()
    {
      this.transform.localPosition =  new Vector3(Random.Range(1.5f,4.5f),child.transform.localPosition.y,Random.Range(-2f,2.5f));
      //   this.transform.localPosition = originalPos;
        this.transform.localRotation = originalrotation;
    }
    public override void CollectObservations(VectorSensor sensor){
       //  sensor.AddObservation(targetTransform.transform.localPosition);
       Vector3 dirToBut = (child.transform.localPosition - this.transform.localPosition).normalized;
         sensor.AddObservation(this.isOccupied?1:0);
         sensor.AddObservation(target.activeInHierarchy?1:0);
         sensor.AddObservation(dirToBut.x);
         sensor.AddObservation(dirToBut.z);
      if(target.activeInHierarchy){
         Vector3 dirToFood = (target.transform.localPosition - this.transform.localPosition).normalized;
         sensor.AddObservation(dirToFood.x);
         sensor.AddObservation(dirToFood.z);
      }else{
         sensor.AddObservation(0f);
         sensor.AddObservation(0f);
      }
    }

public override void Heuristic(in ActionBuffers actionsOut){
   //   ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
     ActionSegment<int> discretesActions = actionsOut.DiscreteActions;
     
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
   //   discretesActions[1] = Input.GetKey(KeyCode.M)?1:0; 
}

  public override void OnActionReceived(ActionBuffers actions){
     float moveX = actions.DiscreteActions[0];
     float moveZ = actions.DiscreteActions[1];
      Vector3 addForce = new Vector3(0,0,0);

      switch(moveX){
        case 0: addForce.x=0f;break;
        case 1: addForce.x=-1f;break;
        case 2: addForce.x=1f;break;
      }
      switch(moveZ){
        case 0: addForce.z=0f;break;
        case 1: addForce.z=1f;break;
        case 2: addForce.z=-1f;break;
      }

     float moveSpeed = 4.5f;
     Agentrb.velocity = addForce * moveSpeed + new Vector3(0,Agentrb.velocity.y,0);
   // Agentrb.AddForce(addForce * moveSpeed + new Vector3(0,Agentrb.velocity.y,0),ForceMode.Impulse);
     
     bool canUseButton = actions.DiscreteActions[2]==1;

   //   Debug.Log(actions.DiscreteActions[0] + " | " + actions.DiscreteActions[1] + " | " + isOccupied + " | "+actions.ContinuousActions[1]);
     
     if(canUseButton){
         Collider[] colliderArray = Physics.OverlapBox(transform.position,Vector3.one * .5f);
         foreach(Collider collider in colliderArray){
                if(collider.transform.TryGetComponent<PressToFeed>(out PressToFeed prf)){
                     if(isOccupied){
                        //   floorMesh.material = winMat;
                          AddReward(1.0f);
                          child.SetActive(false);
                          OnCreateTarget();
                        // OnCreateObj();
                     }
                }
         }
     }

   //   AddReward(-1f/MaxStep);
     
  }
 
   void OnTriggerEnter(Collider other){
      
 if(other.gameObject.transform.parent.gameObject.CompareTag("Button")){
            Debug.Log("We hitted the buttonn");
            Butt.transform.localPosition = new Vector3(Butt.transform.localPosition.x,Butt.transform.localPosition.y-0.5f,Butt.transform.localPosition.z);
            Butt.GetComponent<MeshRenderer>().material = buttonMaterial;
            Debug.Log(Butt.name);
            isOccupied = true;
    }

  }


void OnCollisionEnter(Collision collision){
   if(collision.collider.gameObject.name == "Sphere"){
        floorMesh.material = winMat;
        AddReward(4.5f);
        target.SetActive(false);
        OnCreateObj();
        EndEpisode();
     }else if(collision.collider.gameObject.CompareTag("Wall")){
      floorMesh.material = loseMat;
        AddReward(-1f);
        EndEpisode();
     }
}


  void OnTriggerExit(Collider other){
   if(other.gameObject.transform.parent.gameObject.CompareTag("Button")){
        Debug.Log("Just exited button looooooool");
      isOccupied = false;
   }
  }
 

 void FixedUpdate(){
  if(!isOccupied){
     Butt.GetComponent<MeshRenderer>().material = originalMat;
     Butt.transform.localPosition = originalButtTransform;
  }
   if(isOccupied && Input.GetKeyDown(KeyCode.Space)){
         OnCreateTarget();
  }
 }  
}



