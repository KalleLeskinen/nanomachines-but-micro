using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEventSender : Bolt.EntityBehaviour<IVehicleState>
{
    GameObject raceHandler;
    bool eventFlag = false;
    bool startTheGameFlag = false;
    bool started = false;
    bool cooldown = false;
    bool raceStarted = false;

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
        if (!raceStarted && Time.frameCount % 30 == 0)
        {       
            raceStarted = raceHandler.GetComponent<BoltEntity>().GetState<IStateOfRace>().RaceStarted;
        }

        if (eventFlag)
        {
            var SpawnEvent = RespawnCar.Create();
            SpawnEvent.playerEntity = GetComponentInParent<BoltEntity>();
            SpawnEvent.SpawnPosition = state.SpawnPointID;
            SpawnEvent.Send();
            eventFlag = false;
            cooldown = true;
            StartCoroutine(StartCooldown());
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

    private IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(2.33f);
        cooldown = false;
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !cooldown && raceStarted)
        {
            eventFlag = true;
        }
        if (BoltNetwork.IsServer && !started)
        {
            int playersRdy = raceHandler.GetComponent<BoltEntity>().GetState<IStateOfRace>().PlayersReady;
            int nbOfPlayers = raceHandler.GetComponent<BoltEntity>().GetState<IStateOfRace>().NumberOfPlayers;
            bool isTheGameStarted = raceHandler.GetComponent<BoltEntity>().GetState<IStateOfRace>().RaceStarted;
            if (isTheGameStarted && playersRdy == nbOfPlayers)
            {
                //TODO: WAIT FOR 3,2,1,GO
                startTheGameFlag = true;
            }
        }
    }
}
