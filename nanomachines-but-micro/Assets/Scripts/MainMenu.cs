using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Bolt;
using Bolt.Matchmaking;
using UdpKit;
using UnityEngine.UI;
public class MainMenu : GlobalEventListener
{

    public Button joinGameButtonPrefab;
    public GameObject serverListPanel;
    public float buttonSpacing;
    private string roomName;

    private List<Button> _joinServerButtons = new List<Button>();

    public void ButtonStartServer()
    {
        BoltLauncher.StartServer();
    }

    public void ButtonStartClient()
    {
        BoltLauncher.StartClient();
    }

    public void ButtonQuitGame()
    {
        Application.Quit();
    }

    public void ButtonSelectCar()
    {
        SceneManager.LoadScene("GarageScene");
    }

    public void SetRoomName()
    {
        roomName = GameObject.FindGameObjectWithTag("ServerName").GetComponent<Text>().text;
    }

    public override void BoltStartDone()
    {
        if (BoltNetwork.IsServer)
        {
            int randomInt = Random.Range(0, 9999);
            string matchName = roomName + randomInt;

            BoltMatchmaking.CreateSession(
                sessionID: matchName,
                sceneToLoad: "Level_1" // <-  What scene to load.... muutettu level 1 koska garage scene bugaa atm... T:jonni
            );

        }
    }

    //LobbySystem behaviour
    public override void SessionListUpdated(Map<System.Guid, UdpSession> sessionList)
    {
        //Clear excess buttons
        ClearSessions();

        foreach (var session in sessionList)
        {
            UdpSession photonSession = session.Value as UdpSession;

            Button joinGameButtonClone = Instantiate(joinGameButtonPrefab);
            joinGameButtonClone.transform.parent = serverListPanel.transform;
            joinGameButtonClone.transform.localPosition = new Vector3(0, buttonSpacing * _joinServerButtons.Count, 0);
            joinGameButtonClone.GetComponentInChildren<Text>().text = "Test match " + (_joinServerButtons.Count + 1);
            joinGameButtonClone.gameObject.SetActive(true);

            joinGameButtonClone.onClick.AddListener(() => JoinGame(photonSession));

            _joinServerButtons.Add(joinGameButtonClone);
            /*if (photonSession.Source == UdpSessionSource.Photon)
            {
                
            }*/
        }
    }

    private void JoinGame(UdpSession photonSession)
    {
        BoltMatchmaking.JoinSession(photonSession);
    }

    //Refresh sessions
    private void ClearSessions()
    {
        Debug.Log("ClearSessions was called");

        foreach (Button button in _joinServerButtons)
        {
            Destroy(button.gameObject);
        }

        _joinServerButtons.Clear();
    }

}
