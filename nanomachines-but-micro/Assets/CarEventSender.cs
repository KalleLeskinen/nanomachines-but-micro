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

    private void FixedUpdate()
    {
        if (!raceStarted && Time.frameCount % 30 == 0 && Time.timeSinceLevelLoad>3)
        {
            if (!raceHandler)
                raceHandler = GameObject.FindGameObjectWithTag("RaceHandler");
            raceStarted = raceHandler.GetComponent<BoltEntity>().GetState<IStateOfRace>().RaceStarted;
        }

        if (eventFlag)
        {
            if (entity.IsOwner)
            {
                var SpawnEvent = RespawnCar.Create();
                SpawnEvent.playerEntity = GetComponentInParent<BoltEntity>();
                SpawnEvent.SpawnPosition = state.SpawnPointID;
                SpawnEvent.Send();
                eventFlag = false;
                cooldown = true;
                StartCoroutine(StartCooldown());
            }
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
        if (BoltNetwork.IsServer && !started && Time.timeSinceLevelLoad>2.5f)
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
