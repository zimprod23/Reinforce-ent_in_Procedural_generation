using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicWallsMvm : MonoBehaviour
{
    // Start is called before the first frame update
    public bool switchOrientations=true;
    public float randomSpeed;
    public int orientationIndex = 1;
    public bool StartWallsMvm = false;
    BoxCollider WallCollider;
    public float lowerBound = -0.5f;
    public float upperBound = 0.5f;
  
    public Vector3 OriginalPosition;
    void Start()
    {
        WallCollider = GetComponent<BoxCollider>();
        OriginalPosition = this.transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       if(StartWallsMvm){
        MoveWalls();
       }else{
        this.transform.localPosition = OriginalPosition;
       }
    //    if(this.transform.localPosition.z >  this.transform.localScale.z * 3){
    //     Debug.Log("Out of Bound +++ ");
    //     switchOrientations = !switchOrientations;
    //    }else{
    //     Debug.Log("Out of Bound --- ");
    //     switchOrientations = !switchOrientations;
    //    }
    }
    void MoveWalls(){
         randomSpeed = Random.Range(2.7f,5.8f);
        orientationIndex = switchOrientations?1:-1;
        // if(this.transform.localPosition.z<lowerBound || this.transform.localPosition.z<upperBound){
        //    this.transform.Translate(transform.forward * Time.deltaTime * randomSpeed,Space.Self);    
        // }else if(this.transform.localPosition.z>lowerBound || this.transform.localPosition.z>upperBound){
        //      this.transform.Translate(-transform.forward * Time.deltaTime * randomSpeed,Space.Self);    
        // }
        this.transform.Translate(orientationIndex * transform.forward * Time.deltaTime * randomSpeed,Space.Self);  
    }
    
    void CheckIfItsOutSideTheBound(){
        if(this.transform.localPosition.z<lowerBound || this.transform.localPosition.z>upperBound){
            switchOrientations = !switchOrientations;
        }
    }
   
    private void OnTriggerExit(Collider other){
        if(other.gameObject.CompareTag("TriggerWall")){
            switchOrientations = !switchOrientations;
        }
    }

}
