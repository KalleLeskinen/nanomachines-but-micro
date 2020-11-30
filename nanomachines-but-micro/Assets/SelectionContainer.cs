﻿using System;
using System.Collections;
using System.Collections.Generic;
using Bolt;
using FMODUnity;
using UnityEngine;

public class SelectionContainer : MonoBehaviour
{
    public int prefabIdInteger;

    public static SelectionContainer Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ConfirmSelection()
    {
        prefabIdInteger = VehicleSelection.Instance.i;
    }
}