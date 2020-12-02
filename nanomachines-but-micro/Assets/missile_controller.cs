using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile_controller : Bolt.EntityBehaviour<IRocketState>
{
    public GameObject explosion_effect;
    public SphereCollider explosion_collider;
    public List<GameObject> affected;
    private void Start()
    {
        state.OnExplosion += HandleExplosion;
        state.DetonateTime = 5f;
    }

    private void HandleExplosion()
    {
        // ota käyttöös uusi iso sphere collider
        explosion_collider.enabled = true;

        // tähän räjähdysanimaatio ja äänet!
        //Instantiate(explosion_effect, transform.position, transform.rotation);
        //sound.play()

        // etsi kaikki autot sphere colliderin sisältä (odota hetki)
        StartCoroutine(FindAllCarsInExplosionRadius());

        // räjäytä autot
        foreach (var car in affected)   // räjäytä autot
        {
            car.GetComponent<OnHitController>().Explode(); //autolle kutsuttava räjähdys
        }

        BoltNetwork.Destroy(this.gameObject);
    }

    IEnumerator FindAllCarsInExplosionRadius()
    {
        yield return new WaitForSeconds(0.075f);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        state.DetonateTime -= Time.deltaTime;
        gameObject.transform.Translate(Vector3.forward * Time.deltaTime * 15f);
        if (state.DetonateTime < 0)
        {
            Instantiate(explosion_effect, transform.position, transform.rotation);
            state.Explosion();
        }   // räjähdys / ei räjähdystä vaan katoaminen? (lähellä ei autoja.......)
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("generic_wall"))
        {
            Instantiate(explosion_effect, transform.position, transform.rotation);
            state.Explosion();
        }

        if (other.gameObject.tag.Equals("Player") && !affected.Contains(other.gameObject))
        {
            affected.Add(other.gameObject);
            Debug.Log($"ADDED {other.gameObject} to LIST OF AFFECTED PLAYERS ({affected.Count})");
            Instantiate(explosion_effect, transform.position, transform.rotation);
            state.Explosion();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (affected.Contains(other.gameObject))
        {
            affected.Remove(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("generic_wall"))
        {
            Instantiate(explosion_effect, transform.position, transform.rotation);
            state.Explosion();
        }
    }

}
