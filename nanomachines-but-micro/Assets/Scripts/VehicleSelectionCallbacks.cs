using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bolt;
using Bolt.Matchmaking;
using UnityEngine;
using UnityEngine.SceneManagement;

[BoltGlobalBehaviour]
public class VehicleSelectionCallbacks : GlobalEventListener
{
    /*private static GameObject[] modelPrefabs = new GameObject[4];

    private GameObject rotatingDisplay;

    private GameObject displayedModel = new GameObject();

    public static int _i = 0;

    private void Awake()
    {
        //modelPrefabs.Add(0, Resources.Load<GameObject>("Car1_Torino_Model"));
        //modelPrefabs.Add(1, Resources.Load<GameObject>("Car2_Torino_Model"));
        //modelPrefabs.Add(2, Resources.Load<GameObject>("TruckV1Model"));
        modelPrefabs[0] = Resources.Load("Car1_Torino_Model") as GameObject;
        modelPrefabs[1] = Resources.Load("Car2_Torino_Model") as GameObject;
        modelPrefabs[2] = Resources.Load("Truck-1_Model") as GameObject;
        modelPrefabs[3] = Resources.Load("TruckV1Model") as GameObject;
    }

    public override void SceneLoadLocalDone(string scene)
    {
        if (scene != "GarageScene") return;
        rotatingDisplay = GameObject.FindGameObjectWithTag("VehicleTray");

        displayedModel = modelPrefabs[_i];
        DisplayModel(displayedModel);
    }
    
    // Väliaikaisratkaisu.
    private GameObject DisplayModel(GameObject model)
    {
        Destroy(displayedModel);
        return displayedModel = Instantiate(model, rotatingDisplay.transform.position, Quaternion.identity);
    }

    // tähän UI napit keyboard inputin sijaan, jos hyvin käy voidaan pitää ratkaisu samana jos nämä ei haittaa 
    // inputtia gameplayn aikana.
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "GarageScene")
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if(_i <= 2)
                {
                    Debug.Log("gotem");
                    DisplayModel(modelPrefabs[_i++]);
                }
                else
                {
                    _i = 0;
                    DisplayModel(modelPrefabs[_i]);
                }
            }

            if (Input.GetKeyDown(KeyCode.Return)) //huom muutettu enteriksi.
            {
                BoltNetwork.LoadScene("Level_1"); // tähän BoltLauncher.StartServer(); if (BoltNetwork.Isserver) elseif (isclient) {BoltMatchmaking.JoinSession(photonSession); } luettelo servereistä tähän sceneen? ? ?
            }
            // Input napeista tai näppisinputista jotka togglee autojen välillä, voisi hajauttaa omaan metodiinsa.
        }
    }*/
}
