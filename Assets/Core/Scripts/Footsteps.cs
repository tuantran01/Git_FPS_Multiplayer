using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Footsteps : MonoBehaviour
{
    public Character characterScript;
    public AudioClip[] footsteps;
    public GameObject footstepsSoundObject;
    public bool flick;
    public bool walking;
    public bool running;
    public float walkTime;
    public float runTime;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (characterScript.isWalking)
        {
            walking = true;
        }
        else
        {
            walking = false;
        }

        if (characterScript.isSprinting)
        {
            running = true;
        }
        else
        {
            running = false;
        }

        if (running && !flick || walking && !flick)
        {
            if (characterScript.isGrounded)
            {
                GetComponent<PhotonView>().RPC("PlayFootstep", RpcTarget.All);
                flick = true;
            }
        }
    }

    [PunRPC]
    public void PlayFootstep()
    {
        int footstep = Random.Range(0, footsteps.Length);
        footstepsSoundObject.GetComponent<AudioSource>().PlayOneShot(footsteps[footstep]);
        StartCoroutine(footstepDelay());
    }

    IEnumerator footstepDelay()
    {
        if (walking)
        {
            yield return new WaitForSeconds(walkTime);
            flick = false;
        }

        else
        {
            yield return new WaitForSeconds(runTime);
            flick = false;
        }
    }
}
