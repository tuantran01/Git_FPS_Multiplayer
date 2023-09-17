using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public int Health;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Health <= 0)
        {
            Die();
        }
    }

    public void TakeDamge(int damge)
    {
        Health -= damge;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
