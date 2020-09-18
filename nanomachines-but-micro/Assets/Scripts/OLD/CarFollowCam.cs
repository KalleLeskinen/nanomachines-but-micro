using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarFollowCam : MonoBehaviour
{


    private Transform target;
    
    public float followDistance = 10.0f;
    public float height = 5.0f;
    public float heightDampingMultiplier = 2.0f;

    public float lookAtHeight = 0.0f;
    public float rotationSnapTime = 0.3f;

    private Rigidbody rig;

    public float distanceSnapTime;
    public float distanceMultiplier;

    private Vector3 lookAtVector;
    private float usedDistance;

    float wantedRotationAngle;
    float wantedHeight;

    float currentRotationAngle;
    float currentHeight;

    Vector3 wantedPosition;

    //Velocity in Y
    private float yV= 0;

    //Velocity in Z
    private float zV = 0;




    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rig = GetComponentInParent<Rigidbody>();

        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        lookAtVector = new Vector3(0, lookAtHeight, 0);
        Debug.Log("Target: " + target.gameObject.name);

        wantedHeight = target.position.y + height;
        currentHeight = target.position.y;

        wantedRotationAngle = target.eulerAngles.y;
        
        currentRotationAngle = transform.eulerAngles.y;

        currentRotationAngle = Mathf.SmoothDampAngle(currentRotationAngle, wantedRotationAngle, ref yV, rotationSnapTime);

        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDampingMultiplier * Time.deltaTime);

        wantedPosition = target.position;
        wantedPosition.y = currentHeight;

        usedDistance = Mathf.SmoothDampAngle(usedDistance, followDistance + (rig.velocity.magnitude * distanceMultiplier), ref zV, distanceSnapTime);

        wantedPosition += Quaternion.Euler(0, currentRotationAngle, 0) * new Vector3(0, 0, -usedDistance);

        transform.position = wantedPosition;

        transform.LookAt(target.position + lookAtVector);

    }
}
