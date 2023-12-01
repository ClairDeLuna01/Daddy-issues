using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameManager gameManager;
    private Rigidbody rb;
    private PlayerController playerController;
    public int hp = 100;
    public int energy = 50;



    public void RestoreHealth(int amount)
    {
        hp += amount;
        if (hp > 100) hp = 100;
    }
    public void RestoreEnergy(int amount)
    {
        energy += amount;
        if (energy > 100) energy = 100;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 camDir = Camera.main.transform.forward;

            Ray ray = new(Camera.main.transform.position, camDir);

            // draw gizmo
            Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.red, 100.0f);

            if (Physics.Raycast(ray, out RaycastHit hit, 100.0f))
            {
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    // freeze
                    hit.transform.GetComponent<Enemy>().toggleFreeze();
                    Debug.Log("Freeze");
                }
                else if (hit.collider.gameObject.CompareTag("Projectile"))
                {
                    // deflect
                    hit.transform.GetComponent<Projectile>().Deflect();

                    Debug.Log("Deflect");
                }
            }
        }
    }
}
