using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lap_slider_handle_text : MonoBehaviour
{
    //private string textArea;
    //private float sliderValue;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    textArea = GetComponent<Text>().text;
    //    sliderValue = GameObject.FindGameObjectWithTag("host_name_laps_ui_slider").GetComponent<Slider>().value;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (Time.timeSinceLevelLoad > 1)
    //    {
    //        textArea = sliderValue.ToString();
    //    }
    //}
    public void changeValue()
    {
        //textArea = GetComponent<Text>().text;
        GetComponent<Text>().text = GameObject.FindGameObjectWithTag("host_name_laps_ui_slider").GetComponent<Slider>().value.ToString();
        //textArea = sliderValue.ToString();
    }
}
