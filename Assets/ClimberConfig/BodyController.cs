using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject[] LegsTarget;
     [SerializeField] GameObject[] legCubes;
    Vector3[] legsPositions;
    Vector3[] LegOriginalPosition;
    List<int> NextIndexToMove = new List<int>();
    List<int>  IndexMoving = new List<int>();
    float moveDistance = 3.0f;
    public int legMovementSmoothness = 5;
    void Start()
    {
        legsPositions = new Vector3[LegsTarget.Length];
        LegOriginalPosition = new Vector3[LegsTarget.Length];
        for(int i=0;i<LegsTarget.Length;i++){
           legsPositions[i] = LegsTarget[i].transform.position;
           LegOriginalPosition[i] = legsPositions[i];
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveLegs();
    }
    void MoveLegs(){
         for(int i=0;i<LegsTarget.Length;i++){
            if(Vector3.Distance(LegsTarget[i].transform.position,legCubes[i].transform.position)>= moveDistance){
                if(!NextIndexToMove.Contains(i) && !IndexMoving.Contains(i))NextIndexToMove.Add(i);
            }else if(!IndexMoving.Contains(i)){
                 LegsTarget[i].transform.position = LegOriginalPosition[i];
            }
        }
        if(NextIndexToMove.Count == 0 || IndexMoving.Count != 0)return;
        Vector3 targetPosition = legCubes[NextIndexToMove[0]].transform.position;
        StartCoroutine(step(NextIndexToMove[0],targetPosition));
    }
    IEnumerator step(int index,Vector3 moveTo){
        if(NextIndexToMove.Contains(index)) NextIndexToMove.Remove(index);
        if(!IndexMoving.Contains(index)) IndexMoving.Add(index);

        Vector3 startingPostion = LegOriginalPosition[index];

         for(int i=1;i<=legMovementSmoothness;i++){
          LegsTarget[index].transform.position = Vector3.Lerp(startingPostion,moveTo,i/legMovementSmoothness);
          yield return new WaitForFixedUpdate();
        }
        LegOriginalPosition[index] = moveTo;
        if(IndexMoving.Contains(index))IndexMoving.Remove(index);
    }
}
