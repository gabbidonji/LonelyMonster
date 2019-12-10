using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationController : MonoBehaviour
{
    static Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("is_Walking", true);
        anim.SetBool("is_Punching", false);
        anim.SetBool("is_Dying", false);
        anim.SetBool("is_Being_Bitten", false);
        anim.SetBool("is_Being_Attacked", false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void walk()
    {
        anim.SetBool("is_Walking", true);

        anim.SetBool("is_Punching", false);
        anim.SetBool("is_Dying", false);
        anim.SetBool("is_Being_Bitten", false);
        anim.SetBool("is_Being_Attacked", false);
    }

    public void punch()
    {
        anim.SetBool("is_Punching", true);
        
        anim.SetBool("is_Walking", false);
        anim.SetBool("is_Dying", false);
        anim.SetBool("is_Being_Bitten", false);
        anim.SetBool("is_Being_Attacked", false);
    }

    public void die()
    {
        anim.SetBool("is_Dying", true);

        anim.SetBool("is_Walking", false);
        anim.SetBool("is_Punching", false);
        anim.SetBool("is_Being_Bitten", false);
        anim.SetBool("is_Being_Attacked", false);
    }

    public void getBitten()
    {
        anim.SetBool("is_Being_Bitten", true);

        anim.SetBool("is_Walking", false);
        anim.SetBool("is_Punching", false);
        anim.SetBool("is_Dying", false);
        anim.SetBool("is_Being_Attacked", false);
    }

    public void getAttacked()
    {
        anim.SetBool("is_Being_Attacked", true);

        anim.SetBool("is_Walking", false);
        anim.SetBool("is_Punching", false);
        anim.SetBool("is_Dying", false);
        anim.SetBool("is_Being_Bitten", false);
    }

    public void walkLeft()
    {
        anim.SetBool("isLeft", true);

        anim.SetBool("isIdle", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isDying", false);
        anim.SetBool("isFeeding", false);
        anim.SetBool("isBeingHit", false);
        anim.SetBool("isRight", false);
    }
}
