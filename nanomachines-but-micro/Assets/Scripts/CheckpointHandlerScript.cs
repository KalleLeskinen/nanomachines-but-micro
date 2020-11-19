using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointHandlerScript : MonoBehaviour
{
    [SerializeField]
    GameObject[] checkpoints;
    GameObject finishLine;
    int numberOfCheckpoints;
    float raceClock;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckpointPassed(GameObject car, float car_clock, int cp_number)
    {
        Guid carId = car.GetComponent<LapTimeUpdate>().id;
    }

    public float GetRaceClock()
    {
        return raceClock;
    }
}
