using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class connected_player_ui_script : MonoBehaviour
{
    int i;
    string lastfield = "";
    private void Update()
    {
        if (Time.frameCount % 100 == 0)
        {
            int playersInScene = GameObject.FindGameObjectWithTag("RaceHandler").GetComponent<BoltEntity>().GetState<IStateOfRace>().NumberOfPlayers;
            foreach (var car in BoltNetwork.Entities)
            {
                if (car.TryFindState<IVehicleState>(out IVehicleState state) && !car.IsOwner && i < playersInScene-1)
                {
                    SetEarlierNames(state.PlayerName);
                    lastfield = state.PlayerName;
                    ++i;
                }
            }
        }
    }
    void SetEarlierNames(string name)
    {
        
        foreach (var textField in GetComponentsInChildren<Text>())
        {
            if (lastfield != name && textField.text == "")
            {
                textField.text = name;
                lastfield = name;
                break;
            }
        }
    }
}
