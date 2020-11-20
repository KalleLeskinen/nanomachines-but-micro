using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using Bolt.Utils;


[BoltGlobalBehaviour]
public class NetworkCallbacks : GlobalEventListener
{
    int connectcount = 1;
    public static List<BoltEntity> players;
    public override void SceneLoadLocalDone(string scene)
    {
        if (BoltNetwork.IsServer && scene == "Level_1")
        {
            players = new List<BoltEntity>();
            Debug.Log("#411: player list was created by the host:");
        }
        var list = BoltNetwork.Clients;
        foreach (var obj in list)
        {
            Debug.Log("#2009" + obj.ConnectionId);
            connectcount++;
            Debug.Log("#2009 cc:" + connectcount);
        }
        //level_1 spawnpos
        //Vector3 spawnPos = new Vector3(-26.23f, 4.98f, 18.7f);
        GameObject startpos = GameObject.FindGameObjectWithTag($"{connectcount}_pos");
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
        Debug.Log("#411: player was added to the list. connection:");
    }

    public override void OnEvent(RespawnCar evnt)
    {
        int spawnIndex = evnt.SpawnPosition;
        BoltEntity playerEntity = evnt.playerEntity;
        Debug.Log(playerEntity.ToString() + " should spawn at pos: " + spawnIndex);
    }

    public override void OnEvent(JoinedRoom evnt)
    {
        players.Add(evnt.playerEntity);
        Debug.Log("#411 " + players.Count);
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
