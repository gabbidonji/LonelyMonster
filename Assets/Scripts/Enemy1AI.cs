using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1AI : MonoBehaviour, EnemyAI
{

    public GameObject player;
    private Transform player_tr;
    private PlayerController player_ctr;
    private Vector3 lastSeenPlayerPos;

    public Light flashlight;
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

    public float attackTriggerDist;
    public float attackPredelay;
    public float attackLength;
    public float attackPostdelay;
    private float attackTimer;
    public GameObject attackHitbox;

    private AttackState attackState;

    private Rigidbody rb;

    private bool turning;

    public int hp;
    private NPCAnimationController anim;
    enum AttackState {
        NOT_ATTACKING, PREDELAY, ATTACKING, POSTDELAY
    }

    enum EnemyState {
        PATROL, PURSUE, CHECK, IMMOBILE, HIT,  DEAD
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        attackState = AttackState.NOT_ATTACKING;
        state = EnemyState.PATROL;
        nav = GetComponent<NavMeshAgent>();
        nav.destination = positions[0];
        vision = GetComponent<EnemyVision>();
        player_tr = player.transform;
        player_ctr = player.GetComponent<PlayerController>();
        targetPosIndex = 0;
        checkedRight = false;
        checkedLeft = false;
        checking = false;
        turning = true;
        anim = GetComponentInChildren<NPCAnimationController>();
    }

    void Update()
    {
        switch(state){
            case EnemyState.PATROL:
                anim.walk(gameObject);
                vision.NotFollowingPlayer();
                if(vision.SeesPlayer() && !player_ctr.IsFound()){
                    vision.FollowingPlayer();
                    state = EnemyState.PURSUE;
                } else {
                    if(!turning){
                        nav.enabled = true;
                        if(nav.remainingDistance < 0.001){
                            turning = true;
                            targetPosIndex = (targetPosIndex + 1)%positions.Count;
                            nav.destination = positions[targetPosIndex];
                        }
                    } else {
                        nav.enabled = false;
                        if(FaceDirection(positions[targetPosIndex])){
                            turning = false;
                            nav.enabled = true;
                            nav.destination = positions[targetPosIndex];
                        }
                    }
                }
                break;
            case EnemyState.PURSUE:
                anim.walk(gameObject);
                switch (attackState){
                    case AttackState.NOT_ATTACKING:
                        anim.walk(gameObject);
                        nav.enabled = true;
                        if(vision.SeesPlayer()){
                            nav.destination = player_tr.position;
                            lastSeenPlayerPos = player_tr.position;
                            if(nav.remainingDistance < attackTriggerDist){
                                nav.enabled = false;
                                attackTimer = attackPredelay;
                                attackState = AttackState.PREDELAY;
                            }
                        } else {
                            nav.destination = lastSeenPlayerPos;
                            if(nav.remainingDistance < 0.001) {
                                nav.enabled = false;
                                state = EnemyState.CHECK;
                                checkCenterAngle = transform.rotation.eulerAngles;
                            }
                        }
                        break;
                    case AttackState.PREDELAY:
                        if (attackTimer < 0){
                            attackTimer = attackLength;
                            attackState = AttackState.ATTACKING;
                            attackHitbox.SetActive(true);
                        } else {
                            attackTimer -= Time.deltaTime;
                        }
                        break;
                    case AttackState.ATTACKING:
                        anim.punch(gameObject);
                        if (attackTimer < 0){
                            attackHitbox.SetActive(false);
                            attackTimer = attackPostdelay;
                            attackState = AttackState.POSTDELAY;
                        } else {
                            attackTimer -= Time.deltaTime;
                        }
                        break;
                    case AttackState.POSTDELAY:
                        anim.walk(gameObject);
                        if (attackTimer < 0){
                            nav.enabled = true;
                            nav.destination = lastSeenPlayerPos;
                            attackState = AttackState.NOT_ATTACKING;
                            if(player_ctr.IsFound()){
                                targetPosIndex = FindClosestPathPoint();
                                state = EnemyState.PATROL;
                                turning = true;
                            }
                        } else {
                            attackTimer -= Time.deltaTime;
                        }
                        break;

                }
                break;
            case EnemyState.CHECK:
                if(!checkedLeft){
                    Vector3 leftRotation = checkCenterAngle;
                    leftRotation.y = leftRotation.y - 85;
                    checkedLeft = RotateTowards(Mathf.Deg2Rad*leftRotation);
                } else if (!checkedRight){
                    Vector3 rightRotation = checkCenterAngle;
                    rightRotation.y = rightRotation.y + 85;
                    checkedRight = RotateTowards(Mathf.Deg2Rad*rightRotation);
                } else {
                    checkedLeft = false;
                    checkedRight = false;
                    nav.enabled = true;
                    targetPosIndex = FindClosestPathPoint();
                    vision.NotFollowingPlayer();
                    state = EnemyState.PATROL;
                    nav.destination = positions[targetPosIndex];
                    turning = false;
                }
                break;
            case EnemyState.IMMOBILE:
                anim.getBitten(gameObject);
                flashlight.gameObject.SetActive(false);
                break;
            case EnemyState.DEAD:
                nav.enabled = false;
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
        Vector3 targetRotation = new Vector3(0,Mathf.Atan2(difference.x, difference.z),0);
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

    public void StartFeeding(){
        if(state != EnemyState.PURSUE){
            nav.enabled = false;
            state = EnemyState.IMMOBILE;
        }
    }

    public void StopFeeding(){
        if(state == EnemyState.IMMOBILE){
            nav.enabled = true;
            state = EnemyState.PURSUE;
        }
    }

    public void DestroyEnemy(){
        state = EnemyState.DEAD;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        GetComponent<BoxCollider>().enabled = false;
        foreach(BoxCollider bc in GetComponentsInChildren<BoxCollider>()){
            bc.enabled = false;
        }
    }

    public bool SeesPlayer(){
        return vision.SeesPlayer();
    }

    public void Hit()
    {
        Vector3 playerPos = player.transform.position;
        hp -= 1;
        if(hp <= 0){
            DestroyEnemy();
            anim.die(gameObject);
            flashlight.gameObject.SetActive(false);
        }
        //rb.velocity = new Vector3(0,0,0);
        // GetComponent<BoxCollider>().enabled = false;
        //Vector3 forceDir = (transform.position-playerPos);
        //forceDir[1] = 0;
        //rb.AddForce(200*forceDir.normalized);
    }
}
