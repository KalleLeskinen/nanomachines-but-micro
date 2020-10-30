using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineController : Bolt.EntityBehaviour<ILandMineState>
{
    public Rigidbody[] rb;
    public List<GameObject> affected;
    public override void Attached()
    {
        //rb = GetComponents<Rigidbody>();
        state.DetonateTime = 10f;
        state.OnExplosion += handlerExplosion;
    }

    private void handlerExplosion()
    {
        foreach (var car in affected)
            car.GetComponent<OnHitController>().Explode(); //autolle kutsuttava räjähdys

        BoltNetwork.Destroy(this.gameObject);
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
        affected.Add(other.gameObject);

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
