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

    public void walk(GameObject obj)
    {
        anim = obj.GetComponentInChildren<Animator>();
        anim.SetBool("is_Walking", true);

        anim.SetBool("is_Punching", false);
        anim.SetBool("is_Dying", false);
        anim.SetBool("is_Being_Bitten", false);
        anim.SetBool("is_Being_Attacked", false);
    }

    public void punch(GameObject obj)
    {
        anim = obj.GetComponentInChildren<Animator>();
        anim.SetBool("is_Punching", true);
        
        anim.SetBool("is_Walking", false);
        anim.SetBool("is_Dying", false);
        anim.SetBool("is_Being_Bitten", false);
        anim.SetBool("is_Being_Attacked", false);
    }

    public void die(GameObject obj)
    {
        anim = obj.GetComponentInChildren<Animator>();
        anim.SetBool("is_Dying", true);

        anim.SetBool("is_Walking", false);
        anim.SetBool("is_Punching", false);
        anim.SetBool("is_Being_Bitten", false);
        anim.SetBool("is_Being_Attacked", false);
    }

    public void getBitten(GameObject obj)
    {
        anim = obj.GetComponentInChildren<Animator>();
        anim.SetBool("is_Being_Bitten", true);

        anim.SetBool("is_Walking", false);
        anim.SetBool("is_Punching", false);
        anim.SetBool("is_Dying", false);
        anim.SetBool("is_Being_Attacked", false);
    }

    public void getAttacked(GameObject obj)
    {
        anim = obj.GetComponentInChildren<Animator>();
        anim.SetBool("is_Being_Attacked", true);

        anim.SetBool("is_Walking", false);
        anim.SetBool("is_Punching", false);
        anim.SetBool("is_Dying", false);
        anim.SetBool("is_Being_Bitten", false);
    }
    
}
