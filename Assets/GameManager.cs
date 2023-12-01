using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    [System.NonSerialized]
    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(FlipGun());
        }
    }

    IEnumerator FlipGun()
    {
        GameObject gun = playerController.gun;
        float t = 0.0f;
        float duration = 0.1f;
        Quaternion startRot = gun.transform.localRotation;
        Quaternion endRot = Quaternion.Euler(0.0f, 0.0f, 90.0f);
        while (t < duration)
        {
            t += Time.deltaTime;
            gun.transform.localRotation = Quaternion.Lerp(startRot, endRot, t / duration);
            yield return null;
        }
        t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            gun.transform.localRotation = Quaternion.Lerp(endRot, startRot, t / duration);
            yield return null;
        }
    }
}
