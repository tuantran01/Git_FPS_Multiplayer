using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hastable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;
using UnityEngine.UI;
using System.Threading;
public class RoomTimer : MonoBehaviour
{
    public Text time;
    public bool count;
    public int Time;
    public Manager manager;
    public bool flick;
    ExitGames.Client.Photon.Hashtable setTime = new ExitGames.Client.Photon.Hashtable();
    // Start is called before the first frame update
    void Start()
    {
        count = true;
    }

    // Update is called once per frame
    void Update()
    {
        Time = (int)PhotonNetwork.CurrentRoom.CustomProperties["Time"];
        float minutes = Mathf.FloorToInt((int)PhotonNetwork.CurrentRoom.CustomProperties["Time"] / 60);
        float seconds = Mathf.FloorToInt((int)PhotonNetwork.CurrentRoom.CustomProperties["Time"] % 60);

        time.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        if(PhotonNetwork.IsMasterClient && count)
        {
            StartCoroutine(timer());
            count = false;
        }
        
        if(Time <= 0 && !flick)
        {
            flick = true;
            StartCoroutine(endGame());
        }

        if(flick)
        {
            manager.scoreboardCanvas = true;
            manager.scoreboardUI.SetActive(true);
            time.gameObject.SetActive(false);
        }
    }

    IEnumerator timer()
    {
        yield return new WaitForSeconds(1);
        int nextTime = Time -= 1;
        setTime["Time"] = nextTime;
        PhotonNetwork.CurrentRoom.SetCustomProperties(setTime);
        count = true;
        //PhotonNetwork.CurrentRoom.CustomProperties["Time"] = nextTime;
    }

    IEnumerator endGame()
    {
       yield return new WaitForSeconds(5);
       PhotonNetwork.LeaveRoom();
       Application.LoadLevel(0);
    }
}
