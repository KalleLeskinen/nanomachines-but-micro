using System;
using System.Collections;
using System.Collections.Generic;
using Bolt;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionMenu : GlobalEventListener
{
    public void ButtonConfirm()
    {
        SelectionContainer.Instance.ConfirmSelection();
        SceneManager.LoadScene("MainMenu");
    }

    public void ButtonBack()
    {
        SelectionContainer.Instance.ConfirmSelection();
        SceneManager.LoadScene("MainMenu");
    }

}
