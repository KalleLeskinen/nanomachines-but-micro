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
    static RaceScript raceScript;
    // Start is called before the first frame update
    void Start()
    {
        clock = 0;
        id = Guid.NewGuid(); // generate a guid for the car
        raceScript = GameObject.FindGameObjectWithTag("RaceHandler").GetComponent<RaceScript>();
        car_passed_cps = new List<int>();
    }

    private void Update()
    {
        clock += Time.deltaTime; // tämä pitää vaihtaa laskemaan vain silloin kun on saatu merkki pelin alkamisesta
                                 // nyt se on spawnaamisesta
    }
    public float GetClock()
    {
        return clock;
    }
    public void OnGUI()
    {
        if (clock > 0)
            time = clock.ToString("#.#");
        GUI.Box(new Rect(50, 30, 100, 30), $"laptime: {time}");
        GUI.Box(new Rect(50, 60, 100, 30), $"passed: {car_passed_cps.Count.ToString()}/{raceScript.numberOfcheckpoints}");
    }
}
