using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Bolt;
using Bolt.Matchmaking;
using UdpKit;

public class MainMenu : GlobalEventListener
{
    
    public void ButtonStartServer()
    {
        BoltLauncher.StartServer();
    }

    public void ButtonStartClient()
    {
        BoltLauncher.StartClient();
    }

    public void ButtonQuitGame()
    {
        Application.Quit();
    }

    public override void BoltStartDone()
    {
        if (BoltNetwork.IsServer)
        {
            string matchName = System.Guid.NewGuid().ToString();

            BoltMatchmaking.CreateSession(//
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
