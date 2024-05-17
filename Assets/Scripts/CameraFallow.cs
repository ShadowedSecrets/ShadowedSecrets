using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFallow : MonoBehaviour
{
    public Transform target; // sets the target of the camera(player) 
    public float smoothing;  // sets the smothing rate of camera following
    public Vector2 maxPosition; // sets the max position of camera 
    public Vector2 minPosition; // sets the min position of camera 
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // lateUpdate gets called after all update functions have been called, should always be used for camera follow. 
    void LateUpdate()
    {
        // if statement for checking if the camera is locked on to the target(player) if it is then run it. 
        if (transform.position != target.position){
            //make the camera track the target on the proper z axis. 
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

            // sets the max and min on the x and y axis positions, so the camera doesn't show out of bounds. 
            targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
            targetPosition.y = Mathf.Clamp (targetPosition.y, minPosition.y, maxPosition.y);

            // tracks the target and moves the camera toward target.
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing); 
        }
    }
}
