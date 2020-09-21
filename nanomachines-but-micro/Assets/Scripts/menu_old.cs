using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using Bolt.Matchmaking;
using UdpKit;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Menu : GlobalEventListener
{
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));

        if (GUILayout.Button("Start Server", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
        {
            // START SERVER
            BoltLauncher.StartServer();
        }

        if (GUILayout.Button("Start Client", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
        {
            // START CLIENT
            BoltLauncher.StartClient();
        }

        GUILayout.EndArea();
    }

    public override void BoltStartDone()
    {
        if (BoltNetwork.IsServer)
        {
            string matchName = System.Guid.NewGuid().ToString();

            BoltMatchmaking.CreateSession(
                sessionID: matchName,
                sceneToLoad: "PhotonTest" // <-  What scene to load
            );
        }
    }

    public override void SessionListUpdated(Map<System.Guid, UdpSession> sessionList)
    {
        Debug.LogFormat("Session list updated: {0} total sessions", sessionList.Count);

        foreach (var session in sessionList)
        {
            UdpSession photonSession = session.Value as UdpSession;

            if (photonSession.Source == UdpSessionSource.Photon)
            {
                BoltMatchmaking.JoinSession(photonSession);
            }
        }
    }


}
