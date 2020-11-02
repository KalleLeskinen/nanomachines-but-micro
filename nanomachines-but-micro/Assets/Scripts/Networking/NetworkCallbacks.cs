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
        //Camera camera = new Camera();
        // LocalEvents.Instance.OnCarInstantiate += 
        //Random position on table
        //var spawnPos = new Vector3(Random.Range(-2, 2), 1, -4);
        var spawnInTheCorner = new Vector3(5, 3, -15);
        //Insantiate the player vehicle
        BoltNetwork.Instantiate(BoltPrefabs.Car1_Torino, spawnInTheCorner, Quaternion.identity);
        //PlayerCamera.Instantiate();
        //Tehkää prefab kamerasta, johon post-processingit yms.
        //if entity.isOwner??
        //LocalEvents.Instance.CameraInstantiate(newCar);

    }
}
