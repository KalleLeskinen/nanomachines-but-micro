using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartCameraController : MonoBehaviour
{

    public float 
        positionSmoothing,
        rotationSmoothing;

    public Vector3 startPos;


    private GameObject player;
    private GameObject camPos;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        startPos = transform.position;
        
        
        // Saving the cameras starting position on the player
        camPos = new GameObject();
        camPos.name = "Camera Position";
        camPos.transform.parent = player.transform;
        camPos.transform.position = startPos;

        
    }



    private void FixedUpdate()
    {
        UpdateCamera();
    }


    private void UpdateCamera()
    {

        // Position

        Vector3 targetPos = camPos.transform.position;

        this.transform.position = Vector3.Lerp(transform.position, targetPos, positionSmoothing * Time.deltaTime);




        // Rotation

        Vector3 lookDir = player.transform.position - transform.position;


        Quaternion rotDir = Quaternion.LookRotation(lookDir);

        transform.localRotation = Quaternion.Lerp(transform.rotation, rotDir, rotationSmoothing * Time.deltaTime);

    
    }


}
