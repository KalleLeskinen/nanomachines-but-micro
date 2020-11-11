using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitController : MonoBehaviour
{
    public GameObject explosion_effect;

    //void OnCollisionEnter(Collision other)
    //{
    //    if (other.gameObject.tag.Equals("rocket") || other.gameObject.tag.Equals("mine"))
    //    {
    //        Instantiate(explosion_effect, transform.position, transform.rotation);
    //    }
    //    Debug.Log("onhit1");

    //}

    public void Explode()
    {
        Debug.Log("onhit2");
        GetComponent<Rigidbody>().AddExplosionForce(100000f, gameObject.transform.position, 15f);
        
    }
}
