using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TPW : MonoBehaviour
{
    public GameObject primary;
    public GameObject secondary;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [PunRPC]
    public void PrimaryActive()
    {
        primary.SetActive(true);
        secondary.SetActive(false);
    }

    [PunRPC]
    public void SecondaryActive()
    {
        secondary.SetActive(true);
        primary.SetActive(false);
    }
}
