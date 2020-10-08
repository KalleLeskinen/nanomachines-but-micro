﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapTimeUpdate : MonoBehaviour
{
    public Guid id;
    public List<float> lapTimes;
    public List<int> car_passed_cps;
    int lap;
    public float clock;
    // Start is called before the first frame update
    void Start()
    {
        clock = 0;
        lap = 0;
        id = Guid.NewGuid(); // generate a guid for the car
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
}
