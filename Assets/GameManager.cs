using System.Collections;
using System.Collections.Generic;
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
	}

	public void DeactivateSlow() {
		playerScript.slowing = false;
		handAnimationController.PlaySlowmo();
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ActivateSlow();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            DeactivateSlow();
        }
    }

    public void Fire()
    {
        // gunAnimator.PlayInFixedTime("Idle", 1, 0.0f);
        gunAnimator.SetTrigger("Flip :)");
        muzzleFlash.Play();
        fireAudio.Play();
    }
}
