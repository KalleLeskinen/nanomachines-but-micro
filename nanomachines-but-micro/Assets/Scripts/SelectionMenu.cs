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
        SceneManager.LoadScene("MainMenu");
    }

    public void ButtonBack()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
