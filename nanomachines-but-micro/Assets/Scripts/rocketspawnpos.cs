using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocketspawnpos : MonoBehaviour
{
    public Vector3 rocketPos;
    private void Update()
    {
        rocketPos = gameObject.transform.position;
    }
}
