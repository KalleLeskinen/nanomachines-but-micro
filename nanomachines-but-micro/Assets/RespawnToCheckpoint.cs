using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnToCheckpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            int whichCP = other.gameObject.GetComponent<LapTimeUpdate>().car_passed_cps.Count;
            if (whichCP == 0)
            {
                other.transform.position = GameObject.FindGameObjectWithTag("server_pos").transform.position;
            }
            GameObject[] spawnCPs = GameObject.FindGameObjectsWithTag("checkpoint");
            foreach (var cp in spawnCPs)
            {
                Vector3 cp_pos = cp.transform.position;
                if (cp.GetComponent<CheckpointScript>().checkpointNumber == whichCP-1)
                {
                    other.transform.position = cp_pos + Vector3.up * 3;
                    other.transform.rotation = cp.transform.rotation;
                }
            }
        }
    }
}
