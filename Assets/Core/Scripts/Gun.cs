using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    private ExitGames.Client.Photon.Hashtable updateScore = new ExitGames.Client.Photon.Hashtable();
    public float Damage;
    public float minDamage;
    public float maxDamage;

    public int Ammo;
    public int AmmoPerClip;
    public int AmmoLeft;

    public float fireTime;
    public float reloadTime;
    public float drawTime;
    public bool canFire;

    public Text AmmoText;
    public Text AmmoLeftText;

    public string FireAnimName;
    public string ReloadAnimName;
    public string DrawAnimName;
    public string AimAnimName;
    public string AimeReturnAnimName;

    public GameObject aimObject;
    public bool aiming;
    public AudioClip FireSound;

    public GameObject Flash;
    public GameObject muzzleLight;
    public float effectTime;

    public GameObject i;
    public GameObject damagePrefab;
    public Transform xpSpawn;
    public Transform xpParent;

    public Score score;
    public Killstreaks ks;

    public Manager manager;
    public Character characterScript;
    public Killfeed killfeed;
    public GameObject player;

    public GameObject weaponCam;
    public GameObject mainCam;
    public int defaultPOV;
    public int aimPOV;
    public bool canAIM;
    public float POV;


    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindWithTag("Scripts").GetComponent<Manager>();
        killfeed = GameObject.FindWithTag("Scripts").GetComponent<Killfeed>();
        Draw();
    }

    public void Draw()
    {
        muzzleLight.SetActive(false);
        Flash.SetActive(false);
        canFire = false;
        gameObject.GetComponent<Animation>().Play(DrawAnimName);
        StartCoroutine(drawDelay());
    }

    IEnumerator drawDelay()
    {
        yield return new WaitForSeconds(drawTime);
        canFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (aiming && canAIM && !characterScript.isSprinting)
        {
            if (weaponCam.GetComponent<Camera>().fieldOfView > aimPOV)
            {
                weaponCam.GetComponent<Camera>().fieldOfView -= aimPOV * Time.deltaTime;
            }
            if (mainCam.GetComponent<Camera>().fieldOfView > aimPOV)
            {
                mainCam.GetComponent<Camera>().fieldOfView -= aimPOV * Time.deltaTime;
            }
        }
        else if (canAIM && !characterScript.isSprinting)
        {
            if (weaponCam.GetComponent<Camera>().fieldOfView < defaultPOV)
            {
                weaponCam.GetComponent<Camera>().fieldOfView += aimPOV * Time.deltaTime;
            }
            if (mainCam.GetComponent<Camera>().fieldOfView < defaultPOV)
            {
                mainCam.GetComponent<Camera>().fieldOfView += aimPOV * Time.deltaTime;
            }
        }

        if (!aiming && Input.GetMouseButtonDown(1) && canAIM && !characterScript.isSprinting)
        {
            aiming = true;
            aimObject.GetComponent<Animation>().Play(AimAnimName);
        }

        if (aiming && Input.GetMouseButtonUp(1) && canAIM && !characterScript.isSprinting)
        {
            aiming = false;
            aimObject.GetComponent<Animation>().Play(AimeReturnAnimName);
        }

        AmmoText.text = Ammo.ToString();
        AmmoLeftText.text = "/ " + AmmoLeft.ToString();

        if (Input.GetMouseButton(0) && canFire && Ammo > 0 && !characterScript.isSprinting)
        {
            Fire();
        }
        else if (Ammo == 0 && AmmoLeft > 0 && Input.GetMouseButton(0) && canFire && !characterScript.isSprinting && AmmoLeft > 0 && Ammo < AmmoPerClip)
        {
            Reload();
        }

        if (Input.GetKey(KeyCode.R) && canFire && AmmoLeft > 0 && !characterScript.isSprinting && Ammo < AmmoPerClip)
        {
            Reload();
        }
    }

    void Fire()
    {
        Damage = Random.Range(minDamage, maxDamage);
        //GetComponent<AudioSource>().PlayOneShot(FireSound);
        player.GetComponent<PhotonView>().RPC("AssaultFire", RpcTarget.All);
        GetComponent<Animation>().Stop(FireAnimName);
        GetComponent<Animation>().Play(FireAnimName);
        Ammo -= 1;
        canFire = false;
        StartCoroutine(fireDelay());
        Flash.SetActive(true);
        muzzleLight.SetActive(true);
        StartCoroutine(effectDelay());

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                //Team Deathmatch
                if ((int)PhotonNetwork.CurrentRoom.CustomProperties["GameMode"] == 1)
                {
                    if (hit.collider.gameObject.GetComponent<Health>().dead == false
                    && hit.collider.gameObject.GetComponent<Health>().spawnShield == false
                    && hit.collider.gameObject.GetComponent<Helper>().Team != (int)PhotonNetwork.LocalPlayer.CustomProperties["TEAM"])
                    {
                        if (hit.collider.gameObject.GetComponent<Health>().health <= Damage)
                        {
                            string otherPlayerNickname;
                            otherPlayerNickname = hit.collider.gameObject.GetComponent<Helper>().NICKNAME;
                            killfeed.gameObject.GetComponent<PhotonView>().RPC("PlayerKilled", RpcTarget.All, PhotonNetwork.NickName, otherPlayerNickname);
                            //killfeed.PlayerKilled(PhotonNetwork.NickName, otherPlayerNickname);
                            Damage = hit.collider.gameObject.GetComponent<Health>().health;
                            hit.collider.GetComponent<PhotonView>().RPC("Damage", RpcTarget.All, Damage);
                            i = Instantiate(damagePrefab, xpSpawn.position, xpSpawn.rotation);
                            i.transform.SetParent(xpParent);
                            i.GetComponent<Text>().text = "+" + Damage.ToString("0");
                            score.AddScore(Damage);
                            manager.score += (float)Damage;
                            ks.KillstreakAdd();
                            manager.kills++;
                            if ((int)PhotonNetwork.CurrentRoom.CustomProperties["GameMode"] == 1)
                            {
                                if ((int)PhotonNetwork.LocalPlayer.CustomProperties["TEAM"] == 0)
                                {
                                    updateScore["redscore"] = (int)PhotonNetwork.CurrentRoom.CustomProperties["redscore"] + 1;
                                    PhotonNetwork.CurrentRoom.SetCustomProperties(updateScore);
                                }
                                else
                                {
                                    updateScore["bluescore"] = (int)PhotonNetwork.CurrentRoom.CustomProperties["bluescore"] + 1;
                                    PhotonNetwork.CurrentRoom.SetCustomProperties(updateScore);
                                }
                            }
                        }
                        else
                        {
                            //hit.collider.gameObject.GetComponent<AI>().TakeDamge(Damage);
                            hit.collider.GetComponent<PhotonView>().RPC("Damage", RpcTarget.All, Damage);
                            i = Instantiate(damagePrefab, xpSpawn.position, xpSpawn.rotation);
                            i.transform.SetParent(xpParent);
                            i.GetComponent<Text>().text = "+" + Damage.ToString("0");
                            score.AddScore(Damage);
                            manager.score += (float)Damage;
                        }
                    }
                }
                else
                {
                    //Deathmatch
                    if (hit.collider.gameObject.GetComponent<Health>().dead == false
                        && hit.collider.gameObject.GetComponent<Health>().spawnShield == false)
                    {
                        if (hit.collider.gameObject.GetComponent<Health>().health <= Damage)
                        {
                            string otherPlayerNickname;
                            otherPlayerNickname = hit.collider.gameObject.GetComponent<Helper>().NICKNAME;
                            killfeed.gameObject.GetComponent<PhotonView>().RPC("PlayerKilled", RpcTarget.All, PhotonNetwork.NickName, otherPlayerNickname);
                            //killfeed.PlayerKilled(PhotonNetwork.NickName, otherPlayerNickname);
                            Damage = hit.collider.gameObject.GetComponent<Health>().health;
                            hit.collider.GetComponent<PhotonView>().RPC("Damage", RpcTarget.All, Damage);
                            i = Instantiate(damagePrefab, xpSpawn.position, xpSpawn.rotation);
                            i.transform.SetParent(xpParent);
                            i.GetComponent<Text>().text = "+" + Damage.ToString("0");
                            score.AddScore(Damage);
                            manager.score += (float)Damage;
                            ks.KillstreakAdd();
                            manager.kills++;
                            if ((int)PhotonNetwork.CurrentRoom.CustomProperties["GameMode"] == 1)
                            {
                                if ((int)PhotonNetwork.LocalPlayer.CustomProperties["TEAM"] == 0)
                                {
                                    updateScore["redscore"] = (int)PhotonNetwork.CurrentRoom.CustomProperties["redscore"] + 1;
                                    PhotonNetwork.CurrentRoom.SetCustomProperties(updateScore);
                                }
                                else
                                {
                                    updateScore["bluescore"] = (int)PhotonNetwork.CurrentRoom.CustomProperties["bluescore"] + 1;
                                    PhotonNetwork.CurrentRoom.SetCustomProperties(updateScore);
                                }
                            }
                        }
                        else
                        {
                            //hit.collider.gameObject.GetComponent<AI>().TakeDamge(Damage);
                            hit.collider.GetComponent<PhotonView>().RPC("Damage", RpcTarget.All, Damage);
                            i = Instantiate(damagePrefab, xpSpawn.position, xpSpawn.rotation);
                            i.transform.SetParent(xpParent);
                            i.GetComponent<Text>().text = "+" + Damage.ToString("0");
                            score.AddScore(Damage);
                            manager.score += (float)Damage;
                        }
                    }
                }
            }
        }
    }



    IEnumerator fireDelay()
    {
        yield return new WaitForSeconds(fireTime);
        canFire = true;
    }

    IEnumerator effectDelay()
    {
        yield return new WaitForSeconds(effectTime);
        muzzleLight.SetActive(false);
        Flash.SetActive(false);
    }

    void Reload()
    {
        canFire = false;
        GetComponent<Animation>().Play(ReloadAnimName);
        if (Ammo == 0 && AmmoLeft >= AmmoPerClip)
        {
            StartCoroutine(reloadDelay1());
            return;
        }
        if (Ammo == 0 && AmmoLeft < AmmoPerClip && AmmoLeft >= 1)
        {
            StartCoroutine(reloadDelay2());
            return;
        }
        StartCoroutine(reloadDelay3());

    }

    IEnumerator reloadDelay1()
    {
        yield return new WaitForSeconds(reloadTime);
        Ammo = AmmoPerClip;
        AmmoLeft -= AmmoPerClip;
        canFire = true;
    }

    IEnumerator reloadDelay2()
    {
        yield return new WaitForSeconds(reloadTime);
        Ammo = AmmoLeft;
        AmmoLeft = 0;
        canFire = true;
    }

    IEnumerator reloadDelay3()
    {
        yield return new WaitForSeconds(reloadTime);
        int amoint;
        amoint = AmmoPerClip - Ammo;
        if (Ammo == 0 && AmmoLeft <= AmmoPerClip)
        {
            amoint = AmmoLeft;
        }

        if (AmmoLeft < AmmoPerClip)
        {
            if (AmmoLeft <= AmmoPerClip)
            {
                amoint = AmmoLeft;
            }
        }
        Ammo += amoint;
        AmmoLeft -= amoint;
        canFire = true;
    }
}
