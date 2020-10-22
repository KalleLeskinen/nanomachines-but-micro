using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using Bolt.Utils;


[BoltGlobalBehaviour]
public class NetworkCallbacks : GlobalEventListener
{
    public override void SceneLoadLocalDone(string scene)
    {
        // LocalEvents.Instance.OnCarInstantiate += 
        //Random position on table
        var spawnPos = new Vector3(Random.Range(-2, 2), 1, -4);

        //Insantiate the player vehicle
        var newCar = BoltNetwork.Instantiate(BoltPrefabs.Car1_Torino, spawnPos, Quaternion.identity);
        //Tehkää prefab kamerasta, johon post-processingit yms.
        //if entity.isOwner??
        LocalEvents.Instance.CameraInstantiate(newCar);
    }
}
