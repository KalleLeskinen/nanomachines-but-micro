using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : Bolt.EntityBehaviour<IVehicleState>
{
    public Vector3 cameraPos;

    public override void Attached()
    {
        cameraPos = gameObject.transform.position;
    }
}
