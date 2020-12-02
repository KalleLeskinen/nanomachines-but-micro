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
    int car_to_spawn;

    private void Awake()
    {
    }
    public override void SceneLoadLocalDone(string scene)
    { 
        scoreboard_ui = GameObject.FindGameObjectWithTag("score_panel");
        GameObject selection_container = GameObject.FindGameObjectWithTag("selection_data_container");
        if (selection_container == null)
        {
            car_to_spawn = 0; //torino if player has not chosen one from the main menu
        } else
        {
            car_to_spawn = SelectionContainer.Instance.prefabIdInteger;
        }
        //GameObject startpos = GameObject.FindGameObjectWithTag($"{BoltServerIncrementer.GetNextConnectCount()}_pos");
        GameObject serverPos = GameObject.FindGameObjectWithTag("server_pos");

        //var spawnInTheCorner = new Vector3(5, 3, -15);
        PrefabId[] cars = { BoltPrefabs.Torino, BoltPrefabs.Blurino, BoltPrefabs.Splitrino, BoltPrefabs.Truck_1_green, BoltPrefabs.Truck_1_blue_orange, BoltPrefabs.Truck_2_yellow, BoltPrefabs.Truck_2_black };
        //Instantiate the player vehicle
        //BoltNetwork.Instantiate(cars[Random.Range(0, cars.Length)], spawnInTheCorner, Quaternion.identity);

        //NEW PHYSICS CARS!
        if (scene == "Level_1")
        {
            
            if (BoltNetwork.IsServer)
            {
                BoltEntity serverCar = BoltNetwork.Instantiate(cars[car_to_spawn], serverPos.transform.position, serverPos.transform.rotation);
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
        PrefabId[] cars = { BoltPrefabs.Torino, BoltPrefabs.Blurino, BoltPrefabs.Splitrino, BoltPrefabs.Truck_1_green, BoltPrefabs.Truck_1_blue_orange, BoltPrefabs.Truck_2_yellow, BoltPrefabs.Truck_2_black };
        raceHandler = GameObject.FindGameObjectWithTag("RaceHandler");
        spawnpos = raceHandler.GetComponent<BoltEntity>().GetState<IStateOfRace>().NumberOfPlayers;
        GameObject startpos = GameObject.FindGameObjectWithTag($"{spawnpos}_pos");
        BoltEntity Car = BoltNetwork.Instantiate(cars[car_to_spawn], startpos.transform.position, startpos.transform.rotation);
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
        GameObject players_ui = GameObject.FindGameObjectWithTag("connected_players_ui");
        foreach (var textField in players_ui.GetComponentsInChildren<Text>())
        {
            if (textField.text == "")
            {
                textField.text = evnt.name;
                break;
            }
        }
    }
    public override void OnEvent(HostReadyEvent evnt)
    {
        GameObject players_ui = GameObject.FindGameObjectWithTag("connected_players_ui");
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
        foreach (var textField in players_ui.GetComponentsInChildren<Text>())
        {
            if (textField.text == "")
            { 
                textField.text = evnt.name;
                break;
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
