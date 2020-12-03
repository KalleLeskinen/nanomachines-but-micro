using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEventSender : Bolt.EntityBehaviour<IVehicleState>
{
    bool eventFlag = false;
    bool startTheGameFlag = false;
    public GameObject raceHandler;
    bool started = false;

    // Start is called before the first frame update
    private void Start()
    {
        if (BoltNetwork.IsServer)
        {
            raceHandler = GameObject.FindGameObjectWithTag("RaceHandler");
        }
    }

    private void FixedUpdate()
    {
        if (eventFlag)
        {
            var SpawnEvent = RespawnCar.Create();
            SpawnEvent.playerEntity = GetComponentInParent<BoltEntity>();
            SpawnEvent.SpawnPosition = state.SpawnPointID;
            SpawnEvent.Send();
            eventFlag = false;
        }
        if (startTheGameFlag && !started)
        {
            var startGame = StartTheGame.Create();
            startGame.Send();
            startTheGameFlag = false;
            FMODUnity.RuntimeManager.PlayOneShot("event:/RespawnAudio", GetComponent<Transform>().position);
            started = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && Input.GetKeyDown(KeyCode.T))
        {
            eventFlag = true;
        }
        if (BoltNetwork.IsServer && !started)
        {
            int playersRdy = raceHandler.GetComponent<BoltEntity>().GetState<IStateOfRace>().PlayersReady;
            int nbOfPlayers = raceHandler.GetComponent<BoltEntity>().GetState<IStateOfRace>().NumberOfPlayers;
            bool isTheGameStarted = raceHandler.GetComponent<BoltEntity>().GetState<IStateOfRace>().RaceStarted;
            if (isTheGameStarted && playersRdy == nbOfPlayers || Input.GetKeyDown(KeyCode.F))
            {
                //TODO: WAIT FOR 3,2,1,GO
                startTheGameFlag = true;
            }
        }
    }
}
