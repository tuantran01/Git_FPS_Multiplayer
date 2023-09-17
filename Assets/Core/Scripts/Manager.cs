using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Manager : MonoBehaviourPunCallbacks
{
    private ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    private ExitGames.Client.Photon.Hashtable setTime = new ExitGames.Client.Photon.Hashtable();
    private ExitGames.Client.Photon.Hashtable setTeam = new ExitGames.Client.Photon.Hashtable();




    //0 = None 1 = Spawn 2 = EscapeMenu
    public int UIState;
    public GameObject SpawnUI;
    public bool Alive;
    public Transform[] SpawnPoints;
    public float VariableCooldownTime;

    public float cooldownTime;
    public bool Cooldown;
    public Slider cooldownSlider;
    public Text respawnText;
    public string respawnTextMessage;
    public GameObject cooldownUI;
    public bool scoreboardCanvas;
    public GameObject scoreboardUI;
    public GameObject escapeUI;
    public float score;
    public float kills;
    public float deaths;

    public GameObject Player;
    public GameObject optionsUI;
    public bool options;
    public Text quality;

    public bool endgameflick;
    public bool suicideflick;

    public Text highestKillsUI;
    public Text mykillsUI;
    public int highestKills;
    public int KILLS;

    public GameObject TDMscoreboardUI;
    public GameObject[] DMUI;
    public GameObject[] TDMUI;
    public int Team;
    public Text redscore;
    public Text bluescore;
    // Start is called before the first frame update
    void Start()
    {
        Team = Random.Range(0, 100);
        UIState = 1;
        if ((int)PhotonNetwork.CurrentRoom.CustomProperties["GameMode"] == 1)
        {
            scoreboardUI = TDMscoreboardUI;
        }
    }

    public void cooldown()
    {
        Cooldown = true;
        cooldownTime = VariableCooldownTime;
    }

    [PunRPC]
    public void UpdateHighestKills(int KILLS2)
    {
        highestKills = KILLS2;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        highestKills = 0;
    }

    // Update is called once per frame
    void Update()
    {
        redscore.text = PhotonNetwork.CurrentRoom.CustomProperties["redscore"].ToString();
        bluescore.text = PhotonNetwork.CurrentRoom.CustomProperties["bluescore"].ToString();
        if ((int)PhotonNetwork.CurrentRoom.CustomProperties["GameMode"] == 0)
        {
            foreach (GameObject dm in DMUI)
            {
                dm.SetActive(true);
            }

            foreach (GameObject tdm in TDMUI)
            {
                tdm.SetActive(false);
            }
        }
        if ((int)PhotonNetwork.CurrentRoom.CustomProperties["GameMode"] == 1)
        {
            foreach (GameObject dm in DMUI)
            {
                dm.SetActive(false);
            }

            foreach (GameObject tdm in TDMUI)
            {
                tdm.SetActive(true);
            }
        }

        if (highestKills == 50)
        {
            setTime["Time"] = 0;
            PhotonNetwork.CurrentRoom.SetCustomProperties(setTime);
        }
        KILLS = (int)kills;
        mykillsUI.text = KILLS.ToString();
        highestKillsUI.text = highestKills.ToString();

        if (kills > highestKills)
        {
            GetComponent<PhotonView>().RPC("UpdateHighestKills", RpcTarget.AllBuffered, KILLS);
        }

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

        if (Input.GetKey(KeyCode.Tab))
        {
            scoreboardCanvas = true;
        }
        else
        {
            scoreboardCanvas = false;
        }

        if (scoreboardCanvas && !endgameflick)
        {
            scoreboardUI.SetActive(true);
        }
        else
        {
            scoreboardUI.SetActive(false);
        }

        respawnText.text = respawnTextMessage + " " + cooldownTime.ToString("0.0");
        cooldownSlider.maxValue = VariableCooldownTime;
        cooldownSlider.value = cooldownTime;

        if (Cooldown)
        {
            cooldownTime -= 1 * Time.deltaTime;
            UIState = 2;
        }

        if (cooldownTime <= 0)
        {
            Cooldown = false;
        }

        if (!Alive && Cooldown == false && !options)
        {
            UIState = 1;
        }
        if (UIState == 0)
        {
            SpawnUI.SetActive(false);
            cooldownUI.SetActive(false);
            optionsUI.SetActive(false);
            escapeUI.SetActive(false);
        }
        if (UIState == 1)
        {
            SpawnUI.SetActive(true);
            cooldownUI.SetActive(false);
            escapeUI.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            optionsUI.SetActive(false);

        }
        if (UIState == 2)
        {
            SpawnUI.SetActive(false);
            cooldownUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            escapeUI.SetActive(false);
            optionsUI.SetActive(false);
        }
        if (UIState == 3)
        {
            SpawnUI.SetActive(false);
            cooldownUI.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            escapeUI.SetActive(true);
            optionsUI.SetActive(false);
        }
        if (UIState == 4)
        {
            SpawnUI.SetActive(false);
            cooldownUI.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            escapeUI.SetActive(false);
            optionsUI.SetActive(true);
        }

        if (PhotonNetwork.InRoom)
        {
            playerProperties["score"] = score;
            playerProperties["kills"] = kills;
            playerProperties["deaths"] = deaths;

            if(Team < 50)
            {
                playerProperties["TEAM"] = 1;
            }
            else
            {
                playerProperties["TEAM"] = 0;
            }
            PhotonNetwork.LocalPlayer.CustomProperties = playerProperties;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
        }
        if (Alive && Input.GetKeyDown(KeyCode.Escape) && UIState != 3)
        {
            UIState = 3;
            return;
        }

        if (Alive && Input.GetKeyDown(KeyCode.Escape) && UIState == 3)
        {
            Cursor.lockState = CursorLockMode.Locked;
            UIState = 0;
            return;
        }

        if (cooldownTime <= 0 && !Alive && suicideflick)
        {
            UIState = 1;
            suicideflick = false;
        }
    }

    public void Spawn()
    {
        int spawn = Random.Range(0, SpawnPoints.Length);
        Player = PhotonNetwork.Instantiate("Player", SpawnPoints[spawn].position, Quaternion.identity, 0);
        UIState = 0;
        Alive = true;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        Application.LoadLevel("Menu");
    }

    public void Suicide()
    {
        PhotonNetwork.Destroy(Player);
        UIState = 2;
        Cooldown = true;
        cooldownTime = VariableCooldownTime;
        Alive = false;
        suicideflick = true;
    }

    public void Resume()
    {
        UIState = 0;
    }

    public void Options()
    {
        options = true;
        UIState = 4;
    }
    public void Back()
    {
        if (Alive)
        {
            UIState = 3;
        }
        else
        {
            UIState = 1;
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
