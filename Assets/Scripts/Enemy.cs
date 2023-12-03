using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType
    {
        Runner,
        Ranger,
        Boss
    }

    public int hp = 100;
    public int maxHp = 100;
    public bool aggro = false;
    protected bool frozen = false;

    public EnemyType enemyType;

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
        gameObject.SetActive(false);
        frozen = false;
        gameManager.playerScript.freezing = false;
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

    public void ToggleFreeze()
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

    public void Kill()
    {
        hp = 0;
        Die();
    }

}
