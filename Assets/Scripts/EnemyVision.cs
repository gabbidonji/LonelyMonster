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

    [Range(0, 100)]
    public float followingVisionDistance;

    [SerializeField]
    [Range(0, 180)]
    private float fieldOfView;

    [Range(0, 180)]
    public float followingFieldOfView;

    private Enemy1AI ai;

    private bool followingPlayer;

    void Start()
    {
        followingPlayer = false;
        ai = GetComponent<Enemy1AI>();
    }

    public bool SeesPlayer(){
        float distance = Vector3.Distance(this.transform.position, player.transform.position);
        if (distance < (followingPlayer?followingVisionDistance:visionDistance))
        {
            RaycastHit hit;
            Vector3 playerPositionEnemyBasis = transform.InverseTransformPoint(player.transform.position);
            Vector3 relativePlayerPosition = transform.TransformDirection(playerPositionEnemyBasis);
            if (Physics.Raycast(transform.position, relativePlayerPosition, out hit, followingPlayer?followingVisionDistance:visionDistance))
            {
                GameObject hitObject = hit.transform.gameObject;
                if (hitObject.tag == "Player")
                {
                    float angle = Mathf.Abs(Mathf.Atan2(playerPositionEnemyBasis.x, playerPositionEnemyBasis.z) * Mathf.Rad2Deg);
                    if (angle < (followingPlayer?followingFieldOfView:fieldOfView))
                    {
                        return true;
                    }
                }
            }
            Debug.DrawLine(this.transform.position, hit.point);
        }
        return false;
    }

    public void FollowingPlayer(){
        followingPlayer = true;
    }

    public void NotFollowingPlayer(){
        followingPlayer = false;
    }
}
