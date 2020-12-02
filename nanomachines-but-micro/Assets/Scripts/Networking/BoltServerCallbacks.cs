using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Bolt;
using Bolt.Utils;
using System;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class BoltServerCallbacks : Bolt.GlobalEventListener
{

    public override void SceneLoadLocalDone(string scene)
    {
        
        // var spawnEvent = SpawnOnJoin.Create();
        // spawnEvent.id = 0;
        // spawnEvent.Send();
        //
        // players = new List<int>();
        // players.Add(0);
    }
    public override void SceneLoadRemoteDone(BoltConnection connection)
    {
        
        // var spawnEvent = SpawnOnJoin.Create();
        // spawnEvent.id = players.Count;
        //
        // spawnEvent.Send();
        //
        //
        // players.Add(players.Count);
        // Debug.Log("#213" + connection.ToString());
    }

    /*public override void OnEvent(SpawnOnJoin evnt)
    {
        var car = SpawnPlayer(evnt.id);
        car.AssignControl(evnt.RaisedBy);
    }

    private BoltEntity SpawnPlayer(int id)
    {
        BoltEntity car = BoltNetwork.Instantiate(BoltPrefabs.Truck_1);
        car.transform.position = new Vector3(id*2, id, id*2);
        return car;
    }*/


}
