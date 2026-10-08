using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI;

    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Set Dynamically")]
    public float camZ;
    void Awake()
    {
        camZ = this.transform.position.z;
    }

    void FixedUpdate()
    {
        Vector3 destination;
        // If there is no POI, return to P:[0, 0, 0]
        if (POI == null) {
            destination = Vector3.zero;
        } else {
            // Get the position of the POI
            destination = POI.transform.position;
            // If POI is a Projectile, check to see if it's at rest
            if	(POI.tag == "Projectile") {
                // if it is not moving
                if (POI.GetComponent<Rigidbody>().IsSleeping())	{
                // return to default	view in the next update
                POI = null;
                return;
                }
            }
        }

        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);

        destination = Vector3.Lerp(transform.position, destination, easing);

        destination.z = camZ;

        this.transform.position = destination;

        Camera.main.orthographicSize = destination.y + 10;
    }
}
