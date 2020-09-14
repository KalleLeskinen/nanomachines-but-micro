using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;


[BoltGlobalBehaviour]
public class NetworkCallbacks : GlobalEventListener
{

    public override void SceneLoadLocalDone(string scene)
    {
        
        //Random position on table
        var spawnPos = new Vector3(Random.Range(-2, 2), 1, -4);

        //Insantiate the player vehicle
        BoltNetwork.Instantiate(BoltPrefabs.temprallycar, spawnPos, Quaternion.identity);


    }


}
