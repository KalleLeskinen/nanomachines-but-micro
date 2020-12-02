using System;
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
        BoltNetwork.Destroy(this.gameObject);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.Translate(Vector3.forward * Time.deltaTime * 40f);

        state.DetonateTime -= Time.deltaTime;

        if (state.DetonateTime < 0)
        {
            Instantiate(explosion_effect, transform.position, transform.rotation);
            state.Explosion();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("generic_wall") && !exploded)
        {
            Instantiate(explosion_effect, transform.position, transform.rotation);
            state.Explosion();
        }

        if (other.gameObject.tag.Equals("Player") && !state.Exploded)
        {
            Instantiate(explosion_effect, transform.position, transform.rotation);
            other.gameObject.GetComponent<OnHitController>().Explode();
            state.Explosion();
        }
    }
}
