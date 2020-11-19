using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEventSender : Bolt.EntityBehaviour<IVehicleState>
{
    bool eventFlag = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (eventFlag)
        {
            var SpawnEvent = RespawnCar.Create();
            SpawnEvent.playerEntity = GetComponentInParent<BoltEntity>();
            SpawnEvent.SpawnPosition = state.SpawnPointID;
            SpawnEvent.Send();
            eventFlag = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && Input.GetKeyDown(KeyCode.T))
        {
            eventFlag = true;
        }
    }
}
