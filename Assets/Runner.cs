using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : Enemy
{
    public static float speed = 10.0f;
    public static float range = 1.0f;

    private BoxCollider hitBox;

    protected new void Start()
    {
        // parent start
        base.Start();
        hitBox = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (aggro)
        {
            float distance = Vector3.Distance(gameManager.player.transform.position, transform.position);
            if (distance > range)
            {
                Run();
            }
            else
            {
                Attack();
            }
        }
    }

    void Run()
    {
        Vector3 direction = (gameManager.player.transform.position - transform.position).normalized;
        transform.position += speed * Time.deltaTime * direction;
    }

    void Attack()
    {

    }
}
