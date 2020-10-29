using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitController : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "rocket" && other.gameObject.tag == "mine")
        {
            Debug.Log("ouch!!");
            Destroy(other.gameObject);
        }
    }

    public void Explode()
    {
        GetComponent<Rigidbody>().AddExplosionForce(100000f, gameObject.transform.position, 15f);
    }
}
