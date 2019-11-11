using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float playerWorldRelationAngle = 0;

    private EnemyAI enemyAI;
    public float feedingTime;
    private float feedingTimer;
    public float hitInvincibilityTime;
    private float hitInvincibilityTimer;
    public float hitKnockback;
    public GameObject mesh;

    private Rigidbody rb;

    [SerializeField]
    private float speed;

    public Material foundTex;

    private PlayerState state;

    private AttackState aState;

    [SerializeField]
    private float attackDuration;
    private float attackTimer = 0;

    [SerializeField]
    public GameObject attackHitbox;

    public GameObject feedHitbox;

    private enum PlayerState{
        MOVING, HIT, FEEDING, DEAD
    }

    private enum AttackState {
        ATTACKING, NOTATTACKING
    }

    public enum RotationAxis
    {
        MOUSEX = 1
    }

    public RotationAxis axes = RotationAxis.MOUSEX;
    public float HorizontalSpeed = 100.0f;
    public float rotationX = 0;

    void Start()
    {
        feedingTimer = -1;
        state = PlayerState.MOVING;
        aState = AttackState.NOTATTACKING;
        hitInvincibilityTimer = -1;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
         float moveHorizontal = Input.GetAxis("Horizontal");
         float moveVertical = Input.GetAxis("Vertical");
         if (axes == RotationAxis.MOUSEX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * HorizontalSpeed, 0);
        }
         switch(state){
            case PlayerState.MOVING:
                Vector3 movement = new Vector3(moveVertical, 0.0f, -moveHorizontal);
                GetComponent<Rigidbody>().velocity = transform.TransformDirection(movement) * speed;
                switch(aState){
                    case AttackState.NOTATTACKING:
                        if(Input.GetKeyDown("space"))
                        {
                            attackHitbox.SetActive(true);
                            aState = AttackState.ATTACKING;
                            attackTimer = attackDuration;
                        }
                        if(Input.GetKeyDown(KeyCode.LeftShift)){
                            feedHitbox.SetActive(true);
                            aState = AttackState.ATTACKING;
                            attackTimer = attackDuration;
                        }
                        break;
                    case AttackState.ATTACKING:
                        attackTimer -= Time.deltaTime;
                        if(attackTimer < 0)
                        {
                            aState = AttackState.NOTATTACKING;
                            attackHitbox.SetActive(false);
                            feedHitbox.SetActive(false);
                        }
                        break;
                }
            break;
        case PlayerState.DEAD:
            GetComponent<Rigidbody>().velocity = new Vector3();
            break;
        case PlayerState.FEEDING:
            feedingTimer -= Time.deltaTime;
            if(feedingTimer < Time.deltaTime){
                state = PlayerState.MOVING;
                enemyAI.destroyEnemy();
            }
            if(Input.GetKeyUp(KeyCode.LeftShift)){
                state = PlayerState.MOVING;
            }
            break;
        case PlayerState.HIT:
            if(aState == AttackState.ATTACKING){
                aState = AttackState.NOTATTACKING;
                attackHitbox.SetActive(false);
            }
            hitInvincibilityTimer-=Time.deltaTime;
            if(hitInvincibilityTimer < 0){
                state = PlayerState.DEAD;
            }
            break;
         }
    }

    public void changeReferenceAngle(float angle)
    {
        playerWorldRelationAngle = angle%360;
    }

    public void Hit(Vector3 enemyPos)
    {
        if(state != PlayerState.HIT){
            if(state == PlayerState.FEEDING){
                enemyAI.stopFeeding();
            }
            state = PlayerState.HIT;
            GetComponent<BoxCollider>().enabled = false;
            mesh.GetComponent<MeshRenderer>().material = foundTex;
            Vector3 forceDir = (transform.position-enemyPos);
            forceDir[1] = 0;
            rb.AddForce(hitKnockback*forceDir.normalized);
            hitInvincibilityTimer = hitInvincibilityTime;
        }
    }

    public bool IsFound(){
        return state == PlayerState.DEAD;
    }

    public void StartFeeding(EnemyAI AI){
        if(state != PlayerState.FEEDING){
            state = PlayerState.FEEDING;
            feedingTimer = feedingTime;
            enemyAI = AI;
            enemyAI.startFeeding();
        }
    }
}
