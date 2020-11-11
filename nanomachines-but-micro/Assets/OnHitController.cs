using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitController : MonoBehaviour
{
    public GameObject hit_smoke;
    public void Explode()
    {
        StartCoroutine(hit_smoke_timer());
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    }
    IEnumerator hit_smoke_timer()
    {
        hit_smoke.SetActive(true);
        yield return new WaitForSeconds(2f);
        hit_smoke.SetActive(false);
    }
}
