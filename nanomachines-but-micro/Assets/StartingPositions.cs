using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPositions : Bolt.EntityBehaviour<IStateOfRace>
{
    RaceScript raceScript;
    public Vector3[] startingPositions;

    private void Start()
    {
        Debug.Log("Start:");
        if (BoltNetwork.IsServer)
        {
            startingPositions = new Vector3[6];
            raceScript = GameObject.FindGameObjectWithTag("RaceHandler").GetComponent<RaceScript>();
            InitStartingPositions();
        }
    }

    private void InitStartingPositions()
    {
        Debug.Log("#232 InitStartingPositions");
        Vector3 pole_position = GameObject.FindGameObjectWithTag("pole_position").transform.position;
        Vector3 second_position = GameObject.FindGameObjectWithTag("second_position").transform.position;
        startingPositions[0] = pole_position;
        startingPositions[1] = second_position;
        int j = 1;
        for (int i = 2; i < 6; i++)
        {
            if (i % 2 == 0)
                startingPositions[i] = startingPositions[i - 2] + new Vector3(0, 0, 10.5f);
            else
                startingPositions[i] += startingPositions[i - 2] + new Vector3(0, 0, 10.5f);
        }
        //this is for level_1
    }


    //void Start()
    //{
    //    raceScript = GameObject.FindGameObjectWithTag("RaceHandler").GetComponent<RaceScript>();
    //    finish_line = GameObject.FindGameObjectWithTag("FinishLine");
    //}
    //private void Update()
    //{
    //    Debug.Log(initialized + "232");
    //    if (raceScript.state.RaceStarted && !initialized)
    //    {
    //        startingPositions = new Vector3[raceScript.playerDataList.Count];
    //        startingPositions[0] = GameObject.FindGameObjectWithTag("pole_position").transform.position;
    //        SetStartingPositions();
    //        initialized = true;
    //    }
    //    if (initialized && !set)
    //    {
    //        //SetCarsToStartingPositions();
    //        //waiting_for_race_start = true;
    //        Debug.Log("updating locations 232");
    //    }
    //    if (waiting_for_race_start)
    //    {
    //        foreach (var car in raceScript.cars)
    //        {

    //        }
    //    }
    //}

    //private void SetStartingPositions()
    //{
    //    if (raceScript.playerDataList.Count > 1)
    //    {
    //        startingPositions[1] = GameObject.FindGameObjectWithTag("second_position").transform.position;
    //        for (int i = 1; i < raceScript.playerDataList.Count; i++)
    //        {
    //            if (i % 2 == 0)
    //            {
    //                Vector3 newPosition = startingPositions[0] + new Vector3(0, 0, +8.5f);
    //                Debug.Log(newPosition);
    //                startingPositions[i] = newPosition;
    //            }
    //            else
    //            {
    //                Vector3 newPosition = startingPositions[1] + new Vector3(0, 0, +8.5f);
    //                Debug.Log(newPosition);
    //                startingPositions[i] = newPosition;
    //            }
    //        }
    //    }

    //}

    //public void SetCarsToStartingPositions()
    //{
    //    //int i = 0;
    //    //foreach (var car in raceScript.cars)
    //    //{
    //    //    car.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    //    //    car.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
    //    //    car.transform.position = startingPositions[i];
    //    //    car.transform.LookAt(startingPositions[i] + new Vector3(0,0,-1));
    //    //    car.GetComponentInChildren<Rigidbody>().mass = 10000;
    //    //    //car.GetComponentInParent<BoltEntity>().Freeze(true);
    //    //    i++;
    //    //}
    //    for (int i = 0; i<raceScript.playerDataList.Count;i++)
    //    {
    //        raceScript.cars[i].transform.position = startingPositions[i];
    //        raceScript.cars[i].GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    //        raceScript.cars[i].GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
    //        raceScript.cars[i].transform.LookAt(startingPositions[i] + new Vector3(0, 0, -1));
    //        raceScript.cars[i].GetComponentInChildren<Rigidbody>().mass = 10000;
    //    }
    //    waiting_for_race_start = true;
    //    set = true;
    //}
}

