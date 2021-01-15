using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRoll : MonoBehaviour
{
    public float antiroll_stiffness = 1;

    private Rigidbody rig;

    // 0 is left 1 is right
    public KartWheel[] frontAxle;
    public KartWheel[] rearAxle;

    public bool[] grounded = new bool[4];
    public float[] wheelTravel = new float[4];
    void Start()
    {
        rig = GetComponent<Rigidbody>();   
    }
    void FixedUpdate()
    {

        grounded[0] = frontAxle[0].isGrounded();
        grounded[1] = frontAxle[1].isGrounded();
        grounded[2] = rearAxle[0].isGrounded();
        grounded[3] = rearAxle[1].isGrounded();

        wheelTravel[0] = frontAxle[0].getTravel();
        wheelTravel[1] = frontAxle[1].getTravel();
        wheelTravel[2] = rearAxle[0].getTravel();
        wheelTravel[3] = rearAxle[1].getTravel();

        var frontAntiRollForce = (wheelTravel[0] - wheelTravel[1]) * antiroll_stiffness;
        var rearAntiRollForce = (wheelTravel[2] - wheelTravel[3]) * antiroll_stiffness;


        if(grounded[0])
        {
            rig.AddForceAtPosition(
                frontAxle[0].transform.up * -frontAntiRollForce,
                frontAxle[0].getHitPoint());
        }
        if (grounded[1])
        {
            rig.AddForceAtPosition(
                frontAxle[1].transform.up * -frontAntiRollForce,
                frontAxle[1].getHitPoint());
        }
        if (grounded[2])
        {
            rig.AddForceAtPosition(
                rearAxle[0].transform.up * -rearAntiRollForce,
                rearAxle[0].getHitPoint());
        }
        if (grounded[3])
        {
            rig.AddForceAtPosition(
                rearAxle[1].transform.up * -rearAntiRollForce,
                rearAxle[1].getHitPoint());
        }

    }



}
