using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineController : Bolt.EntityBehaviour<ILandMineState>
{
    public GameObject explosion_effect;
    public float trigger_time;
    void Start()
    {
        state.DetonateTime = 45f;
        state.OnExplosion += handlerExplosion;
        state.Exploded = false;
    }
    void handlerExplosion()
    {
        state.Exploded = true;
        BoltNetwork.Destroy(this.gameObject);
    }
    void FixedUpdate()
    {
        state.DetonateTime = state.DetonateTime - Time.deltaTime;
        if (state.DetonateTime < 0)
        {
            Instantiate(explosion_effect, transform.position, transform.rotation);
            FMODUnity.RuntimeManager.PlayOneShot("event:/Explosion", GetComponent<Transform>().position);
            state.Explosion();
        }
        if (Time.frameCount % 50 == 0)
            gameObject.GetComponentsInChildren<Renderer>()[1].material.EnableKeyword("_EMISSION");
        if (Time.frameCount % 25 == 0 && Time.frameCount % 50 != 0)
            gameObject.GetComponentsInChildren<Renderer>()[1].material.DisableKeyword("_EMISSION");
        if (state.Exploded)
            BoltNetwork.Destroy(this.gameObject);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !state.Exploded && state.DetonateTime < 44f && state.DetonateTime != 0)
        {
            Instantiate(explosion_effect, transform.position, transform.rotation);
            other.gameObject.GetComponent<OnHitController>().Explode();
            FMODUnity.RuntimeManager.PlayOneShot("event:/mineHit", GetComponent<Transform>().position);
            state.Explosion();
        }
    }
}
