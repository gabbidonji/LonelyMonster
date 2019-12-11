using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AttackController : MonoBehaviour
{
    public AudioClip hit;
    public AudioSource asource;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Enemy" || c.gameObject.tag == "EnemyWithKey")
        {
            asource.PlayOneShot(hit);
            c.gameObject.GetComponent<Enemy1AI>().Hit();
            this.enabled = false;
            Debug.Log(UnityEngine.StackTraceUtility.ExtractStackTrace());
        }
    }
}
