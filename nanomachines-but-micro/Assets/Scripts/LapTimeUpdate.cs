using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapTimeUpdate : MonoBehaviour
{
    public Guid id;
    public List<float> lapTimes;
    int lap;
    float clock;
    // Start is called before the first frame update
    void Start()
    {
        clock = 0;
        lap = 0;
        id = Guid.NewGuid(); // generate a guid for the car
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="FinishLine")
        {
            if (lap > 0)
            { 
                lapTimes.Add(clock);
            }
            clock = 0;
            lap++;
            Debug.Log("lap:"+ lap);
        }
    }
    private void Update()
    {
        clock += Time.deltaTime;
    }
}
