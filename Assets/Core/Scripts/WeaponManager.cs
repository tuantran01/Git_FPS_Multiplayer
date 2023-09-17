using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon.StructWrapping;

public class WeaponManager : MonoBehaviour
{
    public GameObject Primary;
    public GameObject Secondary;
    public int curentweapon;
    public TPW tpw;

    // Start is called before the first frame update
    void Start()
    {
        EquipPrimary();
        curentweapon = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && curentweapon != 1)
        {
            curentweapon = 1;
            EquipPrimary();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && curentweapon != 2)
        {
            curentweapon = 2;
            EquipSecondary();
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(curentweapon == 1)
            {
                EquipSecondary();
            }
            else if(curentweapon == 2)
            {
                EquipPrimary();
            }
        }
    }

    public void EquipPrimary()
    {
        if(Secondary.GetComponent<Gun>().aiming)
        {
            Secondary.GetComponent<Gun>().aiming = false;
            Secondary.GetComponent<Gun>().aimObject.GetComponent<Animation>().Play(Secondary.GetComponent<Gun>().AimeReturnAnimName);
            Secondary.GetComponent<Gun>().mainCam.GetComponent<Camera>().fieldOfView = Secondary.GetComponent<Gun>().defaultPOV;
            Secondary.GetComponent<Gun>().weaponCam.GetComponent<Camera>().fieldOfView = Secondary.GetComponent<Gun>().defaultPOV;
        }
        tpw.gameObject.GetPhotonView().RPC("PrimaryActive", RpcTarget.AllBuffered);
        Secondary.SetActive(false);
        Primary.SetActive(true);
        curentweapon = 1;
        Primary.GetComponent<Gun>().Draw();
    }

    public void EquipSecondary()
    {
        if(Primary.GetComponent<Gun>().aiming)
        {
            Primary.GetComponent<Gun>().aiming = false;
            Primary.GetComponent<Gun>().aimObject.GetComponent<Animation>().Play(Primary.GetComponent<Gun>().AimeReturnAnimName);
            Primary.GetComponent<Gun>().mainCam.GetComponent<Camera>().fieldOfView = Primary.GetComponent<Gun>().defaultPOV;
            Primary.GetComponent<Gun>().weaponCam.GetComponent<Camera>().fieldOfView = Primary.GetComponent<Gun>().defaultPOV;
        }
        tpw.gameObject.GetPhotonView().RPC("SecondaryActive", RpcTarget.AllBuffered);
        Primary.SetActive(false);
        Secondary.SetActive(true);
        curentweapon = 2;
        Secondary.GetComponent<Gun>().Draw();
    }
}
