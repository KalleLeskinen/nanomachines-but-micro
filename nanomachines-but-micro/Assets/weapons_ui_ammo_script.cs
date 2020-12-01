using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class weapons_ui_ammo_script : MonoBehaviour
{
    public GameObject myCar;
    private void Update()
    {
        
        if (Time.timeSinceLevelLoad > 1 && myCar == null)
        {
            foreach (var car in BoltNetwork.Entities)
            {
                if (car.TryFindState<IVehicleState>(out IVehicleState state) && car.IsOwner)
                {
                    myCar = car;
                }
            }
        }
        if (Time.frameCount % 30 == 0 && myCar)
        {
            UpdateAmmo();
        }
    }
    void UpdateAmmo()
    {
        gameObject.GetComponent<Text>().text = myCar.GetComponent<BoltEntity>().GetState<IVehicleState>().AmmoCount.ToString();
    }
}
