using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile_controller : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(Vector3.forward * Time.deltaTime * 5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("boom");
    }
}
