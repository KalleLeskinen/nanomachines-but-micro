using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCenterOffMass : MonoBehaviour
{

    // Update is called once per frame
    void FixedUpdate()
    {

        var joint = GetComponent<FixedJoint>();
        var rig = joint.connectedBody;
        gameObject.transform.position = rig.gameObject.transform.position + new Vector3(0,-1.5f,0);
        
    }
}
