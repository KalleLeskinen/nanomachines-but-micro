using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchMaker : Bolt.GlobalEventListener
{
    private bool ongoing = false;
    [SerializeField]Dictionary<BoltEntity, int> carsStartPositions;
    public List<BoltEntity> players;
    public List<string> playersReady;
    [SerializeField] public Vector3[] startingPositions;
    private BoltEntity raceState;

    Vector3 pole_position;
    Vector3 second_position;

    private void Start()
    {
        raceState = GetComponent<BoltEntity>();
    }
    //private void Start()
    //{
    //    pole_position = GameObject.FindGameObjectWithTag("pole_position").transform.position;
    //    second_position = GameObject.FindGameObjectWithTag("second_position").transform.position;
    //    if (BoltNetwork.IsServer)
    //    {
    //        carsStartPositions = new Dictionary<BoltEntity, int>();
    //        startingPositions = new Vector3[6];
    //        InitStartingPositions();
    //        players = new List<BoltEntity>();
    //        playersReady = new List<string>();
    //        Debug.LogError("The script was run");
    //        StartCoroutine(WaitForAdditionalPlayers(10f));
    //    }
    //}


    //private void InitStartingPositions()
    //{
    //    Debug.Log("#232 InitStartingPositions");
    //    pole_position = GameObject.FindGameObjectWithTag("pole_position").transform.position;
    //    second_position = GameObject.FindGameObjectWithTag("second_position").transform.position;
    //    startingPositions[0] = pole_position;
    //    startingPositions[1] = second_position;
    //    int j = 1;
    //    for (int i = 2; i < 6; i++)
    //    {
    //        if (i % 2 == 0)
    //            startingPositions[i] = startingPositions[i - 2] + new Vector3(0, 0, 10.5f);
    //        else
    //            startingPositions[i] += startingPositions[i - 2] + new Vector3(0, 0, 10.5f);
    //    }
    //    //this is for level_1
    //}

    //private void StartTheGame()
    //{
    //    int i = 0;
    //    foreach (var player_entity in players)
    //    {
    //        Transform target = GameObject.FindGameObjectWithTag("pole_position").transform;
    //        Bolt.NetworkTransform playerTransform = player_entity.GetComponent<BoltEntity>().GetState<IVehicleState>().VehicleTransform;
    //        player_entity.GetState<IVehicleState>().SetTransforms(playerTransform, target);
    //        //player_entity.GetComponent<Transform>().position = startingPositions[carsStartPositions[player_entity]];
    //        i++;
    //    }
    //}

    private void ListenToReadyEvents()
    {
        if (players.Count == playersReady.Count)
        {
            //StartTheGame();
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
            evnt.playerEntity.GetState<IVehicleState>().startingPosition = players.Count;
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
    public override void OnEvent(SetCarToStartPosEvent evnt)
    {
        if (BoltNetwork.IsServer)
        {
            if (!carsStartPositions.ContainsKey(evnt.playerEntity))
                carsStartPositions.Add(evnt.playerEntity, carsStartPositions.Count);

            Debug.Log("PLAYER JOINED THE GAME #222 =>" + carsStartPositions.Count);
        }
        //if (BoltNetwork.IsConnected)
        //{
        //    if (!carsStartPositions.ContainsKey(evnt.playerEntity))
        //        carsStartPositions.Add(evnt.playerEntity, carsStartPositions.Count);
        //    Bolt.NetworkTransform playerTransform = evnt.playerEntity.GetComponent<BoltEntity>().GetState<IVehicleState>().VehicleTransform;
        //    evnt.playerEntity.GetState<IVehicleState>().ForceTransform(playerTransform, startingPositions[carsStartPositions[evnt.playerEntity]]);
        //    evnt.playerEntity.GetComponent<Transform>().position = startingPositions[carsStartPositions[evnt.playerEntity]];
        //}
    }

    //IEnumerator WaitForAdditionalPlayers(float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    if (!ongoing)
    //        StartTheGame();
    //}

}
