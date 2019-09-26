using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisionTest : MonoBehaviour
{
    public bool enter = true;

    void Awake()
    {
        // add isTrigger
        var boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (enter)
        {
            Debug.Log("entered");
        }
        Destroy(other.gameObject);
    }
    
}