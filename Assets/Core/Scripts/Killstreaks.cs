using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Killstreaks : MonoBehaviour
{
    public GameObject EnemyKilledUI;
    public GameObject DoubleKillUI;
    public GameObject TripleKillUI;
    public GameObject QuadKillUI;
    public GameObject PentaKillUI;
    public GameObject HexaKillUI;
    public GameObject KillingSpreeUI;

    public int killstreak;
    public float killstreakTime;
    public bool addPoints;
    public Score score;
    public Manager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindWithTag("Scripts").GetComponent<Manager>();

    }

    // Update is called once per frame
    void Update()
    {
        killstreakTime -= 1 * Time.deltaTime;

        if (killstreakTime <= 0)
        {
            killstreak = 0;
        }

        if (killstreak == 1)
        {
            EnemyKilledUI.SetActive(true);
            DoubleKillUI.SetActive(false);
            TripleKillUI.SetActive(false);
            QuadKillUI.SetActive(false);
            PentaKillUI.SetActive(false);
            HexaKillUI.SetActive(false);
            KillingSpreeUI.SetActive(false);
            if (addPoints)
            {
                score.AddScore(100);
                manager.score += 100;
            }
            addPoints = false;
        }

        if (killstreak == 2)
        {
            EnemyKilledUI.SetActive(false);
            DoubleKillUI.SetActive(true);
            TripleKillUI.SetActive(false);
            QuadKillUI.SetActive(false);
            PentaKillUI.SetActive(false);
            HexaKillUI.SetActive(false);
            KillingSpreeUI.SetActive(false);
            if (addPoints)
            {
                score.AddScore(200);
                manager.score += 200;
            }
            addPoints = false;
        }

        if (killstreak == 3)
        {
            EnemyKilledUI.SetActive(false);
            DoubleKillUI.SetActive(false);
            TripleKillUI.SetActive(true);
            QuadKillUI.SetActive(false);
            PentaKillUI.SetActive(false);
            HexaKillUI.SetActive(false);
            KillingSpreeUI.SetActive(false);
            if (addPoints)
            {
                score.AddScore(300);
                manager.score += 300;
            }
            addPoints = false;
        }

        if (killstreak == 4)
        {
            EnemyKilledUI.SetActive(false);
            DoubleKillUI.SetActive(false);
            TripleKillUI.SetActive(false);
            QuadKillUI.SetActive(true);
            PentaKillUI.SetActive(false);
            HexaKillUI.SetActive(false);
            KillingSpreeUI.SetActive(false);
            if (addPoints)
            {
                score.AddScore(400);
                manager.score += 400;
            }
            addPoints = false;
        }

        if (killstreak == 5)
        {
            EnemyKilledUI.SetActive(false);
            DoubleKillUI.SetActive(false);
            TripleKillUI.SetActive(false);
            QuadKillUI.SetActive(false);
            PentaKillUI.SetActive(true);
            HexaKillUI.SetActive(false);
            KillingSpreeUI.SetActive(false);
            if (addPoints)
            {
                score.AddScore(500);
                manager.score += 500;
            }
            addPoints = false;
        }

        if (killstreak == 6)
        {
            EnemyKilledUI.SetActive(false);
            DoubleKillUI.SetActive(false);
            TripleKillUI.SetActive(false);
            QuadKillUI.SetActive(false);
            PentaKillUI.SetActive(false);
            HexaKillUI.SetActive(true);
            KillingSpreeUI.SetActive(false);
            if (addPoints)
            {
                score.AddScore(600);
                manager.score += 600;
            }
            addPoints = false;
        }

        if (killstreak >= 7)
        {
            EnemyKilledUI.SetActive(false);
            DoubleKillUI.SetActive(false);
            TripleKillUI.SetActive(false);
            QuadKillUI.SetActive(false);
            PentaKillUI.SetActive(false);
            HexaKillUI.SetActive(false);
            KillingSpreeUI.SetActive(true);
            if (addPoints)
            {
                score.AddScore(750);
                manager.score += 750;
            }
            addPoints = false;
        }

        if (killstreak == 0)
        {
            EnemyKilledUI.SetActive(false);
            DoubleKillUI.SetActive(false);
            TripleKillUI.SetActive(false);
            QuadKillUI.SetActive(false);
            PentaKillUI.SetActive(false);
            HexaKillUI.SetActive(false);
            KillingSpreeUI.SetActive(false);
            addPoints = false;
        }
    }

    public void KillstreakAdd()
    {
        killstreak++;
        killstreakTime = 5.0f;
        addPoints = true;
    }


}
