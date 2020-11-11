using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineController : Bolt.EntityBehaviour<ILandMineState>
{
    public GameObject explosion_effect;
    public float trigger_time;
    public SphereCollider explosion_collider;
    public List<GameObject> affected;
    public override void Attached()
    {
        state.DetonateTime = 20f;
        state.OnExplosion += handlerExplosion;
    }

    private void handlerExplosion()
    {
        // ota käyttöös uusi iso sphere collider
        explosion_collider.enabled = true;
        // tähän räjähdysanimaatio ja äänet!
        //
        // tuhoa miina kun kaikki autot on räjäytetty
        StartCoroutine(FindAllCarsInExplosionRadius());
        //auto(i)lle kutsuttava räjähdys
        foreach (var car in affected)
            car.GetComponent<OnHitController>().Explode();
        BoltNetwork.Destroy(this.gameObject);

    }
    IEnumerator FindAllCarsInExplosionRadius()
    {
        yield return new WaitForSeconds(0.075f);
        Debug.Log($"EXPLODED {affected.Count} CARS" );
    }

    IEnumerator ExplodeIn(float time)
    {
        yield return new WaitForSeconds(time);
        state.Explosion();
    }

    public override void SimulateOwner()
    {
        state.DetonateTime = state.DetonateTime - Time.deltaTime;
        if (state.DetonateTime < 0)
        {
            Instantiate(explosion_effect, transform.position, transform.rotation);
            state.Explosion();
        }
        if (Time.frameCount % 50 == 0)
        {
            gameObject.GetComponentsInChildren<Renderer>()[1].material.EnableKeyword("_EMISSION");
        }
        if (Time.frameCount % 25 == 0 && Time.frameCount % 50 != 0)
            gameObject.GetComponentsInChildren<Renderer>()[1].material.DisableKeyword("_EMISSION");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;
        Debug.Log("CAR DROVE OVER THE MINE..........EXPLODING!");
        affected.Add(other.gameObject);
        Instantiate(explosion_effect, transform.position, transform.rotation);
        StartCoroutine(ExplodeIn(trigger_time));

    }
    private void OnTriggerExit(Collider other)
    {
        if(!other.gameObject.CompareTag("Player"))
            return;
        if (affected.Contains(other.gameObject))
        {
            affected.Remove(other.gameObject);
        }
    }
}
