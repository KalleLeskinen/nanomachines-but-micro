using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject
{
    public BoltEntity entity;
    public BoltConnection connection;

    public bool IsServer => connection == null;

    public bool IsClient => connection != null;

    public void Spawn()
    {
        if (!entity)
        {
            entity = BoltNetwork.Instantiate(BoltPrefabs.Truck_1, new Vector3(0, 0, 0), Quaternion.identity);

            if (IsServer)
            {
                entity.TakeControl();
            }
            else
            {
                entity.AssignControl(connection);
            }
        }

        entity.transform.position = new Vector3(0,0,0);
    }
}
