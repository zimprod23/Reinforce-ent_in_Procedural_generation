using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody PredRB;
    [SerializeField] GameObject Prey;
    public float moveSpeed = 3.0f;
    public float rotspeed = 5.0f;
    float stopSmoothness = 1;
    public float minVision = 20.0f;
    void Start()
    {
        PredRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
       
        Vector3 dirToPrey = Prey.transform.position - this.transform.position;
        //  Debug.Log("Magnitude" + dirToPrey.magnitude);
        if(dirToPrey.magnitude < minVision){
            Move(dirToPrey);
            // Debug.Log("Detected");
            // if(dirToPrey.magnitude < 1.5){
            //     PredRB.velocity = Vector3.zero;
            // }
        }
    }
   
   void Rotate(Vector3 dirToPrey){
       Quaternion lookRot = Quaternion.LookRotation(new Vector3(dirToPrey.x,0,dirToPrey.z));
       this.transform.localRotation = Quaternion.Slerp(this.transform.rotation,lookRot,Time.deltaTime*rotspeed);
    //     Vector3 Tup = transform.forward;
    //     Debug.Log("Angle is :: "+Vector3.SignedAngle(Tup,dirToPrey,transform.up));
    //    this.transform.Rotate(new Vector3(0,Vector3.SignedAngle(Tup,dirToPrey,transform.up) * 0.02f * rotspeed,0));

   }
    void Move(Vector3 dirToPrey){
        Rotate(dirToPrey);
        Vector3 Tup = transform.forward;
        // Debug.Log("Angle is :: "+Vector3.SignedAngle(Tup,dirToPrey,transform.up));
         PredRB.velocity = transform.forward * moveSpeed  + new Vector3(0,PredRB.velocity.y,0);
        // PredRB.AddForce(transform.forward * moveSpeed  + new Vector3(0,PredRB.velocity.y,0),ForceMode.Acceleration);
        //  this.transform.Translate(transform.forward * moveSpeed * Time.deltaTime,Space.Self);

    }

   void OnCollisionEnter(Collision collision){
       if(collision.collider.transform.CompareTag("Slime")){
        PredRB.velocity = Vector3.zero;
        // GameObject.Destroy(Prey);
        // Debug.Log("Hitted The target");
       }
   }

}
