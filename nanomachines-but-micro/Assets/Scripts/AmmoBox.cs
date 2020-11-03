using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public int ammoAmount = 15;
    // Start is called before the first frame update
    void Start()
    {
        //määrä
    }
    private void FixedUpdate()
    {
        gameObject.transform.Rotate(new Vector3(0, 0, 10 * Time.deltaTime));
    }

}
