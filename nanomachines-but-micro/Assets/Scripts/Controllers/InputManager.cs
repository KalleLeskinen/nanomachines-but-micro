using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InputManager : BoltSingletonPrefab<InputManager>, IInputManager
{
    
    public bool GetButton(int playerId, InputAction action)
    {
        throw new System.NotImplementedException();
    }

    public bool GetButtonDown(int playerId, InputAction action)
    {
        throw new System.NotImplementedException();
    }

    public bool GetButtonUp(int playerId, InputAction action)
    {
        throw new System.NotImplementedException();
    }

    public float GetAxis(int playerId, InputAction action)
    {
        throw new System.NotImplementedException();
    }
}