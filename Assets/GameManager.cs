using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    [System.NonSerialized]
    public PlayerController playerController;
    private Animator gunAnimator;
    private HandAnimationController handAnimationController;
    public ParticleSystem muzzleFlash;
    public AudioSource fireAudio;

    public AudioSource trackCalm;
    public AudioSource trackCombat;

    public bool inCombat = false;

    public float combatTrackMaxVolume = 1.0f;

    private IEnumerator FadeInCoroutine;
    private IEnumerator FadeOutCoroutine;
	private Player playerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        gunAnimator = playerController.gun.GetComponent<Animator>();
        handAnimationController = GameObject.Find("Hand").GetComponent<HandAnimationController>();
		playerScript = player.GetComponent<Player>();
    }

	public void ActivateSlow() {
		playerScript.slowing = true;
		handAnimationController.PlaySlowmo();
        Time.timeScale = 0.3f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
		fireAudio.pitch = 0.5f;
        trackCombat.pitch = 0.5f;
        trackCalm.pitch = 0.5f;
	}

	public void DeactivateSlow() {
		playerScript.slowing = false;
		handAnimationController.PlaySlowmo();
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
		fireAudio.pitch = 1.0f;
        trackCombat.pitch = 1.0f;
        trackCalm.pitch = 1.0f;
	}

    void Update()
    {
        if (playerScript.EnergyCheck(playerScript.slowDrainCost) && Input.GetKeyDown(KeyCode.LeftShift))
        {
            ActivateSlow();
        }
        else if (playerScript.slowing && Input.GetKeyUp(KeyCode.LeftShift))
        {
            DeactivateSlow();
        }
        if (Input.GetKeyDown(KeyCode.X))
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
        if (!inCombat)
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
        if (inCombat)
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
}
