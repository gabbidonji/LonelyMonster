using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float playerWorldRelationAngle = 0;

    [SerializeField]
    private float speed;
    // Start is called before the first frame update

    public Material foundTex;

    private bool found = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        if (!found)
        {
            transform.rotation = Quaternion.Euler(0,playerWorldRelationAngle,0);

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            GetComponent<Rigidbody>().velocity = transform.TransformDirection(movement) * speed;
        } else {
            GetComponent<Rigidbody>().velocity = new Vector3();
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            transform.rotation = Quaternion.LookRotation(movement);
        }
    }

    public void changeReferenceAngle(float angle)
    {
        playerWorldRelationAngle = angle%360;
    }

    public void Found()
    {
        found = true;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<MeshRenderer>().material = foundTex;
    }
}
