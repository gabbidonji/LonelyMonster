using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveTest : MonoBehaviour
{
    private float walkSpeed;
    //private float rotationSpeed;
    private bool rotating;

    [SerializeField]
    private bool positiveX;
    // Start is called before the first frame update
    void Start()
    {
        walkSpeed = 7;
        //rotationSpeed = 5;
        rotating = false;
}

    // Update is called once per frame
    void Update()
    {
        if (rotating)
        {
            Quaternion targetRotation = Quaternion.Euler(0, positiveX ? 180 : 0, 0);
            while (!transform.rotation.Equals(targetRotation))
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime);
            }
            rotating = false;
        }
        else
        {
            if ((this.transform.position.x > 17 && positiveX) || (this.transform.position.x < -17 && !positiveX))
            {
                Debug.Log("walk left");
                rotating = true;
                positiveX = !positiveX;
            }
            else
            {
                //Debug.Log("walk right");
                transform.position += new Vector3((positiveX ? 1 : -1) * walkSpeed * Time.deltaTime, 0, 0);
            }
        }
    } 
}
