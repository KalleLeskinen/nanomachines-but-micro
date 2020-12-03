using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class racestart_sound_script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/RaceStart");
    }
}
