using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LocalEvents : MonoBehaviour
{
    public static LocalEvents Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public event Action<BoltEntity> OnCarInstantiate;

    public void CameraInstantiate(BoltEntity entity)
    {
        OnCarInstantiate?.Invoke(entity);
    }
}
