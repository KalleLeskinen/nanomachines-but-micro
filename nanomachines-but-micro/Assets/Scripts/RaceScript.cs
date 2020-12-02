using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RaceScript : Bolt.EntityBehaviour<IStateOfRace>
{
    [SerializeField]
    List<GameObject> cars;
    private bool played = false;
    [SerializeField] string[] sceneguids;
    public GameObject[] checkpoints;
    public int numberOfcheckpoints;
    public int numberOfLaps;
    private bool starting = false;
    [SerializeField]
    GameObject FinishLine;
    float countdownSeconds;

    public GameObject scoreboard_ui;
    public GameObject player_laptime_ui;
    public GameObject connected_players_ui;
    public GameObject ui_3_2_1_go;

    public bool started = false;
    public bool finished = false;
    BoltEntity winner;

    [SerializeField] public List<PlayerData> playerDataList;

    int warmupTime = 15;

    public bool StartFlag = false;

    public override void Attached()
    {
        Debug.Log("#666 STARTING");
        if (BoltNetwork.IsServer)
        {
            state.NumberOfPlayers = 1;
            state.Finished = false;
            state.RaceStarted = false;
            state.NumberOfLaps = numberOfLaps;
            state.NumberOfCheckpoints = numberOfcheckpoints;
            state.Clock = 15;
            //SetUpTheRace();
            countdownSeconds = warmupTime;
            state.Winner = "";
            state.Second = "";
            state.Third = "";
        }
    }

    private void Update()
    {
        if (Time.frameCount % 30 == 0 && Time.timeSinceLevelLoad > 3 && state.Clock < 0 && state.Clock > -1 && !StartFlag)
        {
            StartRace();
        }

    }
    private void FixedUpdate()
    {
        if (state.Clock < 3.3f && !played)
        {
            played = true;
            ui_3_2_1_go.SetActive(true);
            ui_3_2_1_go.GetComponent<Animation>().Play();
        }
    }

    public override void SimulateOwner()
    {
        CountTime(); //servu laskee staten aikaa
    }

    private void StartRace() //tämä pitää alustaa jokaisen pelaajan itse
    {
        StartFlag = true;
        playerDataList = new List<PlayerData>();
        SetUpCheckPoints();
        GetAllCars();
        GetSceneGuids();
        foreach (var car in cars)
        {
            List<float> plr_laptimes = GetLapTimeList(car);
            List<int> plr_checkpoints = GetCheckpointList(car);
            Guid plr_id = GetGuid(car);
            string plr_name = car.GetComponentInParent<BoltEntity>().GetState<IVehicleState>().PlayerName;
            PlayerData plr = new PlayerData(plr_id, plr_checkpoints, plr_laptimes, plr_name);
            playerDataList.Add(plr);
            Debug.Log($"added {plr_name}... with {plr_laptimes.Count} laps and {plr_checkpoints.Count} checkpoints passed");
            car.GetComponent<LapTimeUpdate>().clock = 0;
        }
        player_laptime_ui.SetActive(true);
        connected_players_ui.SetActive(false);
        ui_3_2_1_go.SetActive(false);
        
        if (BoltNetwork.IsServer)
            state.RaceStarted = true;
    }

    public void UpdatePlayerBase()
    {
        foreach (var car in cars)
        {
            playerDataList = new List<PlayerData>();
            List<float> plr_laptimes = GetLapTimeList(car);
            List<int> plr_checkpoints = GetCheckpointList(car);
            Guid plr_id = GetGuid(car);
            string plr_name = car.GetComponentInParent<BoltEntity>().GetState<IVehicleState>().PlayerName;
            PlayerData plr = new PlayerData(plr_id, plr_checkpoints, plr_laptimes, plr_name);
            playerDataList.Add(plr);
            Debug.Log($"added {plr_name}... with {plr_laptimes.Count} laps and {plr_checkpoints.Count} checkpoints passed");
            car.GetComponent<LapTimeUpdate>().clock = 0;
        }
    }


    private void CheckForWinner()
    {
        Debug.Log("cheking for winer #22");
        for (int i = 0; i<playerDataList.Count; i++)
        {
            if (playerDataList[i].lapTimes.Count == state.NumberOfLaps)
            {
                if (state.Winner == "")
                {
                    if (BoltNetwork.IsServer)
                    {
                        RaceFinished(playerDataList[i], 1);
                        StartCoroutine(WaitAndSendEvent());
                    }
                    playerDataList.Remove(playerDataList[i]); // voittajan kierroksia ei enää lasketa
                    Debug.Log("#22 winner found");
                }
                else if (state.Second == "")
                {
                    if (BoltNetwork.IsServer)
                    {
                        RaceFinished(playerDataList[i], 2);
                        StartCoroutine(WaitAndSendEvent());
                        if (cars.Count == 2)
                        {
                            state.Finished = true;
                        }
                    }
                    playerDataList.Remove(playerDataList[i]); // voittajan kierroksia ei enää lasketa
                    Debug.Log("#22 second found");

                }
                else if (state.Third == "")
                {
                    if (BoltNetwork.IsServer)
                    {
                        RaceFinished(playerDataList[i], 3);
                        StartCoroutine(WaitAndSendEvent());
                    }
                    playerDataList.Remove(playerDataList[i]); // voittajan kierroksia ei enää lasketa
                    Debug.Log("#22 third found");
                    state.Finished = true;
                }
                scoreboard_ui.SetActive(true);
            }
        }
    }


    private void RaceFinished(PlayerData playerData, int position)
    {
        switch (position)
        {
            case 1:
                state.Winner = playerData.name;
                state.WinBestLap = playerData.FindBestLap();
                break;
            case 2:
                state.Second = playerData.name;
                state.SecBestLap = playerData.FindBestLap();
                break;
            case 3:
                state.Third = playerData.name;
                state.ThrBestLap = playerData.FindBestLap();
                break;
        }
        int i = 0;
        foreach (var laptime in playerData.lapTimes)
        {
            Debug.Log($"{++i} : {laptime}");
        }
        StartCoroutine(WaitFor30AndEnd());
    }

    private IEnumerator WaitFor30AndEnd()
    {
        Debug.Log("Waiting for 30 second");
        yield return new WaitForSeconds(25f);
        //aloita animaatio fade in 5 sekunnisksi?
        yield return new WaitForSeconds(5f);
        BoltNetwork.LoadScene("MainMenu");
        //joku muu keino kickaa pelaajat hellävarasesti main menuun
    }

    private void SetUpCheckPoints()
    {
        checkpoints = GameObject.FindGameObjectsWithTag("checkpoint");
        numberOfcheckpoints = checkpoints.Length;
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
            if (playerDataList[i].id == carId /*&& !state.Finished*/)
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
        CheckForWinner();
    }

    IEnumerator WaitAndSendEvent()
    {
        yield return new WaitForSeconds(1);
        var FinishedEvnt = CarFinished.Create();
        FinishedEvnt.Send();
    }

    private void CountTime()
    {
        state.Clock -= Time.deltaTime;
    }
    private Guid GetGuid(GameObject car)
    {
        return car.GetComponent<LapTimeUpdate>().id;
    }
}

