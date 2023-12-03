using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject parent;
    bool deflected = false;

    public LayerMask deflectedInclude;
    public LayerMask deflectedExclude;
    private Rigidbody rb;

    public float aliveTime = 5.0f;

    public bool homing = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine(DestroyAfterTime(aliveTime));

        rb = GetComponent<Rigidbody>();
    }

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        if (!deflected)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Destroy(gameObject);
        // Player player = other.transform.GetComponent<Player>();
        // if(player != null) 
        // {
        // 	player.Hit();
        // }

        if (other.transform.CompareTag("Player") && !deflected)
        {
            gameManager.playerScript.Hit();
        }
        else if (other.transform.CompareTag("Enemy") && deflected)
        {
            // Debug.Log("Enemy hit");
            other.transform.GetComponent<Enemy>().Hit(100);
        }
        Destroy(gameObject);
    }

    public void Deflect()
    {
        deflected = true;
        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        collider.includeLayers = deflectedInclude;
        collider.excludeLayers = deflectedExclude;
    }

    void Update()
    {
        if (deflected)
        {
            // aim for parent
            Vector3 dir = parent.transform.position + new Vector3(0, 1.0f, 0) - transform.position;

            rb.velocity = dir.normalized * 20.0f;

            // face the parent
            transform.LookAt(parent.transform);

            // destroy after 5 seconds
            Destroy(gameObject, 5.0f);
        }
        else if (homing)
        {
            // aim for player
            Vector3 dir = gameManager.player.transform.position - transform.position;

            rb.velocity = dir.normalized * 10.0f;

            // face the player
            transform.LookAt(gameManager.player.transform);

        }
    }
}
