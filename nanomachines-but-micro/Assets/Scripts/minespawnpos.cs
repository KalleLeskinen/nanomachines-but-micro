using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minespawnpos : MonoBehaviour
{
    public Vector3 minePos;

    private void Update()
    {
        minePos = gameObject.transform.position;
    }
}
