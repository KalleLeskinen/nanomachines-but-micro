using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prevent_roll_over : MonoBehaviour
{

    // Update is called once per frame

    void FixedUpdate()
    {
        Vector3 velocityVector = gameObject.GetComponent<Rigidbody>().velocity;
        if (Time.frameCount % 30 == 0 && velocityVector.x < 1f && velocityVector.y < 1f && velocityVector.z < 1f)
        {
            var xRotLimit = 90f;
            var zRotLimit = 90f;
            if (transform.rotation.eulerAngles.x >= xRotLimit && transform.rotation.eulerAngles.x <= 270)
            {
                transform.rotation = Quaternion.identity;
                Debug.Log("flippedx");
            }
            if (transform.rotation.eulerAngles.z >= zRotLimit && transform.rotation.eulerAngles.z <= 270)
            {
                Debug.Log("flippedz");

                transform.rotation = Quaternion.identity;
            }
        }
    }
}
