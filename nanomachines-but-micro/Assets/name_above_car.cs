using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class name_above_car : Bolt.EntityBehaviour<IVehicleState>
{
    private GameObject sign;
    private TextMesh tm;
    // Start is called before the first frame update
    void Start()
    {
        sign = new GameObject("player_label");
        sign.transform.rotation = Camera.main.transform.rotation; // Causes the text faces camera.
        tm = sign.AddComponent<TextMesh>();
        tm.text = state.PlayerName;
        tm.color = new Color(0.8f, 0.8f, 0.8f);
        tm.fontStyle = FontStyle.Bold;
        tm.alignment = TextAlignment.Center;
        tm.anchor = TextAnchor.MiddleCenter;
        tm.characterSize = 0.065f;
        tm.fontSize = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (tm.text == "")
        {
            setName();
        }
        sign.transform.rotation = Camera.main.transform.rotation;
        sign.transform.position = gameObject.transform.position + Vector3.up * 1.5f;
    }

    void setName()
    {
        if (state.PlayerName != "N/A")
        {
            tm.text = state.PlayerName;
        }
    }
}
