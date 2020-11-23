﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using System;

public class RaceScript : Bolt.EntityBehaviour<IStateOfRace>
{
    [SerializeField]
    List<GameObject> cars;

    [SerializeField] string[] sceneguids;
    public GameObject[] checkpoints;
    public int numberOfcheckpoints;
    public int numberOfLaps;
    private bool starting = false;
    [SerializeField]
    GameObject FinishLine;
    float countdownSeconds;

    public bool started = false;
    public bool finished = false;
    BoltEntity winner;

    [SerializeField] public List<PlayerData> playerDataList;

    int warmupTime = 5;

    public override void Attached()
    {
        if (BoltNetwork.IsServer)
        {
            state.NumberOfPlayers = 1;
        }
        state.Finished = false;
        state.RaceStarted = false;
        state.NumberOfLaps = numberOfLaps;
        state.NumberOfCheckpoints = numberOfcheckpoints;
        state.Clock = 0;
        SetUpTheRace();
        countdownSeconds = warmupTime;
    }


    public override void SimulateOwner()
    {
        if (Time.frameCount % 60 == 0 && state.RaceStarted && !state.Finished)
        {
            CheckForWinner();
        }
        countdownSeconds -= Time.deltaTime;
    }

    private void CheckForWinner()
    {
        for (int i = 0; i<playerDataList.Count; i++)
        {
            if (playerDataList[i].lapTimes.Count == state.NumberOfLaps)
            {
                state.Finished = true;
                RaceWinner(playerDataList[i]);
            }
        }
    }

    private void RaceWinner(PlayerData playerData)
    {
        state.Winner = playerData.id;
        
        Debug.Log($"Winner: {playerData.id}");
        int i = 0;
        foreach (var laptime in playerData.lapTimes)
        {
            Debug.Log($"{++i} : {laptime}");
        }
    }

    IEnumerator IntialiseTheGameIn(int warmupTime)
    {
        starting = true;
        GameObject.FindGameObjectWithTag("FinishLine").GetComponent<BoxCollider>().isTrigger = false;
        Debug.Log($"Starting the game in {warmupTime} seconds");
        yield return new WaitForSeconds(warmupTime);
        playerDataList = new List<PlayerData>();
        SetUpCheckPoints();
        GetAllCars();
        GetSceneGuids();
        foreach (var car in cars)
        {
            List<float> plr_laptimes = GetLapTimeList(car);
            List<int> plr_checkpoints = GetCheckpointList(car);
            Guid plr_id = GetGuid(car);
            PlayerData plr = new PlayerData(plr_id,plr_checkpoints,plr_laptimes);
            playerDataList.Add(plr);
            Debug.Log($"added {plr_id.ToString().Split('-')[0]}... with {plr_laptimes.Count} laps and {plr_checkpoints.Count} checkpoints passed");
            car.GetComponent<LapTimeUpdate>().clock = 0;
        }
        starting = false;
        GameObject.FindGameObjectWithTag("FinishLine").GetComponent<BoxCollider>().isTrigger = true;

    }

    private void SetUpCheckPoints()
    {
        checkpoints = GameObject.FindGameObjectsWithTag("checkpoint");
        numberOfcheckpoints = checkpoints.Length;
    }

    private void SetUpTheRace()
    {
        Debug.Log("SetUpTheRace");
        StartCoroutine(IntialiseTheGameIn(warmupTime));
        state.RaceStarted = true;
    }

    private void GetSceneGuids()
    {
        sceneguids = new string[cars.Count];
        int i = 0;
        Debug.Log("GetSceneGuids");
        foreach (var player in cars)
        {
            sceneguids[i++] = player.GetComponent<LapTimeUpdate>().id.ToString();
        }
    }

    private void GetAllCars()
    {
        //get all cars, the assign them dictionaries of laptimes and checkpoints
        Debug.Log("GetAllCars");
        GameObject[] carArray = GameObject.FindGameObjectsWithTag("Player");
        foreach (var car in carArray)
        {
            cars.Add(car);
        }
    }

    private List<float> GetLapTimeList(GameObject car)
    {
        return car.GetComponent<LapTimeUpdate>().lapTimes;
    }

    private List<int> GetCheckpointList(GameObject car)
    {
        return car.GetComponent<LapTimeUpdate>().car_passed_cps;
    }

    public void CheckPointPassed(GameObject Cp, Guid carId, float cp_clock, int cp_number)
    {
        for (int i = 0; i < playerDataList.Count; i++)
        {
            if (playerDataList[i].id == carId && !state.Finished)
            {
                playerDataList[i].checkpointsPassed.Add(cp_number); // auto PlayerDatassa
                Cp.GetComponent<CheckpointScript>()._material.color = Color.green;
                Debug.Log($"{playerDataList[i].id.ToString().Split('-')[0]}... cp:{playerDataList[i].checkpointsPassed.Count}/{numberOfcheckpoints} ({cp_clock})");
            }
        }
    }

    public void FinishLinePassed(GameObject car, Guid carId, float time)
    {
        for (int i = 0; i<playerDataList.Count; i++)
        {
            if (playerDataList[i].id == carId && playerDataList[i].checkpointsPassed.Count % numberOfcheckpoints == 0)
            {
                playerDataList[i].lapTimes.Add(time);
                playerDataList[i].checkpointsPassed.Clear();
                Debug.Log($"player {carId} crossed the finish line at {time}");
                for (int cp = 0; cp<checkpoints.Length; cp++)
                {
                    Debug.Log("reseting colors");
                    checkpoints[cp].GetComponent<CheckpointScript>()._material.color = Color.red;
                }
                Debug.Log("setting up the cp list again");
                playerDataList[i].checkpointsPassed = GetCheckpointList(car);
            }
        }
    }

    private void CountTime()
    {
        state.Clock += Time.deltaTime;
    }
    private Guid GetGuid(GameObject car)
    {
        return car.GetComponent<LapTimeUpdate>().id;
    }
    private void OnGUI()
    {
        //if (state.Finished && state.RaceStarted)
        //    GUI.Box(new Rect(100, 100, 200, 50), state.Winner.ToString());
        if (starting == true)
        {
            string theString = $"Starting the race in {(int)countdownSeconds+1}";
            GUI.Box(new Rect((Screen.width / 2) / 2, (Screen.height / 2)/2, Screen.width / 4, 30), theString);
        }
    }
}

