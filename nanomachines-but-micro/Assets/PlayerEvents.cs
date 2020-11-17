using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : Bolt.EntityBehaviour<IVehicleState>
{
    bool readyFlag = false;
    bool sent = false;
    public override void Attached()
    {
        if (gameObject.GetComponentInParent<BoltEntity>().IsOwner)
        {
            var joinedEvt = JoinedRoom.Create();
            joinedEvt.playerEntity = gameObject.GetComponentInParent<BoltEntity>();
            joinedEvt.Send();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !sent)
        {
            readyFlag = true;
        }
    }
    private void FixedUpdate()
    {
        if (readyFlag)
        {
            if (BoltNetwork.IsServer && gameObject.GetComponentInParent<BoltEntity>().IsOwner)
            {
                var hostReadyEvt = HostReadyEvent.Create();
                hostReadyEvt.playerGuid = gameObject.GetComponentInChildren<LapTimeUpdate>().id.ToString();
                hostReadyEvt.Send();
                readyFlag = false;
                sent = true;
            }
            if (BoltNetwork.IsClient && gameObject.GetComponentInParent<BoltEntity>().IsOwner)
            { 
                var readyEvt = PlayerReadyEvent.Create();
                readyEvt.playerGuid = gameObject.GetComponentInChildren<LapTimeUpdate>().id.ToString();
                readyEvt.Send();
                readyFlag = false;
                sent = true;
            }
        }
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(300,300,200,100), "press R if you want to skip waiting");
    }
}
