using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : BoltSingletonPrefab<PlayerCamera>
{
    public Transform _target { get; set; }

    public new Camera camera
    {
        get { return camera; }
    }


}