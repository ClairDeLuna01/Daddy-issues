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

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        gunAnimator = playerController.gun.GetComponent<Animator>();
		handAnimationController = GameObject.Find("Hand").GetComponent<HandAnimationController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
			handAnimationController.PlaySlowmo();
            Time.timeScale = 0.3f;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
			handAnimationController.PlaySlowmo();
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }
    }

    public void Fire()
    {
        // gunAnimator.PlayInFixedTime("Idle", 1, 0.0f);
        gunAnimator.SetTrigger("Flip :)");
        muzzleFlash.Play();
    }
}
