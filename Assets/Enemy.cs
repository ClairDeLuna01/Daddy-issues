using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 100;
    public bool aggro = false;

    protected GameManager gameManager;
    protected Rigidbody rb;

    protected void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody>();
    }

    public void Hit(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void Aggro()
    {
        aggro = true;
    }

    void Deaggro()
    {
        aggro = false;
    }
}
