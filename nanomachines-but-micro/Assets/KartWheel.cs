using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartWheel : MonoBehaviour
{

    private Rigidbody rig;



    [Header("Suspension")]
    public float restLength;
    public float springTravel;

    public float springStiffness;
    public float damperStiffness;

    private float minLength;
    private float maxLength;
    private float lastLength;
    private float springLength;
    private float springVelocity;
    private float springForce;
    private float damperForce;

    private Vector3 suspensionForce;

    [Header("Steering")]
    public float steerAngle;
    public float steeringTime;
    private float wheelAngle;



    [Header("Wheel")]
    public float wheelRadius; // The radius of the wheel (in meters)

    private Vector3 wheelVelocityL; // The wheel velocity in local space

    private float // The forces affecting the wheel in X and Y directions
        Fx,
        Fy;


    private GameObject wheelModel;

    public enum Wheels
    {
        Front_Left,
        Front_Right,
        Rear_Left,
        Rear_Right
    };

    public Wheels wheel;


    void Start()
    {

        rig = transform.parent.GetComponent<Rigidbody>();
        wheelModel = transform.GetChild(0).gameObject; // The wheelModel must be the only model under the tyre


        // restLength must always be higher than spring travel ( if rest at a position lower than the suspension can extend, the suspension will spaz out

        minLength = restLength - springTravel;
        maxLength = restLength + springTravel;

    }

    void Update()
    {
        wheelAngle = Mathf.Lerp(wheelAngle, steerAngle, steeringTime * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(Vector3.up * wheelAngle);
    }



    private void FixedUpdate()
    {
        SuspensionDynamics();
    }


    // Handles the suspension
    private void SuspensionDynamics()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxLength + wheelRadius))
        {

            // Calculating the suspension

            lastLength = springLength;
            springLength = hit.distance - wheelRadius;
            springLength = Mathf.Clamp(springLength, minLength, maxLength);
            springVelocity = (lastLength - springLength) / Time.fixedDeltaTime;
            springForce = springStiffness * (restLength - springLength);
            damperForce = damperStiffness * springVelocity;

            suspensionForce = (springForce + damperForce) * transform.up;

            // Adding the throttle

            wheelVelocityL = transform.InverseTransformDirection(rig.GetPointVelocity(hit.point));
            Fx = Input.GetAxis("Vertical") * springForce;
            Fy = wheelVelocityL.x * springForce;

            rig.AddForceAtPosition(suspensionForce + (Fx * transform.forward) + (Fy * -transform.right), hit.point);

            // Setting the wheel at ground level
            wheelModel.transform.position = hit.point + new Vector3(0, (wheelModel.GetComponent<MeshRenderer>().bounds.size.y / 2), 0);


            //Debug.Log("rpm: " + RPMCounter());


        }
    }


    public void Boost(float boostAmount)
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxLength + wheelRadius))
        {
            rig.AddForceAtPosition(boostAmount * transform.forward, hit.point);
        }
    }

    public void SlowDown(float slowAmount)
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxLength + wheelRadius))
        {
            rig.AddForceAtPosition(slowAmount * transform.forward, hit.point);
        }
    }



    // Returns RPM of the vehicle
    public float RPMCounter()
    {
        // EVERYTHING IS BROKEN OH GOD
        return 0;
    }
}
