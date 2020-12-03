using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapTimeUpdate : MonoBehaviour
{
    public Guid id;
    public List<float> lapTimes;
    public List<int> car_passed_cps;
    public float clock;
    public string time = "";
    public int car_passed_laps;
    GameObject raceHandler;
    BoltEntity player;
    // Start is called before the first frame update
    void Start()
    {
        car_passed_laps = 0;
        clock = 0;
        id = Guid.NewGuid(); // generate a guid for the car
    }

    private void Update()
    {
        if (clock > 0)
            time = clock.ToString("#.#");
        clock += Time.deltaTime; // tämä pitää vaihtaa laskemaan vain silloin kun on saatu merkki pelin alkamisesta
                                 // nyt se on spawnaamisesta
    }
    public float GetClock()
    {
        return clock;
    }
    public string GetClockFormatted()
    {
        return Math.Floor(clock).ToString();
    }
    public void CarIncrementLap()
    {
        car_passed_laps++;
    }
}
