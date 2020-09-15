using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; //the static point of interest

    [Header("Set in Inspector")]
    public float easing = 0.05f;

    [Header("Set Dynamically")]
    public float camZ;
    public Vector2 minXY = Vector2.zero;

    void Awake()
    {
        camZ = this.transform.position.z; 
    }

    void FixedUpdate()
    {
        //if (POI == null) return; //return if there is no POI

        ////get the position of the POI 
        //Vector3 destination = POI.transform.position;

        Vector3 destination;
        //if there is no poi, return to P:[0, 0, 0]
        if(POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            //get the position of the poi
            destination = POI.transform.position;

            //if poi is a projectile, check to see if it's at rest
            if(POI.tag == "Projectile")
            {
                //if it is sleeping (that is, not moving)
                if(POI.GetComponent<Rigidbody>().IsSleeping() )
                {
                    //return to default view
                    POI = null;

                    //in the next update
                    return;
                }
            }
        }

    

        //limit the X and Y to minimum values
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);

        //interpolate from the current camera position toward destination
        destination = Vector3.Lerp(transform.position, destination, easing);

        //force destination.z to be camz to keep the camera far enough away
        destination.z = camZ;

        //set the camera to the destination
        transform.position = destination;

        //set the orthographicSize of the Camaera to keep ground in view
        Camera.main.orthographicSize = destination.y + 10;
    
    }

}
