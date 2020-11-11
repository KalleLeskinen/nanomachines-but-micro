using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocketdir : MonoBehaviour
{
    public Vector3 rocketDir;
    private void Update()
    {
        rocketDir = gameObject.transform.position;
    }
}
