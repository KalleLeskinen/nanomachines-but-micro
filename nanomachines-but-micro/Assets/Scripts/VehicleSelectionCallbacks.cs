using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bolt;
using UnityEngine;

[BoltGlobalBehaviour("GarageScene")]
public class VehicleSelectionCallbacks : GlobalEventListener
{
    private PrefabId[] entityPrefabIds =
        { BoltPrefabs.Car1_Torino, BoltPrefabs.Car2_Torino, BoltPrefabs.TruckV1 };

    //private Dictionary<int, GameObject> modelPrefabs;
    private GameObject[] modelPrefabs = new GameObject[3];

    private GameObject rotatingDisplay;

    private void Awake()
    {
        //modelPrefabs.Add(0, Resources.Load<GameObject>("Car1_Torino_Model"));
        //modelPrefabs.Add(1, Resources.Load<GameObject>("Car2_Torino_Model"));
        //modelPrefabs.Add(2, Resources.Load<GameObject>("TruckV1Model"));
        modelPrefabs[0] = Resources.Load("Car1_Torino_Model") as GameObject;
        modelPrefabs[1] = Resources.Load("Car2_Torino_Model") as GameObject;
        modelPrefabs[2] = Resources.Load("TruckV1Model") as GameObject;
    }

    public override void SceneLoadLocalDone(string scene)
    {
        rotatingDisplay = GameObject.FindGameObjectWithTag("VehicleTray");

        var model = modelPrefabs[0];
        DisplayModel(model);
    }
    
    // Väliaikaisratkaisu.
    private GameObject DisplayModel(GameObject model)
    {
        return Instantiate(model, rotatingDisplay.transform.position, Quaternion.identity);
    }
}
