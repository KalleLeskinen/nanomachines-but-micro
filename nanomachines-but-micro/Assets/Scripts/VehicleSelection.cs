﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSelection : MonoBehaviour
{
    private GameObject[] modelPrefabs;

    [SerializeField]
    private GameObject rotatingDisplay;

    private GameObject displayedModel;
    
    private void Awake()
    {
        displayedModel = new GameObject();
        modelPrefabs[0] = Resources.Load("Car1_Torino_Model") as GameObject;
        modelPrefabs[1] = Resources.Load("Car2_Torino_Model") as GameObject;
        modelPrefabs[2] = Resources.Load("Truck-1_Model") as GameObject;
        modelPrefabs[3] = Resources.Load("TruckV1Model") as GameObject;
    }
    
    
}
