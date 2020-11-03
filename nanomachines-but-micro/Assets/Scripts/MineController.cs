using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineController : Bolt.EntityBehaviour<ILandMineState>
{
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
        foreach (var car in affected)
            car.GetComponent<OnHitController>().Explode(); //autolle kutsuttava räjähdys

        // tähän räjähdysanimaatio ja äänet!

        // tuhoa miina kun kaikki autot on räjäytetty
        StartCoroutine(FindAllCarsInExplosionRadius());
    }
    IEnumerator FindAllCarsInExplosionRadius()
    {
        yield return new WaitForSeconds(0.05f);
        BoltNetwork.Destroy(this.gameObject);
        Debug.Log($"EXPLODED {affected.Count} CARS AND DELETED THE MINE!" );
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
            state.Explosion();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;
        Debug.Log("CAR DROVE OVER THE MINE..........EXPLODING!");
        affected.Add(other.gameObject);
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
