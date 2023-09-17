using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon.StructWrapping;

public class Killfeed : MonoBehaviour
{
    public Transform KillFeedArea;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public void PlayerKilled (string killer, string killed)
    {
        GameObject prefab = PhotonNetwork.Instantiate("KillFeedPrefab", KillFeedArea.position, Quaternion.identity);
        prefab.transform.SetParent(KillFeedArea);
        prefab.transform.SetAsFirstSibling();
        prefab.GetComponent<PhotonView>().RPC("UpdateNames", RpcTarget.All, killer, killed);
    }
}
