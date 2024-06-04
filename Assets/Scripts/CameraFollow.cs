using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] public Transform target;
    private Vector3 staticPosition;

    private void Update()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            staticPosition = targetPosition;
        }else
        {
            transform.position = Vector3.SmoothDamp(transform.position,staticPosition, ref velocity, smoothTime);   
        }

    }

}
