using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using Bolt.Utils;
using System;

[BoltGlobalBehaviour]
public class NetworkCallbacks : GlobalEventListener
{
    GameObject raceHandler;
    int spawnpos;
    public override void SceneLoadLocalDone(string scene)
    { 
        //GameObject startpos = GameObject.FindGameObjectWithTag($"{BoltServerIncrementer.GetNextConnectCount()}_pos");
        GameObject serverPos = GameObject.FindGameObjectWithTag("server_pos");

        //var spawnInTheCorner = new Vector3(5, 3, -15);
        PrefabId[] cars = { BoltPrefabs.Torino, BoltPrefabs.Blurino, BoltPrefabs.Truck_1, BoltPrefabs.Truck_yellow };
        //Instantiate the player vehicle
        //BoltNetwork.Instantiate(cars[Random.Range(0, cars.Length)], spawnInTheCorner, Quaternion.identity);

        //NEW PHYSICS CARS!
        if (scene == "Level_1")
        {
            
            if (BoltNetwork.IsServer)
            {
                BoltEntity serverCar = BoltNetwork.Instantiate(cars[VehicleSelectionCallbacks._i], serverPos.transform.position, serverPos.transform.rotation);
                serverCar.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                StartCoroutine(WaitAndSpawn(2));
            }
        }

    }

    public override void SceneLoadRemoteDone(BoltConnection connection)
    {
        foreach (BoltEntity bE in BoltNetwork.Entities)
        {
            if (bE.StateIs<IVehicleState>())
            {
                bE.gameObject.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }

    IEnumerator WaitAndSpawn(float time)
    {
        yield return new WaitForSeconds(time);
        raceHandler = GameObject.FindGameObjectWithTag("RaceHandler");
        spawnpos = raceHandler.GetComponent<BoltEntity>().GetState<IStateOfRace>().NumberOfPlayers;
        GameObject startpos = GameObject.FindGameObjectWithTag($"{spawnpos}_pos");
        BoltEntity Car = BoltNetwork.Instantiate(BoltPrefabs.Truck_1, startpos.transform.position, startpos.transform.rotation);
        Car.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }



    public override void OnEvent(StartTheGame evnt)
    {
        foreach (BoltEntity bE in BoltNetwork.Entities)
        {
            if (bE.StateIs<IVehicleState>())
            {
                bE.gameObject.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.None;
                bE.gameObject.transform.parent = null;
            }
        }
    }
}
