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

    private void Start()
    {
        Debug.Log(VehicleSelection.Instance);
        //Instantiate(menuCars[random], new Vector3(-30f, -10f, 50f), Quaternion.Euler(new Vector3(0, 180, 0)));
    }

    public void ButtonStartServer()
    {
        SetRoomName();
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
        if (roomName == "")
        {
            roomName = "<unnamed lobby>";
        }
    }

    public override void BoltStartDone()
    {
        if (BoltNetwork.IsServer)
        {
            //int randomInt = Random.Range(0, 9999);
            string matchName = roomName;

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
            Debug.Log("Session found " + photonSession.HostName);
            Button joinGameButtonClone = Instantiate(joinGameButtonPrefab);
            joinGameButtonClone.transform.parent = serverListPanel.transform;
            joinGameButtonClone.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 100 + buttonSpacing * _joinServerButtons.Count, 0);
            joinGameButtonClone.GetComponentInChildren<Text>().text = photonSession.HostName;
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
