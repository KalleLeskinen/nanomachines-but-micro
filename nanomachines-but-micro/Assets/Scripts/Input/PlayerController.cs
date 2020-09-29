using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Bolt.EntityBehaviour<IVehicleState>
{
    private bool forward;
    private bool backward;
    private bool left;
    private bool right;
    private bool jump;

    private float yaw;

    private PlayerMotor playerMotor;

    private void Awake()
    {
        playerMotor = GetComponent<PlayerMotor>();
    }

    public override void Attached()
    {
        state.SetTransforms(state.VehicleTransform, transform);
    }

    public void PollKeys()
    {
        
    }
}
