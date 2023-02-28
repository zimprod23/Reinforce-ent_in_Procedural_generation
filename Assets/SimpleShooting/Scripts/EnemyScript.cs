using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] GameObject Player;
    // [SerializeField] ParticleSystem deathParticals;
    public int StartingHealth = 100;
    public int Currenthealth;
    Vector3 StartingPos;
    public float rotSpeed = 0.02f;
    public float moveSpeed = 15.0f;
    Rigidbody enemyRb;
     Vector3 Distance;
     NavMeshAgent agent;
    // Start is called before the first frame update
    
     void Start()
    {
        StartingPos = this.transform.localPosition;
        enemyRb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
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
    IEnumerator Die(){
        // deathParticals.transform.position = this.transform.position;
        // deathParticals.Play();
        // Destroy(this.gameObject);
        yield return new WaitForSeconds(1.5f);
        Debug.Log(message: "I died");
        this.transform.position = StartingPos;
        Respawn();
    }
    #region Debug
    private void OnMouseDown(){
       GetShot(StartingHealth);
    }
    #endregion
    void Respawn(){
     
        // Currenthealth = StartingHealth;
        // transform.localPosition = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Distance = Player.transform.localPosition - this.transform.localPosition;
        if(Distance.magnitude > 3)
        agent.SetDestination(Player.transform.position);
      
    }

    public void GotoPlayer(){
         agent.SetDestination(Player.transform.position);
    }
    void Move(Vector3 Distance){
       Quaternion rot = Quaternion.LookRotation(new Vector3(Distance.x,0,Distance.z));
       this.transform.localRotation = Quaternion.Slerp(this.transform.rotation,rot,Time.deltaTime*rotSpeed);
       enemyRb.velocity = this.transform.forward * moveSpeed  + new Vector3(0,enemyRb.velocity.y,0);
    }

    void OnCollisionEnter(Collision collision){
        if(collision.collider.transform.CompareTag("Bullet")){
            Debug.Log("jidoezhfoiezhfioezhfoiez");
           StartCoroutine(Die());
        }
    }
}
