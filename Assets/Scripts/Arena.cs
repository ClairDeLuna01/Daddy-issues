using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    public GameObject[] doors;
    public bool active = false;
    public GameObject[] enemies;
    private Enemy[] enemyScripts;

    static int IDCounter = 0;
    public int ID = IDCounter++;

    private GameManager gameManager;
    public bool cleared = false;

    Vector3[] enemyStartPositions;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        enemyScripts = new Enemy[enemies.Length];
        enemyStartPositions = new Vector3[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            enemyScripts[i] = enemies[i].GetComponent<Enemy>();
            enemyStartPositions[i] = enemies[i].transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !cleared && !active)
        {
            foreach (GameObject door in doors)
            {
                door.SetActive(true);
            }
            foreach (GameObject enemy in enemies)
            {
                enemy.SetActive(true);
                enemy.GetComponent<Enemy>().aggro = true;
            }
            active = true;
            gameManager.EnterCombat();
        }
    }

    void Update()
    {
        if (active)
        {
            foreach (Enemy enemy in enemyScripts)
            {
                if (enemy.hp > 0)
                {
                    return;
                }
            }
            active = false;
            cleared = true;
            foreach (GameObject door in doors)
            {
                door.SetActive(false);
            }
            gameManager.ExitCombat();
        }
    }

    public void Reset()
    {
        cleared = false;
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SetActive(false);
            enemies[i].transform.position = enemyStartPositions[i];
            enemies[i].GetComponent<Enemy>().hp = enemyScripts[i].maxHp;
        }
        foreach (GameObject door in doors)
        {
            door.SetActive(false);
        }
        active = false;
    }
}
