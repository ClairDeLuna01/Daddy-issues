using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public int damage = 1;
    public float range = 20f;
    public float spread = 0.5f;
    public float firerate = 0.5f;
    public int pellets = 12;

    public GameObject bulletHoleDecal;

    private float nextTimeToFire = 0f;

    public Camera fpsCam;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / firerate;
            Shoot();
        }

    }

    void Shoot()
    {
        for (int i = 0; i < pellets; i++)
        {

            RaycastHit hit;
            Vector3 shootDirection = fpsCam.transform.forward + new Vector3
            (
                Random.Range(-spread, spread),
                Random.Range(-spread, spread),
                Random.Range(-spread, spread)
            );

            if (Physics.Raycast(fpsCam.transform.position, shootDirection, out hit, range))
            {
                // Debug.Log("Pellet " + i + " : " + hit.transform.name);
                // draw gizmo
                Debug.DrawRay(fpsCam.transform.position, shootDirection * hit.distance, Color.yellow, 15.0f);

                // check if tag is Enemy
                if (hit.transform.CompareTag("Enemy"))
                {
                    // getComponent is expensive so we only want to call it when absolutely necessary
                    Enemy enemy = hit.transform.GetComponent<Enemy>();
                    enemy.Hit(damage);
                }
                else if (hit.transform.CompareTag("Floor"))
                {
                    // instantiate bullet hole decal
                    GameObject decal = Instantiate(bulletHoleDecal, hit.point + hit.normal * 0.025f, Quaternion.LookRotation(hit.normal));
                    decal.transform.rotation = Quaternion.Euler(decal.transform.rotation.eulerAngles.x, decal.transform.rotation.eulerAngles.y, Random.Range(0, 360));
                    // destroy decal after 5 seconds
                    Destroy(decal, 60.0f);
                }

            }

        }

        gameManager.Fire();
    }
}
