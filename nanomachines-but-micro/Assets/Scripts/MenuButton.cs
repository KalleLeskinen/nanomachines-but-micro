using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public TextMeshProUGUI tmpObj;

    Color originalColor;

    void Start()
    {
        originalColor = tmpObj.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tmpObj.color = new Color32(255, 255, 255, 255);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tmpObj.color = originalColor;
    }

}
