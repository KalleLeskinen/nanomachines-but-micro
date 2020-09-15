using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt; 


public class VehicleController : Bolt.EntityBehaviour<IVehicleState>
{

    //The wheels, 0 = FL, 1 = FR, 2 = BL, 3 = BR; 
    public Vector3[] corners = new Vector3[4];
    //Points where ^^ hit the ground
    public Vector3[] contactPoints = new Vector3[4];
    private Vector3 frontWheels, backWheels;

    
    public float enginePower, brakingPower;
    public float steeringMultiplier;
    public float tractionMultiplier;
    public float suspensionStrength, suspensionDamping, suspensionDistance;



    private Rigidbody rig;

    private BoxCollider body;

    //Attached acts like Start(), it's called when the object is setup
    public override void Attached()
    {

        rig = GetComponent<Rigidbody>();
        body = gameObject.GetComponent<BoxCollider>();

        if(entity.IsOwner)
        {
            state.VehicleColor = new Color(Random.value, Random.value, Random.value);
        }

        state.AddCallback("VehicleColor", ColorChanged);

        //SetTransforms tells Bolt to replicate the transform over the network
        state.SetTransforms(state.VehicleTransform, transform);
    }


    public override void SimulateOwner()
    {
        GetCorners();
        CastRays(corners);
        Traction();

       

    }

    private void Update()
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
        }
    }


    private void Turn(int dir)
    {
        if (contactPoints[0] != new Vector3(0, 0, 0) && contactPoints[1] != new Vector3(0, 0, 0))
        {
            frontWheels = contactPoints[0] + (contactPoints[1] - contactPoints[0]) / 2;

            Vector3 fwd = rig.transform.up;

            rig.AddTorque(fwd * (-dir * steeringMultiplier), ForceMode.Acceleration);
        }


    }

    //Accelerate using only front wheels
    private void Accelerate()
    {

        if (contactPoints[0] != new Vector3(0, 0, 0) && contactPoints[2] != new Vector3(0, 0, 0))
        {
            frontWheels = contactPoints[0] + (contactPoints[1] - contactPoints[0]) / 2;

            Vector3 fwd = rig.transform.forward;

            rig.AddForceAtPosition(fwd * enginePower, frontWheels);
        }
    }

    //Brake using both axels
    private void Brake()
    {

        //Front Wheels
        if (contactPoints[0] != new Vector3(0, 0, 0) && contactPoints[1] != new Vector3(0, 0, 0))
        {
            frontWheels = contactPoints[0] + (contactPoints[1] - contactPoints[0]) / 2;

            Vector3 fwd = rig.transform.forward;

            rig.AddForceAtPosition(fwd * -brakingPower, frontWheels);
        }

        //Back Wheels
        if (contactPoints[2] != new Vector3(0, 0, 0) && contactPoints[3] != new Vector3(0, 0, 0))
        {
            backWheels = contactPoints[2] + (contactPoints[3] - contactPoints[2]) / 2;

            Vector3 fwd = rig.transform.forward;

            rig.AddForceAtPosition(fwd * -brakingPower, backWheels);
        }

    }

    private void Traction()
    {

        //Getting the sideways speed
        float xV = transform.InverseTransformDirection(rig.velocity).x;

        xV *= tractionMultiplier;

        //Subtract it
        rig.AddRelativeForce(Vector3.right * -xV);
        
    }




    //Gets the corners of the vehicles hitbox
    private void GetCorners()
    {

        Vector3 size = body.size;

        Vector3 center = new Vector3(body.center.x, body.center.y, body.center.z);

        Vector3 vx1 = new Vector3(center.x + size.x / 2, center.y, center.z + size.z / 2);
        Vector3 vx2 = new Vector3(center.x - size.x / 2, center.y, center.z - size.z / 2);
        Vector3 vx3 = new Vector3(center.x + size.x / 2, center.y, center.z - size.z / 2);
        Vector3 vx4 = new Vector3(center.x - size.x / 2, center.y, center.z + size.z / 2);

        corners[0] = transform.TransformPoint(vx1);
        corners[1] = transform.TransformPoint(vx2);
        corners[2] = transform.TransformPoint(vx3);
        corners[3] = transform.TransformPoint(vx4);

        /*
        Debug.DrawRay(transform.TransformPoint(vx1), -transform.up * rl, Color.blue);
        Debug.DrawRay(transform.TransformPoint(vx2), -transform.up * rl, Color.blue);
        Debug.DrawRay(transform.TransformPoint(vx3), -transform.up * rl, Color.blue);
        Debug.DrawRay(transform.TransformPoint(vx4), -transform.up * rl, Color.blue);
        */
    }



    //Cast rays from the corners of the vehilces (simulating suspension)
    private void CastRays(Vector3[] corners)
    {

        Vector3 down = transform.TransformDirection(Vector3.down);
        RaycastHit hit;

        for (int i = 0; i < 4; i++)
        {
            if (Physics.Raycast(corners[i], down, out hit, 1))
            {
                Debug.DrawRay(corners[i], down * hit.distance, Color.red, 0.01f);

                rig.AddForceAtPosition(CalculateSuspension(hit.distance, corners[i]) * Vector3.up, corners[i], ForceMode.Acceleration);

                contactPoints[i] = hit.point;

            }
            else
            {
                contactPoints[i] = new Vector3(0, 0, 0);
            }
        }

    }

    private float CalculateSuspension(float distance, Vector3 v3)
    {

        //The length of the compressed spring
        float d = suspensionDistance - distance;

        //Current Vel at spring (on Y axis)
        float curV = rig.GetPointVelocity(v3).y;

        //New Spring value
        float newS = v3.y * d * suspensionStrength;

        //New Spring Delta
        float deltaF = newS - curV;

        Debug.Log("Force = " + deltaF + " | Distance = " + d);

        return deltaF;
    }


    void ColorChanged()
    {
        gameObject.transform.Find("CarBody").GetComponent<Renderer>().materials[1].color = state.VehicleColor;
    }




}
