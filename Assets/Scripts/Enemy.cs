using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 100;
    public bool aggro = false;
    protected bool frozen = false;

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

    public virtual void Freeze()
    {
        frozen = true;
    }
    public virtual void Unfreeze()
    {
        frozen = false;
    }

    public void toggleFreeze()
    {
        if (frozen)
        {
            Unfreeze();
        }
        else
        {
            Freeze();
        }
    }

    public void kill()
    {
        hp = 0;
        Die();
    }

}
