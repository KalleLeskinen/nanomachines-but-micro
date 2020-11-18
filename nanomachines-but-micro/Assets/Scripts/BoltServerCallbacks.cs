using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Bolt;
using Bolt.Utils;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class BoltServerCallbacks : Bolt.GlobalEventListener
{
    public List<int> players;

    public override void SceneLoadLocalDone(string scene)
    {
        players = new List<int>();
        players.Add(0);
    }
    public override void SceneLoadRemoteDone(BoltConnection connection)
    {
        players.Add(players.Count);
        Debug.Log("#213" + connection.ToString());
    }

}
