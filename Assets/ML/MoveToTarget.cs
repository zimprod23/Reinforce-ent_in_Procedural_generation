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
         dimSize = new Vector3(platForm.GetComponent<BoxCollider>().size.x *4,0,platForm.GetComponent<BoxCollider>().size.z*4);
    }
    
    void Start(){
      Invoke("OnCreateObj",1.0f);
       Butt = GameObject.Find("Button");
       originalMat = Butt.GetComponent<MeshRenderer>().material;
       originalButtTransform = Butt.transform.localPosition;
    }

    public  void OnCreateObj(){
       child.SetActive(true);
       child.transform.localPosition = new Vector3(Random.Range(3f,4.5f),child.transform.localPosition.y,Random.Range(-2f,2.5f));
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
        this.transform.localPosition = originalPos;
        this.transform.localRotation = originalrotation;
    }
    public override void CollectObservations(VectorSensor sensor){
       //  sensor.AddObservation(targetTransform.transform.localPosition);
         sensor.AddObservation(this.transform.localPosition);
         if(target.activeInHierarchy){
         sensor.AddObservation(targetTransform.transform.localPosition);
      }else{
         sensor.AddObservation(Vector3.zero);
      }
      sensor.AddObservation(this.isOccupied?1:0);
      sensor.AddObservation(target.activeInHierarchy?1:0);
    }

public override void Heuristic(in ActionBuffers actionsOut){
     ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
     ActionSegment<int> discretesActions = actionsOut.DiscreteActions;
     continuousActions[0] = Input.GetAxis("Horizontal");
     continuousActions[1] = Input.GetAxis("Vertical");
     discretesActions[0] = Input.GetKey(KeyCode.E)?1:0; 
     discretesActions[1] = Input.GetKey(KeyCode.M)?1:0; 
}

  public override void OnActionReceived(ActionBuffers actions){
     float moveX = actions.ContinuousActions[0];
     float moveZ = actions.ContinuousActions[1];

     transform.localPosition += new Vector3(moveZ,0,-moveX) * speed * Time.deltaTime;
     bool useButton = actions.DiscreteActions[0] == 1;
     Debug.Log(actions.DiscreteActions[0] + " | " + actions.DiscreteActions[1] + " | " + isOccupied + " | "+actions.ContinuousActions[1]);
     if(isOccupied){ 
           if(useButton){
            //   OnCreateTarget();
            floorMesh.material = winMat;
            AddReward(1.0f);
            target.SetActive(false);
            OnCreateObj();
           }
     }
     AddReward(-1f/1000);
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
        AddReward(1.5f);
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
//   if(Input.GetKeyDown(KeyCode.Space)){
//     target.GetComponent<Rigidbody>().AddForce(Vector3.up * 5,ForceMode.Impulse);
//   }
  if(!isOccupied){Butt.GetComponent<MeshRenderer>().material = originalMat;
     Butt.transform.localPosition = originalButtTransform;
  }
  else if(isOccupied && Input.GetKeyDown(KeyCode.Space)){
         OnCreateTarget();
  }

  if(Input.GetKey(KeyCode.A)){

  }
 }
   
}



