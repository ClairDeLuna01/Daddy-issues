using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    [System.NonSerialized]
    public PlayerController playerController;
    [System.NonSerialized]
    public Animator gunAnimator;
    private HandAnimationController handAnimationController;
    public ParticleSystem muzzleFlash;
    public AudioSource fireAudio;

    public AudioSource trackCalm;
    public AudioSource trackCombat;

    public bool inCombat = false;

    public float combatTrackMaxVolume = 1.0f;

    private IEnumerator FadeInCoroutine;
    private IEnumerator FadeOutCoroutine;
    public Player playerScript;

    private AudioSource[] footstepSounds;

    public GameState save = null;

    public GameObject[] enemies;
    public GameObject runnerPrefab;
    public GameObject rangerPrefab;

    [System.NonSerialized]
    public Arena[] arenas;

    public bool enableMusic = true;

    public bool bossFight = false;
    public GameObject boss;
    [System.NonSerialized]
    public Boss bossEnemy;

    private AudioSource rewindSound;
    private AudioSource pauseSound;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        gunAnimator = playerController.gun.GetComponent<Animator>();
        handAnimationController = GameObject.Find("Hand").GetComponent<HandAnimationController>();

        footstepSounds = playerController.footstepSounds;
        playerScript = player.GetComponent<Player>();

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        arenas = GameObject.FindObjectsOfType<Arena>();

        if (!enableMusic)
        {
            trackCalm.volume = 0;
            trackCombat.volume = 0;
        }

        SaveGame();

        bossEnemy = boss.GetComponent<Boss>();

        rewindSound = playerScript.rewindSound;
        pauseSound = playerScript.pauseSound;
    }



    public void ActivateSlow()
    {
        playerScript.slowing = true;
        handAnimationController.PlaySlowmo();
        Time.timeScale = 0.3f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
        fireAudio.pitch = 0.5f;
        trackCombat.pitch = 0.5f;
        trackCalm.pitch = 0.5f;
        for (int i = 0; i < footstepSounds.Length; i++)
        {
            footstepSounds[i].pitch = 0.5f;
        }
        playerController.jumpSound.pitch = 0.5f;
        rewindSound.pitch = 0.5f;
        pauseSound.pitch = 0.5f;
    }

    public void DeactivateSlow(bool playAnim = true)
    {
        playerScript.slowing = false;
        if (playAnim)
            handAnimationController.PlaySlowmo();
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
        fireAudio.pitch = 1.0f;
        trackCombat.pitch = 1.0f;
        trackCalm.pitch = 1.0f;
        for (int i = 0; i < footstepSounds.Length; i++)
        {
            footstepSounds[i].pitch = 1.0f;
        }
        playerController.jumpSound.pitch = 1.0f;
        rewindSound.pitch = 1.0f;
        pauseSound.pitch = 1.0f;
    }

    void Update()
    {
        if (playerScript.EnergyCheck(playerScript.slowDrainCost) && Input.GetKeyDown(KeyCode.LeftShift))
        {
            ActivateSlow();
        }
        else if (playerScript.slowing && (Input.GetKeyUp(KeyCode.LeftShift) || !playerScript.EnergyCheck(playerScript.slowDrainCost)))
        {
            DeactivateSlow();
        }
        if (Input.GetKeyDown(KeyCode.X) && enableMusic)
        {
            if (!inCombat)
            {
                EnterCombat();
            }
            else
            {
                ExitCombat();
            }
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            LoadSave();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            playerScript.Hit();
        }
    }

    public void Fire()
    {
        // gunAnimator.PlayInFixedTime("Idle", 1, 0.0f);
        gunAnimator.SetTrigger("Flip :)");
        muzzleFlash.Play();
        fireAudio.Play();
    }

    public void EnterCombat()
    {
        if (!inCombat && enableMusic)
        {
            inCombat = true;
            if (FadeOutCoroutine != null)
            {
                StopCoroutine(FadeInCoroutine);
                trackCombat.volume = 0;
                trackCalm.volume = 1;
            }
            FadeInCoroutine = FadeInCombat();
            StartCoroutine(FadeInCoroutine);
        }
    }

    public void ExitCombat()
    {
        if (inCombat && enableMusic)
        {
            inCombat = false;
            if (FadeInCoroutine != null)
            {
                StopCoroutine(FadeInCoroutine);
                trackCombat.volume = 1;
                trackCalm.volume = 0;
            }
            FadeOutCoroutine = FadeOutCombat();
            StartCoroutine(FadeOutCoroutine);
        }
    }

    private IEnumerator FadeInCombat()
    {
        while (trackCombat.volume < combatTrackMaxVolume)
        {
            trackCombat.volume += 0.005f;
            if (trackCalm.volume > 0)
                trackCalm.volume -= 0.01f;
            yield return new WaitForSeconds(0.02f);
        }

        trackCalm.volume = 0;

        FadeInCoroutine = null;
    }

    private IEnumerator FadeOutCombat()
    {
        while (trackCombat.volume > 0)
        {
            trackCombat.volume -= 0.02f;
            trackCalm.volume += 0.02f;
            yield return new WaitForSeconds(0.02f);
        }

        trackCalm.volume = 1;

        FadeOutCoroutine = null;
    }

    public void LoadSave()
    {
        player.SetActive(true);
        Camera.main.transform.parent = player.transform;
        foreach (Transform child in Camera.main.transform)
        {
            child.gameObject.SetActive(true);
        }
        player.transform.SetPositionAndRotation(save.playerPosition, save.playerRotation);
        playerScript.armor = save.armor;
        playerScript.godMode = save.godMode;
        playerScript.infiniteEnergy = save.infiniteEnergy;
        playerScript.energy = save.energy;
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SetActive(false);
        }

        for (int i = 0; i < save.AliveEnemies.Length; i++)
        {
            EnemyState enemyState = save.AliveEnemies[i];
            enemies[i].SetActive(true);
            enemies[i].GetComponent<Enemy>().hp = enemyState.health;
            enemies[i].GetComponent<Enemy>().aggro = enemyState.aggro;
        }

        ExitCombat();
        DeactivateSlow(false);
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;

        for (int i = 0; i < arenas.Length; i++)
        {
            if (!save.completedArenas[i])
            {
                arenas[i].Reset();
            }
        }

        playerScript.weapon.transform.rotation = Quaternion.Euler(-90f, 0, 0);
        playerScript.weapon.transform.position = Vector3.zero;
        gunAnimator.Play("Idle", -1, 0.0f);
    }

    public void SaveGame()
    {
        EnemyState[] enemyStates = new EnemyState[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemy = enemies[i].GetComponent<Enemy>();
            if (enemy != null && enemy.hp > 0)
                enemyStates[i] = new EnemyState(enemy.transform.position, enemy.transform.rotation, enemy.hp, enemy.enemyType, enemy.aggro);
        }
        bool[] completedArenas = new bool[arenas.Length];
        for (int i = 0; i < arenas.Length; i++)
        {
            completedArenas[i] = arenas[i].cleared;
        }

        save = new GameState(player.transform.position, player.transform.rotation, playerScript.armor, playerScript.godMode, playerScript.infiniteEnergy, playerScript.energy, enemyStates, completedArenas);
    }
}
