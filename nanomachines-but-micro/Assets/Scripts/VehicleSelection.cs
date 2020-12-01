﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class VehicleSelection : MonoBehaviour
{
    [SerializeField] private GameObject rotatingDisplay;

    [SerializeField] private GameObject dataContainer;

    private GameObject[] modelPrefabs;
    
    private GameObject displayedModel;

    public int i;
    public int modelCount;

    public static VehicleSelection Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
        
        i = 40;
        modelCount = 7;
        displayedModel = new GameObject();
        modelPrefabs = new GameObject[modelCount];
        modelPrefabs[0] = Resources.Load("Car1_Torino_Model") as GameObject;
        modelPrefabs[1] = Resources.Load("Car2_Torino_Model") as GameObject;
        modelPrefabs[2] = Resources.Load("Car3_Torino_Model") as GameObject;
        modelPrefabs[3] = Resources.Load("Truck-1_Model") as GameObject;
        modelPrefabs[4] = Resources.Load("Truck-2_Model") as GameObject;
        modelPrefabs[5] = Resources.Load("TruckV1Model") as GameObject;
        modelPrefabs[6] = Resources.Load("TruckV2Model") as GameObject;


        displayedModel = modelPrefabs[0];
        DisplayModel(displayedModel);
    }
    
    private void Update()
    { 
        ProcessInput();
    }

    private void ChangeRemainder()
    {
        DisplayModel(modelPrefabs[i % modelPrefabs.Length]);
    }

    private void DisplayModel(GameObject model)
    {
        Destroy(displayedModel);
        //displayedModel = new GameObject();
        displayedModel = Instantiate(model, rotatingDisplay.transform.position, rotatingDisplay.transform.rotation);
        displayedModel.transform.parent = rotatingDisplay.transform;
    }
    
    private void ProcessInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            i--;
            ChangeRemainder();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            i++;
            ChangeRemainder();
        }
    }

}
