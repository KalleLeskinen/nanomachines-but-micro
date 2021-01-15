using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartWheel : MonoBehaviour
{

    public Rigidbody rig;

    [Header("Engine")]
    public float enginePower; // The power of the vehicle

    [Header("Braking")]
    public float brakingPower; // The power of the brakes ( should be around twice the power of the vehicle)



    [Header("Suspension")]
    public float restLength;
    public float springTravel;

    public float springStiffness;
    public float damperStiffness;


    public float
        minLength,
        maxLength,
        lastLength;

    public float
        springLength,
        springVelocity,
        springForce;

    private float damperForce;

    private Vector3 suspensionForce;

    [Header("Steering")]
    public float steerAngle;
    public float steeringTime;
    private float wheelAngle;




    [Header("Wheel")]
    public float wheelRadius; // The radius of the wheel (in meters)

    private Vector3 wheelVelocityL; // The wheel velocity in local space

    public float test;
    public float testoo;

    private float // The forces affecting the wheel in X and Y directions
        Fx,
        Fy;

    private GameObject wheelPos;
    private GameObject wheelModel;

    private float WheelBoundsSizeY;

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

        CreateWheelPos();


        // restLength must always be higher than spring travel ( if rest at a position lower than the suspension can extend, the suspension will spaz out



    }

    private void CreateWheelPos()
    {
        wheelPos = new GameObject();

        wheelPos.name = "WheelPos_" + transform.name;
        wheelPos.transform.position = wheelModel.transform.position;
        wheelPos.transform.parent = transform;

        wheelModel.transform.parent = wheelPos.transform;
        WheelBoundsSizeY = wheelModel.GetComponent<MeshRenderer>().bounds.size.y;
    }


    float xd = 0;

    void Update()
    {
        wheelAngle = Mathf.Lerp(wheelAngle, steerAngle, steeringTime * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(Vector3.up * wheelAngle);

        minLength = restLength - springTravel + test;
        maxLength = restLength + springTravel + testoo;

        xd++;



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

            //Debug.Log(transform.name + "_MIN_FORCE " + -1500 * (-springLength / maxLength) + " | " + (-springLength / maxLength));

            springForce = Mathf.Clamp(springForce, -1500 * (-springLength / maxLength), 15000);

            suspensionForce = (springForce + damperForce) * transform.up;

            // Adding the throttle

            wheelVelocityL = transform.InverseTransformDirection(rig.GetPointVelocity(hit.point));

            // Accelerating
            if (Input.GetAxis("Vertical") > 0)
            {
                if (transform.InverseTransformDirection(rig.velocity).z < 20)
                {
                    //Debug.Log("Accelerating");
                    Fx = springForce * (Input.GetAxis("Vertical") * enginePower);
                }
                else
                {
                    //Debug.Log("Going too fast!");
                    Fx = springForce * (transform.InverseTransformDirection(rig.velocity).z * -0.01f);
                }

            }

                // Engine friction  
                if (Input.GetAxis("Vertical") == 0)
            {

                if (transform.InverseTransformDirection(rig.velocity).z > 0.3f || transform.InverseTransformDirection(rig.velocity).z < -0.3f)
                {

                    Fx = springForce * (transform.InverseTransformDirection(rig.velocity).z * -0.05f);

                }
                else
                {
                    Fx = springForce * -0.075f;
                }


            }

            // Braking
            if (Input.GetAxis("Vertical") < 0)
            {
                if (transform.InverseTransformDirection(rig.velocity).z > 0.5f)
                {
                    //Debug.Log("Braking!");
                    Fx = (Input.GetAxis("Vertical") * (brakingPower)) * springForce;
                }
                else if (transform.InverseTransformDirection(rig.velocity).z < 0.5f && transform.InverseTransformDirection(rig.velocity).z > -3)
                {
                    //Debug.Log("Backing up slow");
                    Fx = (Input.GetAxis("Vertical") * (enginePower * 0.4f)) * springForce;
                }
                else
                {
                    //Debug.Log("Backing up too fast!");
                    Fx = springForce * (transform.InverseTransformDirection(rig.velocity).z * -0.05f);
                }

            }

            Fy = wheelVelocityL.x * springForce;


            rig.AddForceAtPosition(suspensionForce + (Fx * transform.forward) + (Fy * -transform.right), hit.point);


            wheelPos.transform.position = hit.point + new Vector3(0, (WheelBoundsSizeY / 2), 0);
            




        }
    }

    public float RPM()
    {
        var c = Mathf.PI * WheelBoundsSizeY;

        Debug.Log(wheelModel.transform.parent.name + " | " + rig.velocity.magnitude);

        var rpm = transform.InverseTransformDirection(rig.velocity).z / c;
        rpm = rpm;
        return rpm;

    }


    public void Boost(float boostAmount)
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxLength + wheelRadius))
        {
            //rig.AddForceAtPosition(boostAmount * Vector3.forward, hit.point);
            rig.velocity += transform.forward * boostAmount;
        }
    }

    public void SlowDown(float slowAmount)
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxLength + wheelRadius))
        {
            //rig.AddForceAtPosition(slowAmount * Vector3.forward, hit.point);
            rig.velocity -= transform.forward * slowAmount;
        }
    }

    public void OnExplosion(float explosionAmount)
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxLength + wheelRadius))
        {
            //rig.AddForceAtPosition(slowAmount * Vector3.forward, hit.point);
            //rig.velocity -= transform.up * explosionAmount;
            rig.velocity = Vector3.zero;
            rig.AddExplosionForce(explosionAmount, transform.position, 1f, 5f, ForceMode.Impulse);

        }
    }

    //Is the wheel in contact with ground
    public bool isGrounded()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxLength + wheelRadius))
        {
            return true;
        }

        return false;
    }

    public float getTravel()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxLength + wheelRadius))
        {

            Debug.DrawRay(transform.position, -transform.up * hit.distance, Color.blue, 2f);

            return (-transform.InverseTransformDirection(hit.point).y - wheelRadius) / (springLength - minLength);
        }

        
        return 1;
    }

    public Vector3 getHitPoint()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxLength + wheelRadius))
        {
            return hit.point;
        }

        return new Vector3();
    }
}
