using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowFromBehind : MonoBehaviour
{
    private GameObject cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = gameObject.GetComponentInChildren<CameraPosition>().cameraPos;
        cam.transform.position = pos;
        cam.transform.LookAt(gameObject.transform);
    }
}
