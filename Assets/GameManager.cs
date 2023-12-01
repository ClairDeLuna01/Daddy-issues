using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    [System.NonSerialized]
    public PlayerController playerController;
    private Animator gunAnimator;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        gunAnimator = playerController.gun.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Time.timeScale = 0.3f;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }
    }

    public void Fire()
    {
        // gunAnimator.PlayInFixedTime("Idle", 1, 0.0f);
        if (gunAnimator.GetCurrentAnimatorStateInfo(0).IsName("Flip"))
            gunAnimator.SetTrigger("Skip this sh*t");
        gunAnimator.SetTrigger("Flip :)");
    }
}
