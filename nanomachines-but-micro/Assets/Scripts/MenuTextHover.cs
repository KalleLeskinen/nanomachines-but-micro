using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTextHover : MonoBehaviour
{

    public GameObject normalText;
    public GameObject hoverText;

    // Update is called once per frame
    void OnMouseOver()
    {
        Debug.Log("ASDASD");
        normalText.SetActive(false);
        hoverText.SetActive(true);
    }

    void OnMouseExit()
    {
        normalText.SetActive(true);
        hoverText.SetActive(false);
    }
}
