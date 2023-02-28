using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarPanelController : MonoBehaviour
{
    public GameObject HydrolicArm;
    public GameObject Arm;
    public GameObject Panel;
    public GameObject TargetPos;
    public float mvspeed = 5.0f;
    private GameObject lightSource;
    bool open = false;
    
    bool canRotate = false;
    void Start(){
        lightSource = GameObject.Find("index");
    }
    public void OpenPanels(){
        open =! open;
        GameObject.Find("SolarPanel2").GetComponent<SolarPanelController>().open = open;
    }
    public void LaunchSolarPanel(float latency){
        Vector3 direction = TargetPos.transform.position - Arm.transform.position;
        if(direction.magnitude > 0.1)
           Arm.transform.localPosition += -Arm.transform.forward * latency * mvspeed;
        else canRotate = true;
        Debug.Log("Magnitude is : " + direction.magnitude);
    }
    public void FollowSun(float latency){
       Vector3 dirtolight = lightSource.transform.position - Arm.transform.position;
       float angle = Vector3.SignedAngle(-Arm.transform.forward,new Vector3(dirtolight.x,dirtolight.y,0),Vector3.forward);
        Vector3 euler = new Vector3(0f,0f,(float)Mathf.Clamp(angle,-10,10) * latency);
        Debug.DrawLine(Arm.transform.position,lightSource.transform.position,Color.red,50);
        Debug.DrawRay(Arm.transform.position,-Arm.transform.forward * 100,Color.green,50);
      //  Debug.Log(angle);
        // if(angle > 3 && angle < 30)
          Arm.transform.Rotate(euler,Space.World);
          Debug.Log("Angle is " + angle + "Clamped Angle is " + Mathf.Clamp(angle,-40,40));
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H)){
            open = true;
        }
        if(open){
            LaunchSolarPanel(Time.deltaTime);
            if(canRotate)
               FollowSun(Time.deltaTime);
            }
    }
}
