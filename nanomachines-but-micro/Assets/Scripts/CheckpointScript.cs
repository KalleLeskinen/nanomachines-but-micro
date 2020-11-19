using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    [SerializeField]
    int checkpointNumber;
    public Material _material;
    static RaceScript raceScript;
    // Start is called before the first frame update
    void Start()
    {

        raceScript = GameObject.FindGameObjectWithTag("RaceHandler").GetComponent<RaceScript>();
        _material = gameObject.GetComponent<MeshRenderer>().material;
        _material.color = Color.red;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Guid id = other.gameObject.GetComponent<LapTimeUpdate>().id;
            Debug.Log("Guid was: " + id);
            float timePassedFromStart = other.GetComponent<LapTimeUpdate>().GetClock();
            if (!other.gameObject.GetComponent<LapTimeUpdate>().car_passed_cps.Contains(checkpointNumber))
            {
                raceScript.CheckPointPassed(gameObject, id, timePassedFromStart, checkpointNumber);
                //other.gameObject.GetComponent<LapTimeUpdate>().car_passed_cps.Add(checkpointNumber);
                Debug.Log("ADDED FROM CPS");
            }
        }
    }

}
