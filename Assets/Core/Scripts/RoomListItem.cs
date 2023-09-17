using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{

    public RoomInfo info;
    public Text playerCount;
    public Text roomID;
    public Text gameMode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void configure (RoomInfo myRoomInfo)
    {
        info = myRoomInfo;
        roomID.text = info.Name;
        if((int)info.CustomProperties["GT"] == 0)
        {
            gameMode.text = "Deathmatch";
        }
        else
        {
            gameMode.text = "Team Deathmatch";
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerCount.text = info.PlayerCount.ToString() + "/" + 16;
    }

    public void Join()
    {
        PhotonNetwork.JoinRoom(info.Name);
    }
}
