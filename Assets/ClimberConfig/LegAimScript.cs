using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegAimScript : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject RaycastOrigin;
    int layerMask;
    void Start()
    {
        layerMask = LayerMask.GetMask("Ground");
        RaycastOrigin = this.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(RaycastOrigin.transform.position,-transform.up,out hit,Mathf.Infinity,layerMask)){
            Debug.Log(hit.transform.name);
            this.transform.position = hit.point;
        }
        // RaycastHit hit; //simple raycast downwards
        // if(Physics.Raycast(raycastOrigin.transform.position, -transform.up, out hit, Mathf.Infinity, layerMask))
        // {
        //     transform.position = hit.point + new Vector3(0f, 0.3f, 0f); //move the cube to the ground
        // }
    }
}
