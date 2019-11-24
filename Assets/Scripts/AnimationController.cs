using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    static Animator anim;
    public enum Direction
    {
        BACKWARD, FORWARD, LEFT, RIGHT
    }
    public Direction dir;
    // Start is called before the first frame update
    void Start()
    {
        dir = Direction.FORWARD;
        anim = GetComponent<Animator>();
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFeeding", false);
        anim.SetBool("isIdle", true);
        anim.SetBool("isDying", false);
        anim.SetBool("isBeingHit", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void walk()
    {
        if (dir == Direction.FORWARD)
        {
            anim.speed = 1;
        }
        else
        {
            anim.speed = -1;
        }
        anim.SetBool("isWalking", true);

        anim.SetBool("isIdle", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFeeding", false);
        anim.SetBool("isDying", false);
        anim.SetBool("isBeingHit", false);
    }

    public void idle()
    {
        anim.SetBool("isIdle", true);

        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFeeding", false);
        anim.SetBool("isDying", false);
        anim.SetBool("isBeingHit", false);
    }

    public void attack()
    {
        anim.SetBool("isAttacking", true);

        anim.SetBool("isIdle", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isFeeding", false);
        anim.SetBool("isDying", false);
        anim.SetBool("isBeingHit", false);
    }

    public void feed()
    {
        anim.SetBool("isFeeding", true);

        anim.SetBool("isIdle", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isDying", false);
        anim.SetBool("isBeingHit", false);
    }

    public void die()
    {
        anim.SetBool("isDying", true);

        anim.SetBool("isIdle", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isBeingHit", false);
        anim.SetBool("isFeeding", false);
    }

    public void takeHit()
    {
        anim.SetBool("isBeingHit", true);

        anim.SetBool("isIdle", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isDying", false);
        anim.SetBool("isFeeding", false);
    }
}
