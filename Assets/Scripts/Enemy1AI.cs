using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1AI : MonoBehaviour
{

    public GameObject player;
    private Transform player_tr;
    private Vector3 lastSeenPlayerPos;

    private EnemyVision vision;

    public List<Vector3> positions;
    private int targetPosIndex;

    private NavMeshAgent nav;

    private EnemyState state;
    public float rotationSpeed;
    public float movementSpeed;

    private bool checking;
    private Vector3 checkCenterAngle;
    private bool checkedLeft;
    private bool checkedRight;

    enum EnemyState {
        PATROL, PURSUE, CHECK, RETURN 
    }

    // Start is called before the first frame update
    void Start()
    {
        state = EnemyState.PATROL;
        nav = GetComponent<NavMeshAgent>();
        nav.destination = positions[0];
        vision = GetComponent<EnemyVision>();
        player_tr = player.transform;
        targetPosIndex = 0;
        checkedRight = false;
        checkedLeft = false;
        checking = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state){
            case EnemyState.PATROL:
                /*
                if(vision.SeesPlayer()){
                    state = EnemyState.PURSUE;
                } else {
                    if(FaceDirection(positions[targetPosIndex])){
                        //Debug.Log("here1");
                        if(MoveTowards(positions[targetPosIndex])){
                            //Debug.Log("here2");
                            targetPosIndex = (targetPosIndex + 1)%positions.Count;
                        }
                    }
                }
                */
                if(vision.SeesPlayer()){
                    vision.FollowingPlayer();
                    state = EnemyState.PURSUE;
                } else {
                    if(nav.remainingDistance < 0.001){
                        targetPosIndex = (targetPosIndex + 1)%positions.Count;
                        nav.destination = positions[targetPosIndex];
                    }
                }
                break;
            case EnemyState.PURSUE:
                if(vision.SeesPlayer()){
                    nav.destination = player_tr.position;
                    lastSeenPlayerPos = player_tr.position;
                } else {
                    nav.destination = lastSeenPlayerPos;
                    if(nav.remainingDistance < 0.001) {
                        nav.enabled = false;
                        state = EnemyState.CHECK;
                        checkCenterAngle = transform.rotation.eulerAngles;
                    }
                }
                break;
            case EnemyState.CHECK:
                Debug.Log(checkCenterAngle);
                if(!checkedLeft){
                    Vector3 leftRotation = checkCenterAngle;
                    leftRotation.y = leftRotation.y - 85;
                    Debug.Log("Left: " + leftRotation);
                    checkedLeft = RotateTowards(Mathf.Deg2Rad*leftRotation);
                } else if (!checkedRight){
                    Vector3 rightRotation = checkCenterAngle;
                    rightRotation.y = rightRotation.y + 85;
                    Debug.Log("Right: " + rightRotation);
                    checkedRight = RotateTowards(Mathf.Deg2Rad*rightRotation);
                } else {
                    checkedLeft = false;
                    checkedRight = false;
                    nav.enabled = true;
                    targetPosIndex = FindClosestPathPoint();
                    vision.NotFollowingPlayer();
                    state = EnemyState.PATROL;
                }
                break;
            case EnemyState.RETURN:
                break;
        }
    }


    int FindClosestPathPoint(){
        int minIndex = 0;
        for(int i = 0; i < positions.Count; i++){
            Vector3 position = positions[i];
            if((position - transform.position).magnitude 
            < (positions[minIndex] - transform.position).magnitude){
                minIndex = i;
            }
        }
        return minIndex;
    }
    bool FaceDirection(Vector3 position){
        Vector3 difference = position - transform.position;
        //Debug.Log(difference);
        //Debug.Log(Mathf.Atan2(difference.x, difference.z));
        Vector3 targetRotation = new Vector3(0,Mathf.Atan2(difference.x, difference.z),0);
        //Debug.Log(targetRotation);
        return RotateTowards(targetRotation);
    }


    //targetRotation must be in Degrees
    bool RotateTowards(Vector3 targetRotation){
        Quaternion targetRotationQuat = Quaternion.Euler(Mathf.Rad2Deg*targetRotation);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotationQuat, rotationSpeed * Time.deltaTime);
        if (
            (Mathf.Abs(transform.rotation.eulerAngles.y - Mathf.Rad2Deg*targetRotation.y)%360 < 0.001) ||
            (Mathf.Abs(transform.rotation.eulerAngles.y - (Mathf.Rad2Deg*targetRotation.y+360))%360 < 0.001))
        {
            transform.rotation = targetRotationQuat;
            return true;
        }
        return false;
    }

    bool MoveTowards(Vector3 position){
        Vector3 difference = position - transform.position;
        Vector3 translation = movementSpeed * difference.normalized * Time.deltaTime;
        if(difference.magnitude <= translation.magnitude){
            transform.position = position;
            return true;
        } else {
            transform.Translate(transform.InverseTransformDirection(translation));
            return false;
        }
    }
}
