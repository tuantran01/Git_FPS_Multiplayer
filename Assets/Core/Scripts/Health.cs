using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class Health : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public float HealSpeed;
    public Slider healthSlider;
    public bool heal;
    public PhotonView PV;
    public Manager manager;
    public bool dead;
    public bool spawnShield;
    public float spawnShieldTime;
    public float SST;
    public GameObject SSUI;
    public GameObject SSUI2;
    public GameObject DamageUI;
    public float damageUITime;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindWithTag("Scripts").GetComponent<Manager>();    
        spawnShield = true;
        SST = spawnShieldTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey && spawnShield)
        {
            PV.RPC("spawnShieldOff", RpcTarget.All);
        }
        if(spawnShield)
        {
            health = maxHealth;
            SST -= 1 * Time.deltaTime;
            SSUI.SetActive(true);
            SSUI2.SetActive(true);
            SSUI2.GetComponent<Text>().text = SST.ToString("0.0");
        }
        else
        {
            SSUI.SetActive(false);
            SSUI2.SetActive(false);
        }

        if(SST <= 0)
        {
           PV.RPC("spawnShieldOff", RpcTarget.All);
        }

        healthSlider.value = health;
        healthSlider.maxValue = maxHealth;

        if(health > maxHealth)
        {
            health = maxHealth;
        }

        if(health < maxHealth && health > 0 && heal)
        {
            health += HealSpeed * Time.deltaTime;
        }
    }

    [PunRPC]
    public void Damage(float damage)
    {
        DamageUI.SetActive(true);
        StartCoroutine(damageUIDelay());
        health -= damage;
        if(PV.IsMine)
        {
            if(health <= 0)
            {
              Die();
            }
            if(health <= 0)
            {
                dead = true;
            }
        }
    }

    IEnumerator damageUIDelay()
    {
        yield return new WaitForSeconds(damageUITime);
        DamageUI.SetActive(false);
    }

    public void Die()
    {
        manager.deaths++;
        manager.cooldown();
        manager.Alive = false;
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    public void spawnShieldOff()
    {
        spawnShield = false;
    }

}
