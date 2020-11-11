using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineScript : MonoBehaviour
{
    static RaceScript raceScript;
    // Start is called before the first frame update
    void Start()
    {
        raceScript = GameObject.FindGameObjectWithTag("RaceHandler").GetComponent<RaceScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
        int cp_count = other.gameObject.GetComponent<LapTimeUpdate>().car_passed_cps.Count;

        if (cp_count == 0)
            {
                other.gameObject.GetComponent<LapTimeUpdate>().clock = 0;
            }
            int number_of_cps = raceScript.numberOfcheckpoints;
            Guid id = other.gameObject.GetComponent<LapTimeUpdate>().id;
            Debug.Log("Guid was: " + id);
            float timePassedFromStart = other.GetComponent<LapTimeUpdate>().GetClock();
            if (cp_count > 0 && cp_count % number_of_cps == 0)
            {
                raceScript.FinishLinePassed(other.gameObject, id, timePassedFromStart);
                other.gameObject.GetComponent<LapTimeUpdate>().clock = 0;
                other.gameObject.GetComponent<LapTimeUpdate>().car_passed_cps.Clear();
            }
        }
    }

}
