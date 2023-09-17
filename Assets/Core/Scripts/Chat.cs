using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Chat : MonoBehaviour
{
    public bool ChatOpen;
    public InputField ChatField;
    public Transform ChatHolder;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && ChatOpen)
        {
            if(ChatField.text != "")
            {
                GetComponent<PhotonView>().RPC("SendRoomMessage", RpcTarget.All, ChatField.text, PhotonNetwork.NickName);
            }
        }
        if(Input.GetKeyDown(KeyCode.Return) && !ChatOpen)
        {
            ChatField.text = "";
        }
        if(Input.GetKeyDown(KeyCode.Return))
        {
            ChatOpen = !ChatOpen;
        }  
        if(ChatOpen)
        {
            ChatField.gameObject.SetActive(true);
            ChatField.ActivateInputField();
        }
        else
        {
            ChatField.gameObject.SetActive(false);
        }

    }

    [PunRPC]
    public void SendRoomMessage(string message, string name)
    {
        GameObject mo = PhotonNetwork.Instantiate("roomChatPrefab", ChatHolder.position, Quaternion.identity);
        mo.transform.SetParent(ChatHolder);
        mo.transform.SetAsFirstSibling();
        mo.GetComponent<PhotonView>().RPC("MesssageUser", RpcTarget.All, name);
        mo.GetComponent<PhotonView>().RPC("MessageContent", RpcTarget.All, message);
    }
}
