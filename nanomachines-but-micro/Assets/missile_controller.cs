using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile_controller : Bolt.EntityBehaviour<IRocketState>
{
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
        // tarkista mitkä autot on räjähdysalueen sisällä
        foreach (var car in affected)   // räjäytä autot
        {
            car.GetComponent<OnHitController>().Explode(); //autolle kutsuttava räjähdys
        }
        // tuhoa missile kun kaikki autot on räjäytetty
        StartCoroutine(FindAllCarsInExplosionRadius());

        // tähän räjähdysanimaatio ja äänet!

    }

    IEnumerator FindAllCarsInExplosionRadius()
    {
        yield return new WaitForSeconds(0.05f);
        BoltNetwork.Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        state.DetonateTime -= Time.deltaTime;
        gameObject.transform.Translate(Vector3.forward * Time.deltaTime * 15f);
        if (state.DetonateTime < 0)
        {
            state.Explosion();
            BoltNetwork.Destroy(this.gameObject);
        }   // räjähdys / ei räjähdystä vaan katoaminen? (lähellä ei autoja.......)
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") && !affected.Contains(other.gameObject))
        {
            affected.Add(other.gameObject);
            Debug.Log($"ADDED {other.gameObject} to LIST OF AFFECTED PLAYERS ({affected.Count})");
        }
        state.Explosion();
    }
    private void OnTriggerExit(Collider other)
    {
        if (affected.Contains(other.gameObject))
        {
            affected.Remove(other.gameObject);
        }
    }
}
