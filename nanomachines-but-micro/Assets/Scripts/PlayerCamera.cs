using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : BoltSingletonPrefab<PlayerCamera>
{
    private float 
        startFOV,
        startDis;
    
    public float smoothing;
    
    public Transform _target { get; set; }

    public new Camera camera
    {
        get { return camera; }
    }
    
    public void SetTarget(BoltEntity entity)
    {
        _target = entity.transform;
    }

    private void FixedUpdate()
    {
        //2
        //Getting vector to player
        Vector3 lookDir =  GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;

        Quaternion rotDir = Quaternion.LookRotation(lookDir);

        //Lerping the rotation for smoooth~ action
        transform.rotation = Quaternion.Lerp(transform.rotation, rotDir, smoothing * Time.deltaTime);
    }
}