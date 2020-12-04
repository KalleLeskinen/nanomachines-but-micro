﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile_controller : Bolt.EntityBehaviour<IRocketState>
{
    public GameObject explosion_effect;
    private bool exploded;
    private void Start()
    {
        state.OnExplosion += HandleExplosion;
        state.DetonateTime = 5f;
        state.Exploded = false;
    }
    private void HandleExplosion()
    {
        state.Exploded = true;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.Translate(Vector3.forward * Time.deltaTime * 40f);

        state.DetonateTime -= Time.deltaTime;

        if (state.DetonateTime < 0)
        {
            Instantiate(explosion_effect, transform.position, transform.rotation);
            FMODUnity.RuntimeManager.PlayOneShot("event:/missileMiss", GetComponent<Transform>().position);
            state.Explosion();
        }
        if (state.Exploded)
            BoltNetwork.Destroy(this.gameObject);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("generic_wall") && !exploded)
        {
            Instantiate(explosion_effect, transform.position, transform.rotation);
            FMODUnity.RuntimeManager.PlayOneShot("event:/Explosion", GetComponent<Transform>().position);
            state.Explosion();
        }

        if (other.gameObject.tag.Equals("Player") && !state.Exploded && state.DetonateTime < 4.95f && state.DetonateTime != 0)
        {
            Instantiate(explosion_effect, transform.position, transform.rotation);
            other.gameObject.GetComponent<OnHitController>().Explode();
            FMODUnity.RuntimeManager.PlayOneShot("event:/WeaponHit", GetComponent<Transform>().position);
            state.Explosion();
        }
    }
}
