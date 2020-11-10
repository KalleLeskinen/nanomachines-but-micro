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
        state.OnDropMine += DropMine;
        state.OnShootRocket += ShootRocket;
    }
    private void Update()
    {
        ProcessMoreInputs();
    }
    private void FixedUpdate()
    {
        if (mineFlag)
        {
            state.DropMine();
            mineFlag = false;
        }
        if (rocketFlag)
        {
            state.ShootRocket();
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
    private void DropMine()
    {
        Quaternion rotation = GetComponent<Transform>().rotation;
        Vector3 RotVec3 = Quaternion.ToEulerAngles(rotation);
        RotVec3.x = -90f;
        Quaternion newRot = Quaternion.Euler(RotVec3);

        Vector3 mineSpawnPos = GetComponentInChildren<minespawnpos>().minePos;

        if (state.AmmoCount>=2)
        {
            state.AmmoCount -= 2;
            BoltNetwork.Instantiate(BoltPrefabs.Mine, mineSpawnPos, newRot);
        }
        //spawn the mine behind the car
    }
    private void ShootRocket()
    {
        Quaternion rotation = GetComponent<Transform>().rotation;
        Vector3 rocketSpawnPos = GetComponentInChildren<rocketspawnpos>().rocketPos;
        if (state.AmmoCount >= 1)
        {
            state.AmmoCount -= 1; //raketti
            BoltNetwork.Instantiate(BoltPrefabs.missile, rocketSpawnPos, rotation);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AmmoBlock"))
        {
            state.AmmoCount += other.gameObject.GetComponent<AmmoBox>().ammoAmount;
            Debug.Log("Ammo picked up. Current ammo:" + state.AmmoCount);
        }
    }

}
