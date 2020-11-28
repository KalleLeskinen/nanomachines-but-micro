using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_countdown_ui_script : MonoBehaviour
{
    public GameObject textfield;
    public GameObject raceHandler;
    // Start is called before the first frame update
    void Start()
    {
        raceHandler = GameObject.FindGameObjectWithTag("RaceHandler");
        textfield = GameObject.FindGameObjectWithTag("start_time_counter_ui");
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 10 == 0 && Time.timeSinceLevelLoad > 1)
            textfield.GetComponent<Text>().text = Mathf.FloorToInt(raceHandler.GetComponent<BoltEntity>().GetState<IStateOfRace>().Clock).ToString();
        if (Time.timeSinceLevelLoad > 1 && Mathf.FloorToInt(raceHandler.GetComponent<BoltEntity>().GetState<IStateOfRace>().Clock) < 1)
        {
            textfield.transform.parent.gameObject.SetActive(false);
        }
    }
}
