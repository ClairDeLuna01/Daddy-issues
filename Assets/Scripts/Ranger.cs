using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger : Enemy
{
    public GameObject projectilePrefab;
    public static float speed = 3.0f;
    public static float range = 15.0f;
    public static float attackCooldown = 1.0f;

    private bool attacking = false;
    private bool attackingCooldown = false;
    private bool facePlayer = false;

    private RangerAnimatorController rangerAnimatorController;

    IEnumerator attackRoutine = null;

    new void Start()
    {
        base.Start();
        enemyType = EnemyType.Ranger;
        rangerAnimatorController = GetComponent<RangerAnimatorController>();
        if (rangerAnimatorController != null) rangerAnimatorController.PlayIdle();
    }

    void Move()
    {
        Vector3 direction = (gameManager.player.transform.position - transform.position).normalized;
        transform.position += speed * Time.deltaTime * direction;
    }

    // Update is called once per frame
    void Update()
    {
        if (aggro && !frozen)
        {
            if (!attackingCooldown && !attacking)
            {
                float distance = Vector3.Distance(gameManager.player.transform.position, transform.position);
                if (distance > range)
                {
                    Move();
                }
                else
                {
                    Attack();
                }
                // rotate on the Y axis only to face the player
                Vector3 rot = transform.rotation.eulerAngles;
                float newRot = Quaternion.LookRotation(gameManager.player.transform.position - transform.position).eulerAngles.y;
                // interpolate the rotation so it's not instant
                rot.y = Mathf.LerpAngle(rot.y, newRot, 10f * Time.deltaTime);
                transform.rotation = Quaternion.Euler(rot);
            }
            if (attackingCooldown)
            {
                // rotate on the Y axis only to face the walk direction
                Vector3 rot = transform.rotation.eulerAngles;
                rot.y = Quaternion.LookRotation(rb.velocity).eulerAngles.y;
                transform.rotation = Quaternion.Euler(rot);
            }
            if (facePlayer)
            {
                // rotate on the Y axis only to face the player
                Vector3 rot = transform.rotation.eulerAngles;
                float newRot = Quaternion.LookRotation(gameManager.player.transform.position - transform.position).eulerAngles.y;
                // interpolate the rotation so it's not instant
                rot.y = Mathf.LerpAngle(rot.y, newRot, 10f * Time.deltaTime);
                transform.rotation = Quaternion.Euler(rot);
            }
        }
    }

    void Attack()
    {
        if (!attacking)
        {
            attackRoutine = AttackRoutine();
            StartCoroutine(attackRoutine);
        }
    }

    IEnumerator AttackRoutine()
    {
		if (rangerAnimatorController != null) rangerAnimatorController.PlayShoot();
        attacking = true;
        yield return new WaitForSeconds(0.8f);
        Vector3 direction = (gameManager.player.transform.position - transform.position).normalized;
        Vector3 bulletRot = new(90f, Quaternion.LookRotation(gameManager.player.transform.position - transform.position).eulerAngles.y, 0);
        Quaternion bulletRotQ = Quaternion.Euler(bulletRot);
        GameObject projectile = Instantiate(projectilePrefab, transform.position + direction, bulletRotQ);
        projectile.GetComponent<Projectile>().parent = gameObject;
        projectile.GetComponent<Rigidbody>().velocity = direction * 20.0f;
        attackingCooldown = true;
        // walk in a random direction
		if (rangerAnimatorController != null) rangerAnimatorController.PlayRun();
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        rb.velocity = randomDirection * speed;
        yield return new WaitForSeconds(attackCooldown / 2);
        attackingCooldown = false;
        rb.velocity = Vector3.zero;
        facePlayer = true;
        yield return new WaitForSeconds(attackCooldown / 2 + Random.Range(0.0f, 0.5f));
        attacking = false;
        facePlayer = false;

        attackRoutine = null;
    }

    public override void Freeze()
    {
        base.Freeze();

        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }

        rb.velocity = Vector3.zero;
    }

    public override void Unfreeze()
    {
        base.Unfreeze();

        if (attackRoutine == null)
        {
            attackRoutine = AttackRoutine();
            StartCoroutine(attackRoutine);
        }
    }
}
