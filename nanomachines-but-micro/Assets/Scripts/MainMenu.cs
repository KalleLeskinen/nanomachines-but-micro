using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Bolt;
using Bolt.Matchmaking;
using UdpKit;
using Unity.Collections;
using UnityEngine.UI;
public class MainMenu : GlobalEventListener
{
    public Button joinGameButtonPrefab;
    public GameObject serverListPanel;
    public float buttonSpacing;
    private string roomName;

    private List<Button> _joinServerButtons = new List<Button>();

    private GameObject[] modelPrefabs;
    
    public string SceneToLoad { get; set; }

    private void Awake()
    {
        if (BoltNetwork.IsRunning) { BoltNetwork.Shutdown(); }
        SceneToLoad = "Level_1";
        modelPrefabs = new GameObject[7];
        modelPrefabs[0] = Resources.Load("Car1_Torino_Model") as GameObject;
        modelPrefabs[1] = Resources.Load("Car2_Torino_Model") as GameObject;
        modelPrefabs[2] = Resources.Load("Car3_Torino_Model") as GameObject;
        modelPrefabs[3] = Resources.Load("Truck-1_Model") as GameObject;
        modelPrefabs[4] = Resources.Load("Truck-2_Model") as GameObject;
        modelPrefabs[5] = Resources.Load("TruckV1Model") as GameObject;
        modelPrefabs[6] = Resources.Load("TruckV2Model") as GameObject;
    }

    private void Start()
    {
        if (SelectionContainer.Instance == null)
        {
            GameObject defaultCar = Instantiate(modelPrefabs[0], new Vector3(-30f, -10f, 50f),
                Quaternion.Euler(new Vector3(0, 180, 0)));
            defaultCar.transform.localScale = new Vector3(14, 14, 14);
        }
        else if (SelectionContainer.Instance != null)
        {
            GameObject selectedCar = Instantiate(modelPrefabs[SelectionContainer.Instance.prefabIdInteger],
                new Vector3(-30f, -10f, 50f), Quaternion.Euler(new Vector3(0, 180, 0)));
                selectedCar.transform.localScale = new Vector3(14, 14, 14);
        }
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

    public void ButtonLevelOne()
    {
        SceneToLoad = "Level_1";
    }

    

    public void ButtonLevelTwo()
    {
        SceneToLoad = "Level_2";
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
                sceneToLoad: SceneToLoad // <-  What scene to load.... muutettu level 1 koska garage scene bugaa atm... T:jonni
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
            joinGameButtonClone.transform.SetParent(serverListPanel.transform);
            joinGameButtonClone.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 100 + buttonSpacing * _joinServerButtons.Count, 0);
            joinGameButtonClone.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
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

    public void BoltShutDown()
    {
       BoltLauncher.Shutdown();
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
