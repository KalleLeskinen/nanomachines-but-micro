using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestCam : MonoBehaviour
{

    //Lerp speed
    public float speed;

    //zoom multiplier
    public float zoom;



    private Camera cam;

    private float 
        startFOV,
        startDis;



    void Start()
    {
        cam = Camera.main;
        startFOV = cam.fieldOfView;
        
    }

    void FixedUpdate()
    {
        

        if(GameObject.FindGameObjectWithTag("Player"))
        {


            //FOLLOWING 

            //1
            //this.gameObject.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
            
            //2
            //Getting vector to player
            Vector3 lookDir =  GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;

            Quaternion rotDir = Quaternion.LookRotation(lookDir);

            //Lerping the rotation for smoooth~ action
            transform.rotation = Quaternion.Lerp(transform.rotation, rotDir, speed * Time.deltaTime);


            //ZOOMING

            float dis = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position);

            cam.fieldOfView = startFOV + ((startFOV * (dis - startDis)) * zoom);

        }

        

    }
}
