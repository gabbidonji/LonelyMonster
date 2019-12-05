using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public float attackPredelay;

    public float attackDuration;

    public float attackPostdelay;

    public float feedPredelay;

    public float feedDuration;

    public float feedPostdelay;

    private float attackTimer = 0;

    [SerializeField]
    public GameObject attackHitbox;

    public GameObject feedHitbox;

    public KeyController key;
    public Text dialogue;

    private enum PlayerState{
        MOVING, HIT, FEEDING, DEAD
    }

    private enum AttackState {
        PREDELAY, POSTDELAY, ATTACKING, NOTATTACKING
    }

    private bool isFeeding;

    public enum RotationAxis
    {
        MOUSEX = 1
    }

    public RotationAxis axes = RotationAxis.MOUSEX;
    public float HorizontalSpeed = 100.0f;
    public float rotationX = 0;
    private AnimationController anim;

    private float oldPositionHoriz;
    private float oldPositionVert;

    public Slider healthSlider;
    private float currentHealth = 5f;

    public Slider feedSlider;
    private float currentFeed = 20f;

    void Start()
    {
        feedingTimer = -1;
        state = PlayerState.MOVING;
        aState = AttackState.NOTATTACKING;
        hitInvincibilityTimer = -1;
        rb = GetComponent<Rigidbody>();
        oldPositionHoriz = Input.GetAxis("Horizontal");
        oldPositionVert = Input.GetAxis("Vertical");
        InvokeRepeating("hunger", 2.0f, 2f);
        anim = FindObjectOfType<AnimationController>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        if ((axes == RotationAxis.MOUSEX) && (state != PlayerState.DEAD))
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * HorizontalSpeed, 0);
        }
        if (oldPositionHoriz != moveHorizontal || oldPositionVert != moveVertical) // walking
        {
            if (oldPositionVert > moveVertical)
            {
                anim.dir = AnimationController.Direction.BACKWARD;
                anim.walk();
            }
            else if (oldPositionVert < moveVertical)
            {
                anim.dir = AnimationController.Direction.FORWARD;
                anim.walk();
            }
            else if (oldPositionHoriz > moveHorizontal){
                anim.walkLeft();
            }
            else
            {
                anim.walkRight();
            }
        } else
        {
            anim.idle();
        }
        switch (state){
            case PlayerState.MOVING:
                switch(aState){
                    case AttackState.NOTATTACKING:
                        Vector3 movement = new Vector3(moveVertical, 0.0f, -moveHorizontal);
                        GetComponent<Rigidbody>().velocity = transform.TransformDirection(movement) * speed;
                        if (Input.GetKeyDown(KeyCode.Mouse0)) // left click (attack)
                        {
                            isFeeding = false;
                            anim.attack();
                            //attackHitbox.SetActive(true);
                            aState = AttackState.PREDELAY;
                            attackTimer = attackPredelay;
                        }
                        if(Input.GetKeyDown(KeyCode.Mouse1)){ // right click (feed)
                            isFeeding = true;
                            anim.feed();
                            currentFeed = 25;
                            //feedHitbox.SetActive(true);
                            aState = AttackState.PREDELAY;
                            attackTimer = feedPredelay;
                        }
                        break;
                    case AttackState.PREDELAY:
                        GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
                        attackTimer -= Time.deltaTime;
                        if(attackTimer < 0)
                        {
                            if(isFeeding){
                                feedHitbox.SetActive(true);
                                attackTimer = feedDuration;
                            } else {
                                attackHitbox.SetActive(true);
                                attackTimer = attackDuration;
                            }
                            aState = AttackState.ATTACKING;
                        }
                        break;
                    case AttackState.ATTACKING:
                        GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
                        attackTimer -= Time.deltaTime;
                        if(attackTimer < 0){
                            if(isFeeding){
                                attackTimer = feedPostdelay;
                            } else {
                                attackTimer = attackPostdelay;
                            }
                            aState = AttackState.POSTDELAY;
                            feedHitbox.SetActive(false);
                            attackHitbox.SetActive(false);
                        }
                        break;
                    case AttackState.POSTDELAY:
                        GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
                        attackTimer -= Time.deltaTime;
                        if(attackTimer < 0)
                        {
                            aState = AttackState.NOTATTACKING;
                        }
                        break;
                }
            break;
        case PlayerState.DEAD:
                death();
            break;
        case PlayerState.FEEDING:
                feedingTimer -= Time.deltaTime;
            if(feedingTimer < Time.deltaTime){
                state = PlayerState.MOVING;
                enemyAI.DestroyEnemy();
            }
            if(Input.GetKeyDown(KeyCode.Mouse1)){
                enemyAI.StopFeeding();
                state = PlayerState.MOVING;
            }
            break;
        case PlayerState.HIT:
            anim.takeHit();
            if (aState == AttackState.ATTACKING){
                aState = AttackState.NOTATTACKING;
                attackHitbox.SetActive(false);
                feedHitbox.SetActive(false);
            }
            hitInvincibilityTimer-=Time.deltaTime;
            if(hitInvincibilityTimer < 0){
                    if (currentHealth <= 0)
                    {
                        state = PlayerState.DEAD;
                    } else
                    {
                        GetComponent<BoxCollider>().enabled = true;
                        state = PlayerState.MOVING;
                    }
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
        if (state != PlayerState.HIT){
            if(state == PlayerState.FEEDING){
                enemyAI.StopFeeding();
            }
            state = PlayerState.HIT;
            decreaseHealth();
            currentHealth = healthSlider.value;

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
        if((state != PlayerState.FEEDING) && !(AI.SeesPlayer())){
            state = PlayerState.FEEDING;
            feedingTimer = feedingTime;
            enemyAI = AI;
            enemyAI.StartFeeding();
        }
    }

    void decreaseHealth()
    {
        healthSlider.value = currentHealth - 1;
    }

    private void death()
    {
        anim.die();
        GetComponent<Rigidbody>().velocity = new Vector3();
        Time.timeScale = 0.5f;
        //yield return new WaitForSeconds(2f);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
    }

    void hunger()
    {
        currentFeed -= 1;
        feedSlider.value = currentFeed;
        if (currentFeed == 0)
        {
            anim.die();
            death(); // not working
        }
    }

        void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            if (key.isActiveAndEnabled)
            {
                dialogue.text = "The gate is unlocked";
            }
            else
            {
                dialogue.text = "Find the key!";
            }
        }
    }
}
