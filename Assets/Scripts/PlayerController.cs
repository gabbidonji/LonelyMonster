using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float playerWorldRelationAngle = 0;

    public GameObject mesh;

    [SerializeField]
    private float speed;
    // Start is called before the first frame update

    public Material foundTex;

    private bool found = false;

    private bool attacking = false;

    [SerializeField]
    private float attackDuration;
    private float attackTimer = 0;

    [SerializeField]
    GameObject attackHitbox;

    public enum RotationAxis
    {
        MOUSEX = 1
    }

    public RotationAxis axes = RotationAxis.MOUSEX;
    public float HorizontalSpeed = 100.0f;
    public float rotationX = 0;

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
             Vector3 movement = new Vector3(moveVertical, 0.0f, -moveHorizontal);
             GetComponent<Rigidbody>().velocity = transform.TransformDirection(movement) * speed;
         } else {
             GetComponent<Rigidbody>().velocity = new Vector3();
             Vector3 movement = new Vector3(moveVertical, 0.0f, -moveHorizontal);
         }

        if (axes == RotationAxis.MOUSEX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * HorizontalSpeed, 0);
        }

        if(Input.GetKeyDown("space") && !attacking)
        {
            attackHitbox.SetActive(true);
            attacking = true;
            attackTimer = attackDuration;
        }
        if(attacking)
        {
            attackTimer -= Time.deltaTime;
            if(attackTimer < 0)
            {
                attacking = false;
                attackHitbox.SetActive(false);
            }
        }
    }

    public void changeReferenceAngle(float angle)
    {
        playerWorldRelationAngle = angle%360;
    }

    public void Hit()
    {
        found = true;
        GetComponent<BoxCollider>().enabled = false;
        mesh.GetComponent<MeshRenderer>().material = foundTex;
    }

    public bool IsFound(){
        return found;
    }
}
