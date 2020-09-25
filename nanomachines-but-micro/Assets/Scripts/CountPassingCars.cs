using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountPassingCars : MonoBehaviour
{
    //decide amount of rounds
    //5 sec free warmup
    //find all cars from the track
    //assign to starting position(s)
    //if collider finds car, find it from the list and check if driven rounds are enough

    public static int amountOfLaps = 2;
    public Dictionary<Guid, List<float>> playerScoreList = new Dictionary<Guid, List<float>>();
    public static List<GameObject> players = new List<GameObject>(); //tämän listan avulla selvitetään pelaajien lukumäärä
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IntialiseTheGameIn(5));
        
    }

    /// <summary>
    /// initializes the game in x seconds.
    /// 1. gets cars in scene, place to starting positions
    /// 2. create a KeyValuePair Dictionary for players and their laptimes
    /// </summary>
    /// <param name="warmupTime"></param>
    /// <returns></returns>
    IEnumerator IntialiseTheGameIn(int warmupTime)
    {
        Debug.Log($"Starting the game in {warmupTime} seconds");
        yield return new WaitForSeconds(warmupTime);

        GetCars();
        SetLapTimeList();
    }

    /// <summary>
    /// Set up the lap time list before its use.
    /// </summary>
    public void SetLapTimeList()
    {    
        foreach (var player in players)
        {
            LapTimeUpdate lpu_script = player.GetComponent<LapTimeUpdate>();
            playerScoreList.Add(lpu_script.id, lpu_script.lapTimes);
            Debug.Log($"ADDED: {lpu_script.id} + {lpu_script.lapTimes.Count} laps");
        }
    }
    /// <summary>
    /// this is actually a win condition checker rather than updater, as updating is done automagically as players cross the finish line
    /// </summary>
    /// <param name="go"></param>
    private void UpdateLapTime(GameObject go)
    {
        foreach (var player in players)
        {
            LapTimeUpdate lpu_script = player.GetComponent<LapTimeUpdate>();
            if (lpu_script.id == go.GetComponent<LapTimeUpdate>().id)
            {
                if (lpu_script.lapTimes.Count==amountOfLaps)
                {
                    PrintWinner(lpu_script.id);
                }
            }
        }
    }
    /// <summary>
    /// Sets car(s) in a starting position and adds them to the list of players
    /// </summary>
    private void GetCars()
    {
        foreach (GameObject car in GameObject.FindGameObjectsWithTag("Player"))
        {
            car.transform.position = new Vector3(9.5f, 0.5f, 3);
            car.transform.LookAt(new Vector3(9.5f, 0, 0));
            players.Add(car);
        }
    }
    /// <summary>
    /// prints GUID of the winner car and prints laptimes for each lap
    /// </summary>
    /// <param name="id"></param>
    private void PrintWinner(Guid id)
    {
        foreach (KeyValuePair<Guid, List<float>> kvp in playerScoreList)
        {
            Debug.Log($"Winner is {id}");
            if (kvp.Key == id)
            {
                int lapIndex = 0;
                foreach (float run in kvp.Value)
                {
                    Debug.Log($"Lap <{lapIndex + 1}> : {kvp.Value[lapIndex]}s");
                    lapIndex++;
                }
            }
        }
    }


    /// <summary>
    /// this just checks if an object with a tag "Player" crosses the finish line, and then proceeds to check if the car won the race
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            UpdateLapTime(col.gameObject);
        }
    }

}
