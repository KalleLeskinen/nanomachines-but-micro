using System;
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

    public static VehicleSelection Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
        
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
        ChangeRemainder();
    }

    private void ChangeRemainder()
    {
        DisplayModel(modelPrefabs[i % modelPrefabs.Length]);
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
            i--;

        if (Input.GetKeyDown(KeyCode.D))
            i++;
    }

}
