using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Menu : MonoBehaviourPunCallbacks
{
    public int menusate;

    public GameObject loadingCanvas;
    public GameObject mainCanvas;
    public GameObject createCanvas;
    public GameObject lobbyCanvas;
    public Text LoadingText;
    public int loadingTextState;
    public bool loadingFlick;

    public InputField createRoomIF;

    public Transform roomListHolder;
    public GameObject roomListItemPrefab;
    public GameObject optionsUI;
    public Text quality;

    public int GameMode;
    public Image DMButton;
    public Image TDMButton;
    // Start is called before the first frame update
    void Start()
    {
        loadingFlick = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.NickName = "Player " + Random.Range(0, 99999);
        Debug.Log("Connected to master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        menusate = 1;
    }

    public void DM()
    {
        GameMode = 0;
    }

    public void TDM()
    {
        GameMode = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameMode == 0)
        {
            DMButton.color = Color.green;
            TDMButton.color = Color.white;
        }

        if (GameMode == 1)
        {
            DMButton.color = Color.white;
            TDMButton.color = Color.green;
        }
        Cursor.lockState = CursorLockMode.None;
        int qualityLevel = QualitySettings.GetQualityLevel();

        if (qualityLevel == 0)
        {
            quality.text = "Very Low";
        }
        if (qualityLevel == 1)
        {
            quality.text = "Low";
        }
        if (qualityLevel == 2)
        {
            quality.text = "Medium";
        }
        if (qualityLevel == 3)
        {
            quality.text = "High";
        }
        if (qualityLevel == 4)
        {
            quality.text = "Very High";
        }
        if (qualityLevel == 5)
        {
            quality.text = "Ultra";
        }

        if (loadingFlick == true)
        {
            loadingFlick = false;
            StartCoroutine(loadstatechange());
        }
        if (loadingTextState == 0)
        {
            LoadingText.text = "Loading";
        }
        if (loadingTextState == 1)
        {
            LoadingText.text = "Loading.";
        }
        if (loadingTextState == 2)
        {
            LoadingText.text = "Loading..";
        }
        if (loadingTextState == 3)
        {
            LoadingText.text = "Loading...";
        }

        if (menusate == 0)
        {
            loadingCanvas.SetActive(true);
            mainCanvas.SetActive(false);
            createCanvas.SetActive(false);
            lobbyCanvas.SetActive(false);
            optionsUI.SetActive(false);
        }
        if (menusate == 1)
        {
            loadingCanvas.SetActive(false);
            mainCanvas.SetActive(true);
            createCanvas.SetActive(false);
            lobbyCanvas.SetActive(false);
            optionsUI.SetActive(false);
        }
        if (menusate == 2)
        {
            loadingCanvas.SetActive(false);
            mainCanvas.SetActive(false);
            createCanvas.SetActive(true);
            lobbyCanvas.SetActive(false);
            optionsUI.SetActive(false);
        }
        if (menusate == 3)
        {
            loadingCanvas.SetActive(false);
            mainCanvas.SetActive(false);
            createCanvas.SetActive(false);
            lobbyCanvas.SetActive(true);
            optionsUI.SetActive(false);
        }
        if (menusate == 4)
        {
            loadingCanvas.SetActive(false);
            mainCanvas.SetActive(false);
            createCanvas.SetActive(false);
            lobbyCanvas.SetActive(false);
            optionsUI.SetActive(true);
        }

    }

    IEnumerator loadstatechange()
    {
        yield return new WaitForSeconds(0.25f);
        loadingFlick = true;
        if (loadingTextState < 3)
        {
            loadingTextState++;
        }
        else
        {
            loadingTextState = 0;
        }
    }

    public void createRoom()
    {
        menusate = 2;
    }

    public void Lobby()
    {
        menusate = 3;
    }

    public void Back()
    {
        menusate = 1;
    }

    public void Options()
    {
        menusate = 4;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void QuickStart()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions roomOptions = new RoomOptions();
        string[] lobbyProps = { "GT" };
        Hashtable options = new Hashtable
        {
            { "Time", 600 },
            { "GameMode", GameMode},
            {"redscore", 0 },
            {"bluescore", 0 },
            {"GT", GameMode }

        }; 
        roomOptions.CustomRoomPropertiesForLobby = lobbyProps;
        roomOptions.CustomRoomProperties = options;
        PhotonNetwork.CreateRoom("Room " + Random.Range(0, 1000), roomOptions);
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(createRoomIF.text))
        {
            return;
        }
        RoomOptions roomOptions = new RoomOptions();
        string[] lobbyProps = { "GT" };
        Hashtable options = new Hashtable
        {
            { "Time", 600 },
            { "GameMode", GameMode},
            {"redscore", 0 },
            {"bluescore", 0 },
            {"GT", GameMode }

        }; 
        roomOptions.CustomRoomPropertiesForLobby = lobbyProps;
        roomOptions.CustomRoomProperties = options;

        PhotonNetwork.CreateRoom(createRoomIF.text, roomOptions);
        menusate = 0;
        loadingTextState = 0;
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Development");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Room list updated");
        foreach (Transform trans in roomListHolder)
        {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            {
                continue;
            }
            Instantiate(roomListItemPrefab, roomListHolder).GetComponent<RoomListItem>().configure(roomList[i]);
        }
    }

    public void IncreaseGraphics()
    {
        QualitySettings.IncreaseLevel(true);
    }

    public void DecreaseGraphics()
    {
        QualitySettings.DecreaseLevel(true);
    }

}

