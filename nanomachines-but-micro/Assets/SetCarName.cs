using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            Debug.Log("#323 start4");
            foreach (var obj in BoltNetwork.Entities)
            {
                if (obj.IsOwner&&obj.StateIs<IVehicleState>())
                {
                    string nametoset = GameObject.FindGameObjectWithTag("player_name").GetComponent<Text>().text;
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
