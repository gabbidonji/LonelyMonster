using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject player;

    [SerializeField]
    [Range(0, 10)]
    private float visionDistance;

    [SerializeField]
    [Range(0, 180)]
    private float fieldOfView;

    void Start()
    {

    }

    void Update()
    {
        float distance = Vector3.Distance(this.transform.position, player.transform.position);
        if (distance < visionDistance)
        {
            RaycastHit hit;
            Vector3 playerPositionEnemyBasis = transform.InverseTransformPoint(player.transform.position);
            Vector3 relativePlayerPosition = transform.TransformDirection(playerPositionEnemyBasis);
            if (Physics.Raycast(transform.position, relativePlayerPosition, out hit, visionDistance))
            {
                GameObject hitObject = hit.transform.gameObject;
                if (hitObject.tag == "Player")
                {
                    float angle = Mathf.Abs(Mathf.Atan2(playerPositionEnemyBasis.x, playerPositionEnemyBasis.z) * Mathf.Rad2Deg);
                    if (angle < fieldOfView)
                    {
                        if(player.GetComponent<DemoPlayerMovement>() != null) player.GetComponent<DemoPlayerMovement>().Found();
                        if(player.GetComponent<PlayerController>() != null) player.GetComponent<PlayerController>().Found();
                    }
                }
            }
            Debug.DrawLine(this.transform.position, hit.point);
        }

    }
}
