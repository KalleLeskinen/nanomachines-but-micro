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
       
    }
}
