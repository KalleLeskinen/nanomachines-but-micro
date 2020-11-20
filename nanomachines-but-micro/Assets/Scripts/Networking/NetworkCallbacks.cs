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

        //level_1 spawnpos
        //Vector3 spawnPos = new Vector3(-26.23f, 4.98f, 18.7f);
        GameObject startpos = GameObject.FindGameObjectWithTag($"{BoltServerIncrementer.connectCount}_pos");
        GameObject serverPos = GameObject.FindGameObjectWithTag("server_pos");
        //var spawnInTheCorner = new Vector3(5, 3, -15);
        PrefabId[] cars = { BoltPrefabs.Car1_Torino, BoltPrefabs.Car2_Torino, BoltPrefabs.TruckV1, BoltPrefabs.TruckV2 };
        //Instantiate the player vehicle
        //BoltNetwork.Instantiate(cars[Random.Range(0, cars.Length)], spawnInTheCorner, Quaternion.identity);

        //NEW PHYSICS CARS!
        if (scene == "Level_1")
        {
            if (BoltNetwork.IsServer)
            {
                BoltEntity serverCar = BoltNetwork.Instantiate(BoltPrefabs.Truck_1, serverPos.transform.position , serverPos.transform.rotation);
            } else
            {
                BoltEntity Car = BoltNetwork.Instantiate(BoltPrefabs.Truck_1, startpos.transform.position, startpos.transform.rotation);
            }
            //car.GetState<IVehicleState>().SpawnPointID = ++playedNo;
        }
    }

    public override void Connected(BoltConnection connection)
    {
        BoltServerIncrementer.connectCount++;
    }

    public override void OnEvent(RespawnCar evnt)
    {
        int spawnIndex = evnt.SpawnPosition;
        BoltEntity playerEntity = evnt.playerEntity;
    }

    public override void OnEvent(JoinedRoom evnt)
    {
        players.Add(evnt.playerEntity);
    }

    //public override void OnEvent(StartTheGame evnt)
    //{
    //    int playerNumber = 0;
    //    foreach (BoltEntity bE in BoltNetwork.Entities)
    //    {
    //        if (bE.StateIs<IVehicleState>())
    //        {
    //            var omistaja = bE.Source;
    //            Vector3 startpos = GameObject.FindGameObjectWithTag($"{++playerNumber}_pos").transform.position;
    //            bE.GetState<IVehicleState>().SetTeleport(bE.GetState<IVehicleState>().VehicleTransform);
    //            bE.GetComponentsInChildren<Transform>()[1].position = startpos;
    //        }
    //    }
    //}
}
