using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    public AudioClip hit;
    public AudioSource asource;
    Transform parent_tr;
    // Start is called before the first frame update
    void Start()
    {
        parent_tr = transform.parent.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.tag == "Player")
        {
            c.gameObject.GetComponent<PlayerController>().Hit(parent_tr.position);
            asource.PlayOneShot(hit);
        }
    }
}
