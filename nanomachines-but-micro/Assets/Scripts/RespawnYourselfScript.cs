using Bolt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnYourselfScript : GlobalEventListener

{
    NetworkCallbacks ncb_script;
    BoltEntity car;
    // Start is called before the first frame update
    void Start()
    {
        ncb_script = GameObject.Find("BoltBehaviours").GetComponent<NetworkCallbacks>();
        car = this.gameObject.GetComponent<BoltEntity>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.T))
        {
            Vector3 spawnpos= ncb_script.spawnPos;
            spawnpos.y += 10f;
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
            
            gameObject.transform.position = spawnpos;
        }
    }
}
