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

        //spawn the mine behind the car
        Vector3 NewPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - 3f);

        state.AmmoCount -= 2; //miina

        BoltNetwork.Instantiate(BoltPrefabs.Mine, NewPosition, Quaternion.identity);
        Debug.Log("mine");
        return 0;
    }
    private int Shooty(float count)
    {
        state.AmmoCount -= 1; //raketti
        Debug.Log("rocket");
        return 1;
    }

}
