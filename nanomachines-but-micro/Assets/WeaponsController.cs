using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapon
{
    Minea, Rocketa
}
public class WeaponsController : Bolt.EntityBehaviour<IVehicleState>
{
    bool mineFlag = false;
    bool rocketFlag = false;

    public override void Attached()
    {
        state.OnShoot += Shooty;
    }
    private void Update()
    {
        ProcessMoreInputs();
    }
    private void FixedUpdate()
    {
        if (mineFlag)
        {
            state.Shoot();
            mineFlag = false;
        }
        if (rocketFlag)
        {
            Debug.Log("rocket");
            rocketFlag = false;
        }
    }

    public void ProcessMoreInputs()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            mineFlag = true;
            Debug.Log(state.AmmoCount);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            rocketFlag = true;
            Debug.Log(state.AmmoCount);
        }
    }
    private void Shooty()
    {
        mineFlag = false;
        Vector3 NewPosition = GetComponentInChildren<minespawnpos>().minePos;
        if (state.AmmoCount>=2)
        {
            state.AmmoCount -= 2;
            BoltNetwork.Instantiate(BoltPrefabs.Mine, NewPosition, Quaternion.identity);
            Debug.Log("mine");
        }
        //spawn the mine behind the car
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
