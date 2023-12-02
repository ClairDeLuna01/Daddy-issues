using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : Enemy
{
    public static float speed = 10.0f;
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
        if (aggro)
        {
            if (!attackingCooldown && !attacking)
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
                // rotate on the Y axis only to face the player
                Vector3 rot = transform.rotation.eulerAngles;
                float newRot = Quaternion.LookRotation(gameManager.player.transform.position - transform.position).eulerAngles.y;
                // interpolate the rotation so it's not instant
                rot.y = Mathf.LerpAngle(rot.y, newRot, 10f * Time.deltaTime);
                transform.rotation = Quaternion.Euler(rot);
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
        if (!attacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        Vector3 rot = transform.rotation.eulerAngles;
        rot.y = Quaternion.LookRotation(gameManager.player.transform.position - transform.position).eulerAngles.y;
        transform.rotation = Quaternion.Euler(rot);
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
        /*
		if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit");
        }
		*/
		Player player = other.transform.GetComponent<Player>();

		if(player != null) 
		{
			player.Hit();
		}
    }
}
