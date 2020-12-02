using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitController : Bolt.EntityBehaviour<IVehicleState>
{
    public GameObject hit_smoke;

    private void Start()
    {
        state.OnExploded += explosionEffect;
    }

    private void explosionEffect()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        GetComponent<Rigidbody>().AddExplosionForce(4000f, Vector3.down, 5f);
    }

    public void Explode()
    {
        state.Exploded();
        StartCoroutine(hit_smoke_timer());
    }
    IEnumerator hit_smoke_timer()
    {
        hit_smoke.SetActive(true);
        yield return new WaitForSeconds(2f);
        hit_smoke.SetActive(false);
    }
}
