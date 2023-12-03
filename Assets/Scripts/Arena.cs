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

    public bool saveOnClear = true;

    Vector3[] enemyStartPositions;

    public bool isBossArena = false;

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

            if (isBossArena)
            {
                gameManager.bossFight = true;
            }
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
            if (saveOnClear)
            {
                gameManager.SaveGame();
            }
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
        if (isBossArena)
        {
            doors[0].SetActive(true);
            enemies[0].SetActive(true);
            gameManager.bossFight = false;
        }
        active = false;
    }
}
