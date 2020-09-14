using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using System;
using TMPro.EditorUtilities;

public class VehicleController : Bolt.EntityBehaviour<IVehicleState>
{

    //The wheels, 0 = FL, 1 = FR, 2 = BL, 3 = BR; 
    public Vector3[] corners = new Vector3[4];

    public float SuspensionStrength, SuspensionDamping, SuspensionDistance;

    private Rigidbody rig;

    private BoxCollider body;

    //Attached acts like Start(), it's called when the object is setup
    public override void Attached()
    {

        


        //SetTransforms tells Bolt to replicate the transform over the network
        state.SetTransforms(state.VehicleTransform, transform);
    }


    public override void SimulateOwner()
    {

        

    }

    //CHANGE THIS vvv -> ^^^


    public void Awake()
    {
        rig = GetComponent<Rigidbody>();
        body = gameObject.GetComponent<BoxCollider>();
    }

    private void Update()
    {

        
        //CastRays(wheels);

        var speed = 4f;
        var movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) { movement.z += 1; }
        if (Input.GetKey(KeyCode.S)) { movement.z -= 1; }
        if (Input.GetKey(KeyCode.A)) { movement.x -= 1; }
        if (Input.GetKey(KeyCode.D)) { movement.x += 1; }

        if (movement != Vector3.zero)
        {
            transform.position = transform.position + (movement.normalized * speed * BoltNetwork.FrameDeltaTime);
        }
    }

    private void FixedUpdate()
    {
        GetCorners();
        CastRays(corners);



        
    }

    private void GetCorners()
    {
        Debug.Log("Gettign Corners");

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




    private void CastRays(Vector3[] corners)
    {
        
        Vector3 down = transform.TransformDirection(Vector3.down);
        RaycastHit hit;

        foreach(Vector3 v3 in corners)
        {
            
            if (Physics.Raycast(v3, down, out hit, 1))
            {
                Debug.DrawRay(v3, down * hit.distance, Color.green, 0.01f);

                rig.AddForceAtPosition(CalculateSuspension(hit.distance, v3) * Vector3.up, v3);

            }
        }
    }

    private float CalculateSuspension(float distance, Vector3 v3)
    {

        //The length of the compressed spring
        float d = SuspensionDistance - distance;

        float force = -SuspensionStrength * d + (SuspensionDamping * rig.GetPointVelocity(v3).y);

        Debug.Log("Force = " + force);
        return force;
    }




}
