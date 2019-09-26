using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float rotationSpeed;

    private bool rotating;
    
    [SerializeField]
    private bool positiveZ;

    void Start()
    {
        rotating = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(rotating){
            Quaternion targetRotation = Quaternion.Euler(0,positiveZ ? 0:180 ,0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed*Time.deltaTime);
            if(transform.rotation.Equals(targetRotation)){
                rotating = false;
            }
        } else {
            if((this.transform.position.z > 7 && positiveZ) || (this.transform.position.z < -7 && !positiveZ)){
                rotating = true;
                positiveZ = !positiveZ;
            }
            else{
                Debug.Log("here");
                transform.position += new Vector3(0,0,(positiveZ? 1 : -1)*walkSpeed*Time.deltaTime);
            }
        }
    }
}
