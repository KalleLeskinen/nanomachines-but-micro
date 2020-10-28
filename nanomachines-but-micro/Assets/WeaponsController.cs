using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapon
{
    Mine, Rocket
}
public class WeaponsController : Bolt.EntityBehaviour<IVehicleState>
{
    public override void SimulateOwner()
    {
        ProcessMoreInputs();
    }

    private void ProcessMoreInputs()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Shooty(state.AmmoCount);
            Debug.Log(state.AmmoCount);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Shooty(1.0f);
            Debug.Log(state.AmmoCount);
        }
    }
    private int Shooty(int count)
    {
        Vector3 NewPosition = GetComponentInChildren<minespawnpos>().minePos;
        if (state.AmmoCount>=2)
        {
            state.AmmoCount -= 2;
            BoltNetwork.Instantiate(BoltPrefabs.Mine, NewPosition, Quaternion.identity);
            Debug.Log("mine");
        }
        //spawn the mine behind the car
        return 0;
    }
    private int Shooty(float count)
    {
        if (state.AmmoCount >= 1)
        {
            state.AmmoCount -= 1; //raketti
            Debug.Log("rocket");
        }
        return 1;
    }

}
