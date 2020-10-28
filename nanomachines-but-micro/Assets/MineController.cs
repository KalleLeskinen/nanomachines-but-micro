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
    }
    public override void SimulateOwner()
    {
        state.DetonateTime = state.DetonateTime - Time.deltaTime;
        if (state.DetonateTime < 0)
        {
            foreach(var car in affected)
            {
                Vector3 underneath = new Vector3(car.transform.position.x, car.transform.position.y - 2, car.transform.position.z);
                car.GetComponent<Rigidbody>().AddExplosionForce(1000000f, underneath, 10f);
                Debug.Log("exploded");
            }
            BoltNetwork.Destroy(this.gameObject);
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
