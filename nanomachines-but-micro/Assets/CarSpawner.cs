using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class CarSpawner : Bolt.EntityBehaviour<IVehicleState>
{
    public RaceScript rs;
    public override void Attached()
    {
        state.OnSpawnCar = SpawnTheCar;
    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().ToString().Equals("Level_1"))
            rs = GameObject.FindGameObjectWithTag("RaceHandler").GetComponent<RaceScript>();
    }

    private void SpawnTheCar()
    {
        //target.position = mm_script.startingPositions[i];
        int startPos = state.startingPosition;
        Transform target = CreateTransform(rs.startingPositions[startPos]);
        Bolt.NetworkTransform player_transform = state.VehicleTransform;
        state.SetTransforms(player_transform, target);
        transform.position = target.position;
        state.SetTransforms(state.VehicleTransform, transform);
    }
    public Transform CreateTransform(Vector3 position)
    {
        GameObject helper = new GameObject();
        helper.transform.position = position;
        return helper.transform;
    }
}
