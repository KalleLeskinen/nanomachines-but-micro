using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Bolt;
using Bolt.Utils;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class BoltServerCallbacks : Bolt.GlobalEventListener
{
    public override void SceneLoadLocalDone(string scene)
    {
    
    }
    public override void SceneLoadRemoteDone(BoltConnection connection)
    {
        Debug.Log("#213" + connection.ToString());
    }

}
