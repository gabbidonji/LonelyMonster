using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveTest : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float rotationSpeed;
    private bool rotating;

    [SerializeField]
    private bool verticalMovement;

    [SerializeField]
    private float minPosition;

    [SerializeField]
    private float maxPosition;

    [SerializeField]
    private bool positiveMotion;
    // Start is called before the first frame update
    void Start()
    {
        rotating = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (rotating)
        {
            Quaternion targetRotation = verticalMovement ? Quaternion.Euler(0, positiveMotion ? 0 : 180, 0) : Quaternion.Euler(0, positiveMotion ? 90 : 270, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            if (Mathf.Abs(transform.rotation.eulerAngles.y - targetRotation.eulerAngles.y) < 0.00001)
            {
                transform.rotation = targetRotation;
                rotating = false;
            }
        }
        else
        {
            if (((verticalMovement ? this.transform.position.z : this.transform.position.x) > maxPosition && positiveMotion) 
            || ((verticalMovement ? this.transform.position.z : this.transform.position.x) < minPosition && !positiveMotion))
            {
                rotating = true;
                positiveMotion = !positiveMotion;
            }
            else
            {
                Vector3 horizontal = new Vector3((positiveMotion ? 1 : -1) * walkSpeed * Time.deltaTime, 0, 0);
                Vector3 vertical = new Vector3(0,0,(positiveMotion ? 1 : -1) * walkSpeed * Time.deltaTime);
                transform.position += verticalMovement ? vertical : horizontal;
            }
        }
    } 
}
