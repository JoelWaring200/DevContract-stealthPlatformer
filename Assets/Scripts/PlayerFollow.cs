using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    //Set the target by using its Transform
    private Transform target;

    //For smooth movement of the camera
    public float smoothFactor = 1.125f;

    //Vector3 for camera offset positioning
    public Vector3 cameraOffset = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        //Look for a GameObject with a tag "Player" and get its transform
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    //Runs after Update is done updating
    void LateUpdate()
    {
        //Local Vector3 variable.
        //I did this to "separate" the z value out because we DO NOT want the camera's
        //z position to be the same as the player's z position.
        //Our new position will be the target's x position, the target's y position but the
        //z remains unchanged.
        Vector3 newPos = new Vector3(target.position.x, target.position.y,
                                    transform.position.z);

        //Change the camera's position using Vector3.Lerp.
        /*Lerp is "Linear Interpolation" which is a smooth movement between two Vectors.
         * Parameters are (starting point, ending point, time for transition)
         * So, we are moving from the camera's current position to the target position calculated
         * above at the speed of time (deltaTime = time between frames) scaled by smoothFactor
         * which is just there for pizazz.*/
        transform.position = Vector3.Lerp(transform.position, newPos + cameraOffset,
                                        Time.deltaTime * smoothFactor);
    }
}
