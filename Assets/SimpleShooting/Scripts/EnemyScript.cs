using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] GameObject Player;
    public int StartingHealth = 100;
    public int Currenthealth;
    Vector3 StartingPos;
    public float rotSpeed = 0.02f;
    // Start is called before the first frame update
    
     void Start()
    {
        StartingPos = this.transform.localPosition;
    }


    public void GetShot(int damage){
        ApplyDamage(damage);
    }
    private void ApplyDamage(int damage){
        Currenthealth -= damage;
        if(Currenthealth<=0){
            Die();
        }
    }
    void Die(){
        Debug.Log(message: "I died");
        Respawn();
    }
    #region Debug
    private void OnMouseDown(){
       GetShot(StartingHealth);
    }
    #endregion
    void Respawn(){
        Currenthealth = StartingHealth;
        transform.localPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }











    void Move(){
       Vector3 Distance = Player.transform.position - this.transform.position;
       Quaternion rot = Quaternion.LookRotation(new Vector3(Distance.x,0,Distance.z));
       this.transform.rotation = Quaternion.Slerp(this.transform.rotation,rot,Time.deltaTime*rotSpeed);
       if(Distance.magnitude > 3)
       this.transform.Translate(Distance.normalized * 15 * Time.deltaTime);
    }
     float RotationAngle(){
       Vector3 DistanceVect = Player.transform.position - this.transform.position;
       Vector3 FrVect = this.transform.forward;
    
       float AdjustAngle = Vector3.SignedAngle(FrVect,DistanceVect,Vector3.up);

       return AdjustAngle;
    }
}
