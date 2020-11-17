using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapTimeUpdate : MonoBehaviour
{

    public Guid id;
    public string guid_string;
    public List<float> lapTimes;
    public List<int> car_passed_cps;
    public float clock;
    public string time = "";
    GameObject raceHandler;
    BoltEntity player;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponentInParent<BoltEntity>();
        if (player.IsOwner)
        {
            clock = 0;
            id = Guid.NewGuid(); // generate a guid for the car
            raceHandler = GameObject.FindGameObjectWithTag("RaceHandler");
            //car_passed_cps = new List<int>();
            guid_string = id.ToString();
        }
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
    public void OnGUI()
    {
        if (raceHandler && player)
        {
            GUI.Box(new Rect(50, 30, 100, 30), $"laptime: {time}");
            GUI.Box(new Rect(50, 60, 100, 30), $"passed: {car_passed_cps.Count.ToString()}/{raceHandler.GetComponent<RaceScript>().numberOfcheckpoints}");
        }
    }
}
