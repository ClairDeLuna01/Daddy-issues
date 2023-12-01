using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
	public int damage = 1;
	public float range = 20f;
	public float spread = 0.5f;
	public float firerate = 0.5f;
	public int pellets = 12;

	private float nextTimeToFire = 0f;

	public Camera fpsCam;

    // Update is called once per frame
    void Update()
    {
		if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire) {
			nextTimeToFire = Time.time + 1f/firerate;
			Shoot();
		}
        
    }

	void Shoot() 
	{
		for(int i = 0; i < pellets; i++) {

			RaycastHit hit;
			Vector3 shootDirection = fpsCam.transform.forward + new Vector3
			(
				Random.Range(-spread, spread),
				Random.Range(-spread, spread),
				Random.Range(-spread, spread)
			);

			if(Physics.Raycast(fpsCam.transform.position, shootDirection, out hit, range)) 
			{
				Debug.Log("Pellet " + i + " : " + hit.transform.name);

				Enemy enemy = hit.transform.GetComponent<Enemy>();
				if(enemy != null) {
					enemy.Hit(damage);
				}

			}

		}
		

	}
}
