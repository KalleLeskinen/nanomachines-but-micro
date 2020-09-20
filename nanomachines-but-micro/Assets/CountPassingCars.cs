using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountPassingCars : MonoBehaviour
{
    //decide amount of rounds
    //5 sec free warmup
    //create collection key value "lap":"time"
    //find all cars from the track
    //assign to starting position(s)
    //start a lap timer from 0 for each player

    //if collider finds car, find it from the list and set a lap time
    public static int amountOfLaps = 2;
    public Dictionary<Guid, List<float>> playerScoreList = new Dictionary<Guid, List<float>>();
    public static List<GameObject> players = new List<GameObject>(); //tämän listan avulla selvitetään pelaajien lukumäärä
    private List<LapTimeCounter> playerLapTimeList = new List<LapTimeCounter>();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IntialiseTheGameIn(5));
        
    }


    private class LapTimeCounter
    {
        public Guid _id { get; set; }
        public float[] _lapTimes { get; set; }

        public void PrintLapTimes(GameObject go)
        {
            foreach (float laptime in _lapTimes)
            {
                Debug.Log(laptime);
            }
        }
    }

    IEnumerator IntialiseTheGameIn(int warmupTime)
    {
        Debug.Log($"Starting the game in {warmupTime} seconds");
        yield return new WaitForSeconds(warmupTime);

        GetCars();
        SetLapTimeList();
    }

    public void SetLapTimeList()
    {    
        foreach (var player in players)
        {
            LapTimeUpdate lpu_script = player.GetComponent<LapTimeUpdate>();
            playerScoreList.Add(lpu_script.id, lpu_script.lapTimes);
            Debug.Log($"ADDED: {lpu_script.id} + {lpu_script.lapTimes.Count} laps");
        }
    }
    private void UpdateLapTime(GameObject go)
    {
        foreach (var player in players)
        {
            LapTimeUpdate lpu_script = player.GetComponent<LapTimeUpdate>();
            if (lpu_script.id == go.GetComponent<LapTimeUpdate>().id)
            {
                if (lpu_script.lapTimes.Count==amountOfLaps)
                {
                    foreach (KeyValuePair<Guid, List<float>> kvp in playerScoreList)
                    {
                        Debug.Log($"Winner is {lpu_script.id}");
                        if (kvp.Key == lpu_script.id)
                        {
                            int lapIndex = 0;
                            foreach (float run in kvp.Value)
                            {
                                Debug.Log($"Lap <{lapIndex+1}> : {kvp.Value[lapIndex]}s");
                                lapIndex++;
                            }
                        }
                    }
                }
            }
        }
    }
    private void GetCars()
    {
        foreach (GameObject car in GameObject.FindGameObjectsWithTag("Player"))
        {
            car.transform.position = new Vector3(9.5f, 0.5f, 3);
            car.transform.LookAt(new Vector3(9.5f, 0, 0));
            players.Add(car);
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            UpdateLapTime(col.gameObject);
        }
    }

}
