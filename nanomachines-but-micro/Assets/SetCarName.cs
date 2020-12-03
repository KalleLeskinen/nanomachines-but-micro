using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class SetCarName : MonoBehaviour
{

    private void Start()
    {
        GameObject.FindGameObjectWithTag("score_panel").SetActive(false);
        if (BoltNetwork.IsServer)
        {
            GameObject.FindGameObjectWithTag("client_start_ui").SetActive(false);
        }
        else
        {
            GameObject.FindGameObjectWithTag("host_name_laps_ui").SetActive(false);
        }

    }
    public void SetCarNameForMyEntity()
    {
        if (BoltNetwork.IsClient)
        {
            foreach (var obj in BoltNetwork.Entities)
            {
                if (obj.IsOwner&&obj.StateIs<IVehicleState>())
                {
                    string nametoset = GameObject.FindGameObjectWithTag("player_name").GetComponent<Text>().text;
                    if (nametoset == "")
                    {
                        nametoset = GenerateRandom();
                    }
                    Debug.Log("My object was " + obj.name);
                    Debug.Log("field text was " + nametoset);
                    obj.GetState<IVehicleState>().PlayerName = nametoset;
                    var ClientReadyEvent = PlayerReadyEvent.Create();
                    ClientReadyEvent.name = nametoset;
                    ClientReadyEvent.Send();
                }
            }

            GameObject.FindGameObjectWithTag("client_start_ui").SetActive(false);
        }
        else
        {
            GameObject.FindGameObjectWithTag("RaceHandler").GetComponent<RaceScript>().UpdatePlayerBase();
        }
    }

    private string GenerateRandom()
    {
        return $"{ReadRandomName("Assets/Resources/TextFiles/hundred_adjectives.txt")} " +
               $"{ReadRandomName("Assets/Resources/TextFiles/hundred_nouns.txt")}";
    }

    private string ReadRandomName(string path)
    {
        var reader = File.ReadAllLines(path);
        var random = new Random();
        var randomLineNumber = random.Next(0, reader.Length - 1);
        return reader[randomLineNumber];
    }

    public void HostSetCarNameAndNumberOfLaps()
    {
        if (BoltNetwork.IsServer)
        {
            Debug.Log("HOST!");
            foreach (var obj in BoltNetwork.Entities)
            {
                if (obj.StateIs<IStateOfRace>() && BoltNetwork.IsServer)
                {
                    int nbOfLaps = Mathf.FloorToInt(GameObject.FindGameObjectWithTag("host_name_laps_ui").GetComponentInChildren<Slider>().value);
                    obj.GetState<IStateOfRace>().NumberOfLaps = nbOfLaps;
                }
                if (obj.IsOwner && obj.StateIs<IVehicleState>())
                {
                    string nametoset = GameObject.FindGameObjectWithTag("player_name").GetComponent<Text>().text;
                    if (nametoset == "")
                    {
                        nametoset = GenerateRandom();
                    }
                    obj.GetState<IVehicleState>().PlayerName = nametoset;
                    var hostready = HostReadyEvent.Create();
                    hostready.name = nametoset;
                    hostready.Send();
                }
            }
            GameObject.FindGameObjectWithTag("host_name_laps_ui").SetActive(false);
        }
    }
}
