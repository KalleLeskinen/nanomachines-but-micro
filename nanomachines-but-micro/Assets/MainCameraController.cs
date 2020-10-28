using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    public static MainCameraController instance;

    void Awake()
    {
        instance = this;
    }

    // Setup the camera with the default view
    public void Configure(Transform parent)
    {
        gameObject.transform.parent = parent;
        gameObject.transform.localPosition = new Vector3(0, 5f, -5f);
        gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}