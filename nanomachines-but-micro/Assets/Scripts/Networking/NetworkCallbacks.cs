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

        //level_1 spawnpos
        Vector3 spawnPos = new Vector3(-26.23f, 4.98f, 18.7f);


        //var spawnInTheCorner = new Vector3(5, 3, -15);
        PrefabId[] cars = { BoltPrefabs.Car1_Torino, BoltPrefabs.Car2_Torino, BoltPrefabs.TruckV1, BoltPrefabs.TruckV2 };
        //Instantiate the player vehicle
        //BoltNetwork.Instantiate(cars[Random.Range(0, cars.Length)], spawnInTheCorner, Quaternion.identity);

        //NEW PHYSICS CARS!

        BoltEntity car = BoltNetwork.Instantiate(BoltPrefabs.Truck_1);
        car.transform.position = spawnPos;

        //PlayerCamera.Instantiate();
        //Tehkää prefab kamerasta, johon post-processingit yms.
        //if entity.isOwner??
        //LocalEvents.Instance.CameraInstantiate(newCar);

    }
}
