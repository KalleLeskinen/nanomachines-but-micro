using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEventSender : Bolt.EntityBehaviour<IVehicleState>
{
    bool eventFlag = false;
    bool startTheGameFlag = false;
    // Start is called before the first frame update

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
        if (startTheGameFlag)
        {
            var startGame = StartTheGame.Create();
            startGame.Send();
            startTheGameFlag = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && Input.GetKeyDown(KeyCode.T))
        {
            eventFlag = true;
        }
        if (Input.GetKeyDown(KeyCode.F) && BoltNetwork.IsServer)
        {
            startTheGameFlag = true;
        }
    }
}
