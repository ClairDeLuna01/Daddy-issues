using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : Enemy
{
    public static float speed = 10.0f;
    // Update is called once per frame
    void Update()
    {

    }

    void Run()
    {
        Vector3 direction = (gameManager.player.transform.position - transform.position).normalized;
        transform.position += speed * Time.deltaTime * direction;
    }
}
