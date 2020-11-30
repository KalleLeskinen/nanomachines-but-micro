using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class VehicleSelection : MonoBehaviour
{
    [SerializeField]
    private GameObject rotatingDisplay;

    private GameObject[] modelPrefabs;
    
    private GameObject displayedModel;

    private int i;

    private void Awake()
    {
        i = 0;
        displayedModel = new GameObject();
        modelPrefabs = new GameObject[4];
        modelPrefabs[0] = Resources.Load("Car1_Torino_Model") as GameObject;
        modelPrefabs[1] = Resources.Load("Car2_Torino_Model") as GameObject;
        modelPrefabs[2] = Resources.Load("Truck-1_Model") as GameObject;
        modelPrefabs[3] = Resources.Load("TruckV1Model") as GameObject;

        displayedModel = modelPrefabs[0];
        DisplayModel(displayedModel);
    }
    
    private void Update()
    { 
        ProcessInput();
    }
    
    private GameObject DisplayModel(GameObject model)
    {
        Destroy(displayedModel);
        displayedModel = new GameObject();
        return displayedModel = Instantiate(model, rotatingDisplay.transform.position, Quaternion.identity);
    }
    
    private void ProcessInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (i == 0)
            {
                i = 4;
                DisplayModel(modelPrefabs[i]);
            } 
            else
            {
                DisplayModel(modelPrefabs[i--]);
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (i == 4)
            {
                i = 0;
                DisplayModel(modelPrefabs[i]);
            }
            else
            {
                DisplayModel(modelPrefabs[i++]);
            }
        }
    }
    
}
