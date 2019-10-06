using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverheadCameraController : MonoBehaviour
{
    [SerializeField]
    [Range(0,30)]
    private float distanceFromPlayer;

    [SerializeField]
    [Range(0,90)]
    private float groundCameraAngle;
    private float initialCameraWorldRotationAngle;
    private float cameraWorldRotationAngle;
    private float targetCameraWorldRotationAngle;

    [SerializeField]
    [Range(1,10)]
    private float fixedRotationSpeed;

    [SerializeField]
    [Range(1,300)]
    private float freeRotationSpeed;

    [SerializeField]
    private GameObject player;

    private float rotationTimer;

    [SerializeField]
    public bool freeRotation;

    // Start is called before the first frame update
    void Start()
    {
        initialCameraWorldRotationAngle = transform.rotation.eulerAngles.y;
        cameraWorldRotationAngle = initialCameraWorldRotationAngle;
        rotationTimer = 1.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!freeRotation){
            if(Input.GetKeyDown("e") || Input.GetKeyDown("q")){
                rotationTimer = 0;
                initialCameraWorldRotationAngle = targetCameraWorldRotationAngle;
                targetCameraWorldRotationAngle = (targetCameraWorldRotationAngle+(Input.GetKeyDown("q") ? -1 : 1)*90)%360;
                player.GetComponent<PlayerController>().changeReferenceAngle(targetCameraWorldRotationAngle);
            }

            if(rotationTimer <= 1){
                cameraWorldRotationAngle = Mathf.LerpAngle(initialCameraWorldRotationAngle, targetCameraWorldRotationAngle, rotationTimer);
                rotationTimer += fixedRotationSpeed * Time.deltaTime;
            } else {
                cameraWorldRotationAngle = targetCameraWorldRotationAngle;
            }
        } else {
            if(Input.GetKey("e")){
                cameraWorldRotationAngle = cameraWorldRotationAngle + freeRotationSpeed * Time.deltaTime;
            }
            if(Input.GetKey("q")){
                cameraWorldRotationAngle = cameraWorldRotationAngle - freeRotationSpeed * Time.deltaTime;
            }
            player.GetComponent<PlayerController>().changeReferenceAngle(cameraWorldRotationAngle);
        }

        float groundCameraAngleRads = groundCameraAngle*Mathf.Deg2Rad;
        float cameraWorldRotationAngleRads = cameraWorldRotationAngle%360*Mathf.Deg2Rad;
        Vector3 rotation = new Vector3(groundCameraAngle,cameraWorldRotationAngle,0);
        transform.rotation = Quaternion.Euler(rotation);
        float flattenedDistance = Mathf.Cos(groundCameraAngleRads)*distanceFromPlayer;
        Vector3 playerRelativePostion = new Vector3(-1*flattenedDistance*Mathf.Sin(cameraWorldRotationAngleRads), Mathf.Sin(groundCameraAngleRads)*distanceFromPlayer,-1*flattenedDistance*Mathf.Cos(cameraWorldRotationAngleRads));
        transform.position = player.transform.position + playerRelativePostion;
    }
}
