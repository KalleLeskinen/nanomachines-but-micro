using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KartCameraController : MonoBehaviour
{
    private bool
        cameraActive = false;

    public float 
        positionSmoothing,
        rotationSmoothing;

    public Vector3 startPos;


    private GameObject player;
    private GameObject cam;
    private GameObject camPos;

    public GameObject escMenu;


    private void Start()
    {
        StartCoroutine(waitForSpawn(3f));

        startPos = transform.localPosition;
        
        // Saving the cameras starting position on the player


        
    }

    IEnumerator waitForSpawn(float time)
    {
        yield return new WaitForSeconds(time);
        player = transform.parent.GetChild(0).gameObject;
        Debug.Log("Player: " + player);
        camPos = new GameObject();
        camPos.name = "Camera Position";
        camPos.transform.parent = player.transform;

        camPos.transform.localPosition = new Vector3(0,2.5f,-10);

        camPos.transform.parent = transform.parent.GetChild(0).transform;
        cameraActive = true;
    
    
    
    }

    private void FixedUpdate()
    {
        UpdateCamera();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escMenu.SetActive(true);
        }
    }


    private void UpdateCamera()
    {
        if (!cameraActive)
        {
            Debug.Log("return pois");
            return;
        } else
        {


        // Position

        Vector3 targetPos = camPos.transform.position;

        this.transform.position = Vector3.Lerp(transform.position, targetPos, positionSmoothing * Time.deltaTime);




        //// Rotation

        Vector3 lookDir = player.transform.position - transform.position;

        Quaternion rotDir = Quaternion.LookRotation(lookDir);

        transform.rotation = Quaternion.Lerp(transform.rotation, rotDir, rotationSmoothing * Time.deltaTime);

        }
    }

    public void ExitGame()
    {
        Debug.Log("Exiting to main menu...");
        SceneManager.LoadScene(0);
    }

}
