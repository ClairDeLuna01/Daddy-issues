using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    enum AttackType
    {
        Burst,
        Spread,
        Homing,
        Predictive
    }

    public GameObject projectilePrefab;
    public float speed = 10.0f;
    public float range = 15.0f;
    public float attackCooldown = 1.0f;

    public int projectileCount = 5;

    private bool attacking = false;
    private bool attackingCooldown = false;
    private bool facePlayer = false;

    IEnumerator attackRoutine = null;

    public AudioSource pistolFire;
    public AudioSource automaticRifleFire;
    public AudioSource huntingRifleFire;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        enemyType = EnemyType.Boss;
    }

    private void OnEnable()
    {
        attacking = false;
        attackingCooldown = false;
        attackRoutine = null;
        facePlayer = false;
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
        if (!attacking && !frozen)
        {
            attackRoutine = AttackRoutine();
            StartCoroutine(attackRoutine);
        }
    }

    IEnumerator AttackRoutine()
    {
        attacking = true;
        yield return new WaitForSeconds(0.5f);
        // choose a random attack type
        // AttackType attackType = (AttackType)Random.Range(0, 4);
        AttackType attackType = AttackType.Burst;


        switch (attackType)
        {
            case AttackType.Burst: // projectileCount projectiles in quick succession
                {
                    for (int i = 0; i < projectileCount; i++)
                    {
                        Vector3 direction = (gameManager.player.transform.position - transform.position).normalized;
                        Vector3 bulletRot = new(90f, Quaternion.LookRotation(gameManager.player.transform.position - transform.position).eulerAngles.y, 0);
                        Quaternion bulletRotQ = Quaternion.Euler(bulletRot);
                        GameObject projectile = Instantiate(projectilePrefab, transform.position + direction, bulletRotQ);
                        projectile.GetComponent<Projectile>().parent = gameObject;
                        projectile.GetComponent<Rigidbody>().velocity = direction * 25.0f;
                        automaticRifleFire.Play();
                        yield return new WaitForSeconds(0.13f);
                    }
                    break;
                }
            case AttackType.Spread: // 3 projectiles in a 45 degree spread fired at the same time
                {
                    const float spread = 30f;
                    Vector3 direction = (gameManager.player.transform.position - transform.position).normalized;
                    Vector3 bulletRot = new(90f, Quaternion.LookRotation(gameManager.player.transform.position - transform.position).eulerAngles.y, 0);
                    Quaternion bulletRotQ = Quaternion.Euler(bulletRot);
                    GameObject projectile = Instantiate(projectilePrefab, transform.position, bulletRotQ);
                    projectile.GetComponent<Projectile>().parent = gameObject;
                    projectile.GetComponent<Rigidbody>().velocity = direction * 20.0f;
                    Vector3 direction2 = Quaternion.Euler(0, spread / 2, 0) * direction;
                    Vector3 bulletRot2 = new(90f, Quaternion.LookRotation(gameManager.player.transform.position - transform.position).eulerAngles.y, 0);
                    Quaternion bulletRotQ2 = Quaternion.Euler(bulletRot2);
                    GameObject projectile2 = Instantiate(projectilePrefab, transform.position, bulletRotQ2);
                    projectile2.GetComponent<Projectile>().parent = gameObject;
                    projectile2.GetComponent<Rigidbody>().velocity = direction2 * 20.0f;
                    Vector3 direction3 = Quaternion.Euler(0, -spread / 2, 0) * direction;
                    Vector3 bulletRot3 = new(90f, Quaternion.LookRotation(gameManager.player.transform.position - transform.position).eulerAngles.y, 0);
                    Quaternion bulletRotQ3 = Quaternion.Euler(bulletRot3);
                    GameObject projectile3 = Instantiate(projectilePrefab, transform.position, bulletRotQ3);
                    projectile3.GetComponent<Projectile>().parent = gameObject;
                    projectile3.GetComponent<Rigidbody>().velocity = direction3 * 20.0f;
                    pistolFire.Play();
                }
                break;

            // fires a slow homing projectile that follows the player for a short time
            case AttackType.Homing:
                {
                    Vector3 direction = (gameManager.player.transform.position - transform.position).normalized;
                    Vector3 bulletRot = new(90f, Quaternion.LookRotation(gameManager.player.transform.position - transform.position).eulerAngles.y, 0);
                    Quaternion bulletRotQ = Quaternion.Euler(bulletRot);
                    GameObject projectile = Instantiate(projectilePrefab, transform.position + direction, bulletRotQ);
                    projectile.GetComponent<Projectile>().parent = gameObject;
                    projectile.GetComponent<Rigidbody>().velocity = direction * 10.0f;
                    projectile.GetComponent<Projectile>().homing = true;
                    projectile.GetComponent<Projectile>().aliveTime = 2.5f;
                    pistolFire.Play();
                }
                break;

            // slowly fires multiple fast projectiles that predict the player's position based on their current velocity and the distance between the player and the boss
            case AttackType.Predictive:
                {
                    for (int i = 0; i < 4; i++)
                    {
                        huntingRifleFire.Play();
                        yield return new WaitForSeconds(.908f);
                        Vector3 direction = (gameManager.player.transform.position - transform.position).normalized;
                        float distance = Vector3.Distance(gameManager.player.transform.position, transform.position);
                        Vector3 playerVelocity = gameManager.player.GetComponent<Rigidbody>().velocity;
                        float time = distance / 40.0f;
                        Vector3 predictedPosition = gameManager.player.transform.position + playerVelocity * time;
                        Vector3 bulletRot = new(90f, Quaternion.LookRotation(predictedPosition - transform.position).eulerAngles.y, 0);
                        Quaternion bulletRotQ = Quaternion.Euler(bulletRot);
                        GameObject projectile = Instantiate(projectilePrefab, transform.position + direction, bulletRotQ);
                        projectile.GetComponent<Projectile>().parent = gameObject;
                        projectile.GetComponent<Rigidbody>().velocity = (predictedPosition - transform.position).normalized * 40.0f;
                        yield return new WaitForSeconds(.6f);
                    }
                }
                break;

            default:
                break;
        }


        attackingCooldown = true;
        rb.velocity = Vector3.zero;
        facePlayer = true;
        yield return new WaitForSeconds(attackCooldown);
        attackingCooldown = false;
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

        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public override void Unfreeze()
    {
        base.Unfreeze();

        if (attackRoutine == null)
        {
            attackRoutine = AttackRoutine();
            StartCoroutine(attackRoutine);
        }

        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
}
