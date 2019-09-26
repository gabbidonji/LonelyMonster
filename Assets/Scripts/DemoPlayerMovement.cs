using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoPlayerMovement : MonoBehaviour
{

    public float speed;
    public Material foundTex;
    private bool found = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!found){
            if(Input.GetKey(KeyCode.W)){
                transform.position += new Vector3(0,0,speed*Time.deltaTime);
            }
            if(Input.GetKey(KeyCode.A)){
                transform.position -= new Vector3(speed*Time.deltaTime,0,0);
            }
            if(Input.GetKey(KeyCode.S)){
                transform.position -= new Vector3(0,0,speed*Time.deltaTime);
            }
            if(Input.GetKey(KeyCode.D)){
                transform.position += new Vector3(speed*Time.deltaTime,0,0);
            }
        }
    }

    public void Found()
    {
        found = true;
        GetComponent<MeshRenderer>().material = foundTex;
    }
}
