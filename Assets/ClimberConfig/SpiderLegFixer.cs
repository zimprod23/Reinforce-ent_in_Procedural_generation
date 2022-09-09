using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderLegFixer : MonoBehaviour
{
    Vector3 OriginalPos;
    public GameObject moveCube;
    void Start()
    {
        OriginalPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        // this.transform.position = OriginalPos;
        // float distanceToMove = Vector3.Distance(transform.position,moveCube.transform.position);
        // if(distanceToMove>= 0.7f){
        //     // transform.position = Vector3.LerpUnclamped()
        // }
    }
}
