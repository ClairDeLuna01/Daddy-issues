using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : Enemy
{
    public static float speed = 7.0f;
    public static float range = 3.0f;

    private BoxCollider hitBox;
    private bool attacking = false;
    private bool attackingCooldown = false;


    protected new void Start()
    {
        // parent start
        base.Start();
        hitBox = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (aggro && !attackingCooldown)
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

            transform.LookAt(gameManager.player.transform);
        }
    }

    void Run()
    {
        Vector3 direction = (gameManager.player.transform.position - transform.position).normalized;
        transform.position += speed * Time.deltaTime * direction;
    }

    void Attack()
    {
        if (!attacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        attacking = true;
        yield return new WaitForSeconds(0.2f);

        hitBox.enabled = true;
        Vector3 direction = (gameManager.player.transform.position - transform.position).normalized;
        rb.velocity = direction * 10.0f;
        yield return new WaitForSeconds(0.4f);
        hitBox.enabled = false;
        rb.velocity = Vector3.zero;

        attacking = false;
        attackingCooldown = true;

        yield return new WaitForSeconds(1.0f);
        attackingCooldown = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit");
        }
    }
}
