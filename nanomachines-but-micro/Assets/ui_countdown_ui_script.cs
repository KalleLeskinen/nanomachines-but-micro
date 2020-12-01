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
    void Update() //nää boltentityn ettimiset heittää erroria joskus jostain syystä....: You can't access any Bolt specific methods or properties on an entity which is detached
    {
        if (Time.timeSinceLevelLoad > 1 && raceHandler.GetComponent<BoltEntity>().TryFindState<IStateOfRace>(out IStateOfRace state))
        {
            if (Time.frameCount % 10 == 0)
                textfield.GetComponent<Text>().text = Mathf.FloorToInt(state.Clock).ToString();
            if (Mathf.FloorToInt(state.Clock) < 3.3f)
            {
                textfield.transform.parent.gameObject.SetActive(false);
            }
        }
    }
}
