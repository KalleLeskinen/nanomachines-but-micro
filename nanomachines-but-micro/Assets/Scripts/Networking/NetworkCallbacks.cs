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
        var spawnPos = new Vector3(Random.Range(-2, 2), 1, -4);
        PrefabId[] cars = { BoltPrefabs.Car1_Torino, BoltPrefabs.Car2_Torino, BoltPrefabs.TruckV1, BoltPrefabs.TruckV2 };
        //Insantiate the player vehicle
        BoltNetwork.Instantiate(cars[Random.Range(0,cars.Length)], spawnPos, Quaternion.identity);
        //PlayerCamera.Instantiate();
        //Tehkää prefab kamerasta, johon post-processingit yms.
        //if entity.isOwner??
        //LocalEvents.Instance.CameraInstantiate(newCar);

    }
}
