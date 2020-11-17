using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToStartingPosition : Bolt.EntityBehaviour<IVehicleState>
{
	public int _startingPosition;

    public void PlaceCarInStartPos(Vector3 _pos)
    {
	    gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
	    gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(0,0,0);
	    gameObject.GetComponent<Rigidbody>().mass = 100000f;

        gameObject.transform.position = _pos;
	    gameObject.transform.LookAt(_pos + new Vector3 (0,0,-2));
    }
}
