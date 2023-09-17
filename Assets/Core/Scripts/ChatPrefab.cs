using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ChatPrefab : MonoBehaviour
{
    public Text Username;
    public Text Message;
    public bool local;
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<PhotonView>().IsMine)
        {
            StartCoroutine(deleteDelay());
        }
       
    }

    IEnumerator deleteDelay()
    {
        yield return new WaitForSeconds(15);
        if(GetComponent<PhotonView>().IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(local)
        {
            Username.color = Color.yellow;
            Message.color = Color.yellow;
        }
    }

    [PunRPC]
    public void MesssageUser(string username)
    {
        Username.text = username;
        if(username == PhotonNetwork.NickName)
        {
            local = true;
        }
    }

    [PunRPC]
    public void MessageContent(string message)
    {
        Message.text = message;
        
    }
}
