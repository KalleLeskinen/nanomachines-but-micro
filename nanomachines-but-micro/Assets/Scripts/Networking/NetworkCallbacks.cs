using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using Bolt.Utils;
using System;
using UnityEngine.UI;

[BoltGlobalBehaviour]
public class NetworkCallbacks : GlobalEventListener
{
    GameObject raceHandler;
    int spawnpos;
    public GameObject scoreboard_ui;

    private void Awake()
    {
    }
    public override void SceneLoadLocalDone(string scene)
    { 
        scoreboard_ui = GameObject.FindGameObjectWithTag("score_panel");
        //GameObject startpos = GameObject.FindGameObjectWithTag($"{BoltServerIncrementer.GetNextConnectCount()}_pos");
        GameObject serverPos = GameObject.FindGameObjectWithTag("server_pos");

        //var spawnInTheCorner = new Vector3(5, 3, -15);
        PrefabId[] cars = { BoltPrefabs.Torino, BoltPrefabs.Blurino, BoltPrefabs.Truck_1, BoltPrefabs.Truck_yellow };
        //Instantiate the player vehicle
        //BoltNetwork.Instantiate(cars[Random.Range(0, cars.Length)], spawnInTheCorner, Quaternion.identity);

        //NEW PHYSICS CARS!
        if (scene == "Level_1")
        {
            
            if (BoltNetwork.IsServer)
            {
                BoltEntity serverCar = BoltNetwork.Instantiate(cars[SelectionContainer.Instance.prefabIdInteger], serverPos.transform.position, serverPos.transform.rotation);
                serverCar.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                StartCoroutine(WaitAndSpawn(2f));
            }
            
        }


    }

    public override void SceneLoadRemoteDone(BoltConnection connection)
    {
        foreach (BoltEntity bE in BoltNetwork.Entities)
        {
            if (bE.StateIs<IVehicleState>())
            {
                bE.gameObject.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }

        }
        GameObject.FindGameObjectWithTag("RaceHandler").GetComponent<RaceScript>().UpdatePlayerBase();
    }

    IEnumerator WaitAndSpawn(float time)
    {
        yield return new WaitForSeconds(time);
        raceHandler = GameObject.FindGameObjectWithTag("RaceHandler");
        spawnpos = raceHandler.GetComponent<BoltEntity>().GetState<IStateOfRace>().NumberOfPlayers;
        GameObject startpos = GameObject.FindGameObjectWithTag($"{spawnpos}_pos");
        BoltEntity Car = BoltNetwork.Instantiate(BoltPrefabs.Truck_1, startpos.transform.position, startpos.transform.rotation);
        Car.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        GameObject.FindGameObjectWithTag("RaceHandler").GetComponent<RaceScript>().UpdatePlayerBase();
    }



    public override void OnEvent(StartTheGame evnt)
    {
        foreach (BoltEntity bE in BoltNetwork.Entities)
        {
            if (bE.StateIs<IVehicleState>())
            {
                bE.gameObject.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.None;
                bE.gameObject.transform.parent = null;
            }
        }
    }

    public override void OnEvent(PlayerReadyEvent evnt)
    {
        if (BoltNetwork.IsServer)
        {

            foreach (BoltEntity bE in BoltNetwork.Entities)
            {
                if (bE.StateIs<IStateOfRace>())
                {
                    bE.GetState<IStateOfRace>().PlayersReady += 1;
                }
            }
        }
    }
    public override void OnEvent(HostReadyEvent evnt)
    {
        if (BoltNetwork.IsServer)
        {
            foreach (BoltEntity bE in BoltNetwork.Entities)
            {
                if (bE.StateIs<IStateOfRace>())
                {
                    bE.GetState<IStateOfRace>().PlayersReady += 1;
                }
            }
        }
    }
    public override void OnEvent(CarFinished evnt)
    {
        string winner;
        string winnerBestLap;
        string second;
        string secondBestLap;
        string third;
        string thirdBestLap;
        foreach (BoltEntity bE in BoltNetwork.Entities)
        {
            if (bE.StateIs<IStateOfRace>() && bE.TryFindState<IStateOfRace>(out IStateOfRace state))
            {
                winner = state.Winner;
                winnerBestLap = state.WinBestLap.ToString();
                second = state.Second;
                secondBestLap = state.SecBestLap.ToString();
                third = state.Third;
                thirdBestLap = state.ThrBestLap.ToString();

                GameObject.FindGameObjectWithTag("scoreboard_winner").GetComponent<Text>().text = $"1. {winner}";
                GameObject.FindGameObjectWithTag("best_lap_winner").GetComponent<Text>().text = $"best lap: {winnerBestLap} s";

                GameObject.FindGameObjectWithTag("scoreboard_second").GetComponent<Text>().text = $"2. {second}";
                if (secondBestLap != "0")
                    GameObject.FindGameObjectWithTag("best_lap_second").GetComponent<Text>().text = $"best lap: {secondBestLap} s";

                GameObject.FindGameObjectWithTag("scoreboard_third").GetComponent<Text>().text = $"3. {third}";
                if (thirdBestLap != "0")
                    GameObject.FindGameObjectWithTag("best_lap_third").GetComponent<Text>().text = $"best lap: {thirdBestLap} s";
            }
        }
    }
}
