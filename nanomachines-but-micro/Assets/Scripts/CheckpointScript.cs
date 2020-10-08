using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    [SerializeField]
    int checkpointNumber;

    static RaceScript raceScript;
    // Start is called before the first frame update
    void Start()
    {
        raceScript = GameObject.FindGameObjectWithTag("RaceHandler").GetComponent<RaceScript>();
        gameObject.GetComponent<MeshRenderer>().material.color = Color.red; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.GetComponent<LapTimeUpdate>().car_passed_cps.Contains(checkpointNumber))
            other.gameObject.GetComponent<LapTimeUpdate>().car_passed_cps.Add(checkpointNumber);
        Guid id = other.gameObject.GetComponent<LapTimeUpdate>().id;
        Debug.Log("Guid was: " + id);
        float timePassedFromStart = other.GetComponent<LapTimeUpdate>().GetClock();
        raceScript.CheckPointPassed(id, timePassedFromStart, checkpointNumber);
        gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
    }

}
