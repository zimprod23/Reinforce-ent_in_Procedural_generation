using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScooterBarController : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject ScooterBar;
    private GameObject HandBar;
    private float  speedRot = 0.02f;
    private GameObject Bar;
    Vector3 TargetPos;
    public bool openToogle = false;
    
    Vector3 stvect;
    void Start()
    {
        ScooterBar = GameObject.Find("upperpart");
        HandBar = GameObject.Find("handbar");
        Bar = GameObject.Find("bar");
        stvect = HandBar.transform.position - Bar.transform.position;
    }
    
    public void OpenBar(float latency){
       TargetPos = this.transform.position;
       Vector3 direction = TargetPos - Bar.transform.position;
       float angle = Vector3.Angle(Bar.transform.up,direction);
        Vector3 euler = new Vector3(0f,0f,360f - (float)angle * latency);
        if(angle >= 7f){
            ScooterBar.transform.Rotate(euler,Space.Self);
        }else{
            openToogle = false;
        }
       print(angle);
    }
    
    public void Toogle(){
        openToogle =! openToogle;
    }
    public void CloseBar(){

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            print(Bar.transform.localPosition + " | " + HandBar.transform.localPosition);
            Debug.DrawRay(Bar.transform.position,TargetPos - Bar.transform.position,Color.red,50);
            Debug.DrawRay(Bar.transform.position,Bar.transform.up,Color.green * 100,50);
            openToogle = true;
        }
        if(openToogle)OpenBar(Time.deltaTime);
    }
}

// Vector3 direction = target.transform.position - this.transform.position;
//         Quaternion lookRot = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
//         parentObj.transform.rotation = Quaternion.Slerp(parentObj.transform.rotation,lookRot,Time.deltaTime*speedRot);
//         float? angle = RotateTurrent();
//           if(angle != null && Vector3.Angle(direction,parentObj.transform.forward) < 10){
//             Fire();
//         }
