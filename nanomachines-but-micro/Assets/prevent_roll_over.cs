using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prevent_roll_over : MonoBehaviour
{

    // Update is called once per frame

    void FixedUpdate()
    {
        if (Time.frameCount % 30 == 0)
        {
            var xRotLimit = 90f;
            var zRotLimit = 90f;
            if (transform.rotation.eulerAngles.x >= xRotLimit && transform.rotation.eulerAngles.x <= 270)
            {
                transform.rotation = Quaternion.identity;
            }
            if (transform.rotation.eulerAngles.z >= zRotLimit && transform.rotation.eulerAngles.z <= 270)
            {
                transform.rotation = Quaternion.identity;
            }
        }
    }
}
