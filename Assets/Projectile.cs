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

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine(destroyAfterTime(5.0f));

        rb = GetComponent<Rigidbody>();
    }

    IEnumerator destroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        if (!deflected)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && !deflected)
        {
            Debug.Log("Player hit");
        }
        else if (other.transform.CompareTag("Enemy") && deflected)
        {
            Debug.Log("Enemy hit");
            other.transform.GetComponent<Enemy>().kill();
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
            Vector3 dir = parent.transform.position - transform.position;

            rb.velocity = dir.normalized * 20.0f;

            // // rotate on the Y axis only to face the parent
            // Vector3 rot = transform.rotation.eulerAngles;
            // float newRot = Quaternion.LookRotation(parent.transform.position - transform.position).eulerAngles.y;
            // // interpolate the rotation so it's not instant
            // rot.y = Mathf.LerpAngle(rot.y, newRot, 10f * Time.deltaTime);
            // transform.rotation = Quaternion.Euler(rot);

            // destroy after 5 seconds
            Destroy(gameObject, 5.0f);
        }
    }
}
