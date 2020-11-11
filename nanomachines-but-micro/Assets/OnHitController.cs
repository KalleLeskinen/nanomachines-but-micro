using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitController : MonoBehaviour
{
    public void Explode()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    }
}
