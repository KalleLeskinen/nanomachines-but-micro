using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spin_the_lift : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.forward * 25f * Time.deltaTime);
    }
}
