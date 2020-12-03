using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCar : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(new Vector3(0, 10 * Time.deltaTime, 0));
    }
}
