using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject PlatForm;
    [SerializeField] GameObject Ball;
    
    MoveToTarget mv;
    Rigidbody BallRb;
    
    public bool isActive = true;
    private Vector3 StartingPos;
    void Start()
    {
        mv = GameObject.FindObjectOfType(typeof(MoveToTarget)) as MoveToTarget;
        StartingPos = mv.OnCreateTargetPos();
        BallRb = GetComponent<Rigidbody>();
    }
    
    public void ActivateBall(){
       Ball.SetActive(true);
       Ball.transform.localPosition = StartingPos;
    }

    public IEnumerator KillBall(){
        yield return new WaitForSeconds(2.0f);
        Ball.SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void LauncProjectile(){
       ActivateBall();
       Vector3 direction = (PlatForm.transform.position - this.transform.position);
       Vector3 AdjustedDirect = new Vector3(direction.x,direction.magnitude * Mathf.Cos(45f),direction.magnitude * Mathf.Sin(45f));
       Debug.DrawRay(this.transform.position,direction.normalized,Color.red,5.0f);
       BallRb.AddForce(AdjustedDirect * 15.0f,ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision){
        if(collision.collider.gameObject.name == "Cube"){
            KillBall();
        }
    }

}
