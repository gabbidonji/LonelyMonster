using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public KeyController key;
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
            if (c.gameObject.tag == "EnemyWithKey")
            {
                key.showKey();
            }
            Destroy(c.gameObject);
            
        }
    }
}
