using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedController : MonoBehaviour
{
    private PlayerController pc;
    // Start is called before the first frame update
    void Start()
    {
        Transform parentTransform = transform.parent.transform;
        pc = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.tag == "Enemy")
        {
            pc.StartFeeding(c.gameObject.GetComponent<Enemy1AI>());
        }
    }
}
