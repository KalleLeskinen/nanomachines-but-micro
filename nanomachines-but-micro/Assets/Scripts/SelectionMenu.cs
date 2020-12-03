using System;
using System.Collections;
using System.Collections.Generic;
using Bolt;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionMenu : GlobalEventListener
{
    public void ButtonConfirm()
    {
        SelectionContainer.Instance.ConfirmSelection();
    }

    public void ButtonBack()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public static event Action ForwardToggle;
    public void ToggleForward()
    {
        ForwardToggle?.Invoke();
    }

    public static event Action BackwardToggle;
    public void ToggleBackward()
    {
        BackwardToggle?.Invoke();
    }
}
