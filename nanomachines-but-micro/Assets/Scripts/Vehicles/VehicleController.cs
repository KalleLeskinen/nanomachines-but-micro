using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;


public class VehicleController : Bolt.EntityBehaviour<IVehicleState>
{


    //Basically, the wheels, 0 = FL, 1 = FR, 2 = BL, 3 = BR;
    private Vector3[] corners = new Vector3[4];

    //Points where the "wheels" touch the ground
    public Vector3[] contactPoints = new Vector3[4];

    private Vector3 frontWheels, backWheels;

    public float enginePower, brakingPower;
    public float steeringMultiplier;
    public float tractionMultiplier;
    public float suspensionStrength, suspensionDamping, suspensionDistance;

    //Rigidbody for the car
    private Rigidbody rig;

    //Collider for the body of the car
    private BoxCollider body;



    //Attach acts like Start(), It's called when the object is setup on the server
    public override void Attached()
    {

        rig = GetComponent<Rigidbody>();
        body = gameObject.GetComponent<BoxCollider>();



        //Checks if you own the entity
        if (entity.IsOwner)
        {
            //Sets the vehicles color to a random value    
            state.VehicleColor = new Color(Random.value, Random.value, Random.value);
        }

        //If you're not the owner
        if (entity.IsOwner == false)
        {
            //Set the gravity to false, the owner of the object calculates the physics
            rig.useGravity = false;
        }

        state.AddCallback("VehicleColor", ColorChanged);

        //SetTransforms tells Bolt to replicate the transform over the network
        state.SetTransforms(state.VehicleTransform, transform);

    }

    //Acts as FixedUpdate() on the owner of the object
    public override void SimulateOwner()
    {
        ProcessInput();
        GetCorners();
        CastRays(corners);
        Traction();
    }

    //All below should be cleaned up. vvvvvvvvvvvvvvvvvvvvvvvvvvv

    //Processing the player input
    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Accelerate();
        }

        if (Input.GetKey(KeyCode.S))
        {
            Brake();
        }


        if (Input.GetKey(KeyCode.A))
        {
            //Turn Left
            Turn(1);
        }

        if (Input.GetKey(KeyCode.D))
        {
            //Turn Right
            Turn(-1);
        }

        //Reset
        if (Input.GetKey(KeyCode.R))
        {
            transform.position = new Vector3(0, 10, 0);
            transform.rotation = new Quaternion();
        }
    }

    //Turning
    private void Turn(int dir)
    {
        //Checking if both the front tires are on the ground
        if (contactPoints[0] != new Vector3(0, 0, 0) && contactPoints[1] != new Vector3(0, 0, 0))
        {
            //The point between the front tires
            //frontWheels = contactPoints[0] + (contactPoints[1] - contactPoints[0]) / 2;

            Vector3 fwd = rig.transform.up;

            //Adding torque to the vehicle to turn, (dir is just for the direction of the turn)
            rig.AddTorque(fwd * (-dir * steeringMultiplier), ForceMode.Acceleration);
        }


    }

    //Acceleration
    private void Accelerate()
    {
        //Checking if both the front tires are on the ground
        if (contactPoints[0] != new Vector3(0, 0, 0) && contactPoints[2] != new Vector3(0, 0, 0))
        {
            //The point between the front tires
            frontWheels = contactPoints[0] + (contactPoints[1] - contactPoints[0]) / 2;

            Vector3 fwd = rig.transform.forward;

            //Adding the force at the position of the frontwheels
            rig.AddForceAtPosition(fwd * enginePower, frontWheels);
        }
    }

    //The braking
    private void Brake()
    {

        //Checking if the front tires are on the ground
        if (contactPoints[0] != new Vector3(0, 0, 0) && contactPoints[1] != new Vector3(0, 0, 0))
        {

            frontWheels = contactPoints[0] + (contactPoints[1] - contactPoints[0]) / 2;

            Vector3 fwd = rig.transform.forward;

            rig.AddForceAtPosition(fwd * -brakingPower, frontWheels);
        }

        //Checking if the back tires are on the ground
        if (contactPoints[2] != new Vector3(0, 0, 0) && contactPoints[3] != new Vector3(0, 0, 0))
        {
            backWheels = contactPoints[2] + (contactPoints[3] - contactPoints[2]) / 2;

            Vector3 fwd = rig.transform.forward;

            rig.AddForceAtPosition(fwd * -brakingPower, backWheels);
        }

    }

    //Gets the corners of the vehicles hitbox
    private void GetCorners()
    {
        //The size of the vehicles collider
        Vector3 size = body.size;

        //Center for the vehicle
        Vector3 center = new Vector3(body.center.x, body.center.y, body.center.z);

        //The locations of the corners are calculated here
        Vector3 vx1 = new Vector3(center.x + size.x / 2, center.y, center.z + size.z / 2);
        Vector3 vx2 = new Vector3(center.x - size.x / 2, center.y, center.z - size.z / 2);
        Vector3 vx3 = new Vector3(center.x + size.x / 2, center.y, center.z - size.z / 2);
        Vector3 vx4 = new Vector3(center.x - size.x / 2, center.y, center.z + size.z / 2);

        //Saving all the calculated corners
        corners[0] = transform.TransformPoint(vx1);
        corners[1] = transform.TransformPoint(vx2);
        corners[2] = transform.TransformPoint(vx3);
        corners[3] = transform.TransformPoint(vx4);

    }

    //Cast rays from the corners of the vehicles (simulating suspension)
    private void CastRays(Vector3[] corners)
    {

        //Down vector in worldspace instead of the local
        Vector3 down = transform.TransformDirection(Vector3.down);

        RaycastHit hit;

        //Checking all 4 corners of the vehicle
        for (int i = 0; i < 4; i++)
        {
            if (Physics.Raycast(corners[i], down, out hit, 1))
            {
                Debug.DrawRay(corners[i], down * hit.distance, Color.red, 0.02f);

                //Adds a upward force to simulate the suspension
                rig.AddForceAtPosition(CalculateSuspension(hit.distance, corners[i]) * Vector3.up, corners[i], ForceMode.Acceleration);

                //Adding all the known "tires" that are on the ground
                contactPoints[i] = hit.point;

            }
            else
            {
                //If the "tire" isn't on the ground, set it to 0
                contactPoints[i] = new Vector3(0, 0, 0);
            }
        }

    }

    //Calculates the suspension force from the given distance to ground
    private float CalculateSuspension(float distance, Vector3 cor)
    {

        //The amount the suspension has been compressed
        float d = suspensionDistance - distance;

        //Current velocity at spring (on the Y axis)
        float curV = rig.GetPointVelocity(cor).y;

        //Calculating the new value for the spring
        float newS = cor.y * d * suspensionStrength;

        //Removing the current speed of the suspension so that the expansion stays constant
        float deltaF = newS - curV;

        //Debug.Log("Force = " + deltaF + " | Distance = " + d);

        return deltaF;
    }

    //Very temporary and very simple traction 
    private void Traction()
    {

        //Getting the sideways speed
        float xV = transform.InverseTransformDirection(rig.velocity).x;

        xV *= tractionMultiplier;

        //Subtract it
        rig.AddRelativeForce(Vector3.right * -xV);

    }

    //Callback for when a remote player changes color
    void ColorChanged()
    {
        gameObject.transform.Find("CarBody").GetComponent<Renderer>().materials[1].color = state.VehicleColor;
    }






}
