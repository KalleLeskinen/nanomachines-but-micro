using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[BoltGlobalBehaviour]
public class MatchMaker : Bolt.GlobalEventListener
{
    private bool ongoing = false;
    public List<BoltEntity> players;
    public List<string> playersReady;


    private void Start()
    {
        if (BoltNetwork.IsServer)
        {
            players = new List<BoltEntity>();
            playersReady = new List<string>();
            Debug.LogError("The script was run");
            StartCoroutine(WaitForAdditionalPlayers(3f));
        }
    }

    private void StartTheGame()
    {
        foreach (var player_entity in players)
        {
            player_entity.GetComponent<Transform>().position = new Vector3(0, 15, 0);
        }
    }

    private void ListenToReadyEvents()
    {
        if (players.Count == playersReady.Count)
        {
            StartTheGame();
            Debug.LogError("Game starteD!");
            ongoing = true;
        }
    }

    public override void Connected(BoltConnection connection)
	{
		Debug.Log(connection.ToString() + " connected to us.");
	}
    public override void OnEvent(JoinedRoom evnt)
    {
        if (BoltNetwork.IsServer)
        {
            players.Add(evnt.playerEntity);
        }
    }
    public override void OnEvent(PlayerReadyEvent evnt)
    {
        if (BoltNetwork.IsServer)
        {
            Debug.LogWarning(evnt.playerGuid);
            string playerGuid = evnt.playerGuid;
            if (!playersReady.Contains(playerGuid) && !playerGuid.StartsWith("00000000"))
            {
                playersReady.Add(playerGuid);
            }
            Debug.LogWarning(playersReady.Count + " players ready!");
        }
    }
    public override void OnEvent(HostReadyEvent evnt)
    {
        if (BoltNetwork.IsServer)
        {
            Debug.LogWarning(evnt.playerGuid);
            string playerGuid = evnt.playerGuid;
            if (!playersReady.Contains(playerGuid) && !playerGuid.StartsWith("00000000"))
            {
                playersReady.Add(playerGuid);
            }
            Debug.LogWarning(playersReady.Count + " players ready!");
        }
    }
    IEnumerator WaitForAdditionalPlayers(float time)
    {
        Debug.LogError("waiting... starting...");
        yield return new WaitForSeconds(time);
        if (!ongoing)
            StartTheGame();
    }

}
