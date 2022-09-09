using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    // Start is called before the first frame update
    private CharacterController playerController;
    public Camera mainCam;
    public float speed = 12f;
    public float turnSmooth = 0.1f;
    private float smoothVelocity;
    public LayerMask mask;
    public Transform pistole;
    public GameObject bulletPrefab;

    // Update is called once per frame
    void Start(){
        playerController = this.GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal,0,vertical).normalized;
        
        if(direction.magnitude >= 0.1){
            float targetAngle = Mathf.Atan2(direction.x,direction.z) * Mathf.Rad2Deg + mainCam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(this.transform.eulerAngles.y,targetAngle,ref smoothVelocity,turnSmooth);

            this.transform.rotation = Quaternion.Euler(0f,angle,0f);
            
            Vector3 MoveDirection = Quaternion.Euler(0f,targetAngle,0f) * Vector3.forward;

            playerController.Move(MoveDirection.normalized * speed * Time.deltaTime);
        }
        Shoot(pistole,mask);

    }

    public void Shoot(Transform source,LayerMask targetMask){
        RayPrecision(source,targetMask);
        if(Input.GetButtonDown("Fire1")){
            // Debug.Log("Target shot");
            GameObject bullet = (GameObject)Instantiate(bulletPrefab,pistole.position,Quaternion.identity); 
        }
        if(Input.GetButton("Fire2")){
            // Debug.Log("Target shot");
            GameObject bullet = (GameObject)Instantiate(bulletPrefab,pistole.position,Quaternion.identity); 
        }
    }

    public void RayPrecision(Transform source,LayerMask targetMask){
       Ray src = new Ray(source.position,source.forward);
       RaycastHit hit;

       if(Physics.Raycast(src,out hit,100,targetMask)){
          Debug.DrawLine(src.origin,hit.point,Color.red);
       }else{
        Debug.DrawLine(src.origin,src.origin + src.direction * 100,Color.green);
       }

    }
}
