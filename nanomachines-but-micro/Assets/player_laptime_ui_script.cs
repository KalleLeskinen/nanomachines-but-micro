using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_laptime_ui_script : MonoBehaviour
{
    private bool isFinished;
    private GameObject myCar;
    public int maxLaps;
    Guid carId;
    int playerDataIndex;
    private void Start()
    {
        maxLaps = 999;
    }
    void Update()
    {
        if (Time.timeSinceLevelLoad > 1 && maxLaps == 999)
        {
            maxLaps = GameObject.FindGameObjectWithTag("RaceHandler").GetComponent<BoltEntity>().GetState<IStateOfRace>().NumberOfLaps;
            carId = myCar.GetComponentInChildren<LapTimeUpdate>().id;
        }
        if (Time.timeSinceLevelLoad > 1 && myCar == null)
        {
            foreach (var car in BoltNetwork.Entities)
            {
                if (car.TryFindState<IVehicleState>(out IVehicleState state) && car.IsOwner)
                {
                    myCar = car;
                }
            }
            int i = 0;
            foreach (var player in GameObject.FindGameObjectWithTag("RaceHandler").GetComponent<RaceScript>().playerDataList)
            {
                if (player.id == carId)
                {
                    playerDataIndex = i;
                }
                i++;
            }
        }
        if (Time.frameCount % 30 == 0)
        {
            UpdateUi();
        }
    }
    void UpdateUi()
    {
        if (Time.timeSinceLevelLoad > 5)
        {
            GetComponentsInChildren<Text>()[0].text = myCar.GetComponentInChildren<LapTimeUpdate>().GetClockFormatted();
            GetComponentsInChildren<Text>()[1].text = $"{myCar.GetComponentInChildren<LapTimeUpdate>().car_passed_laps+1} / {maxLaps}";

        }
    }
}
