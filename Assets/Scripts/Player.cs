using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{
	public bool armor = false;
	public int energy = 50;
	public float Iframes = 20f;
	public bool godMode = true;
	private float nextDamage;
	private HandAnimationController handAnimationController;
	private GameManager gameManager;
    private Rigidbody rb;
    private PlayerController playerController;

	protected void Start() {
		playerController = GetComponent<PlayerController>();
		handAnimationController = GameObject.Find("Hand").GetComponent<HandAnimationController>();
		nextDamage = Time.time;
	}

	public void RestoreArmor() {
		armor = true;
	}
	public void RestoreEnergy(int amount) {
		energy += amount;
		if(energy > 100) energy = 100;
	}

	public void Hit() {
		if(!godMode && Time.time >= nextDamage) {
			nextDamage = Time.time + 1f/Iframes;
			if(armor) {
				armor = false;
			} else {
				Destroy(gameObject);
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			}
		}
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
					handAnimationController.PlayPause();
                    hit.transform.GetComponent<Enemy>().toggleFreeze();
                    Debug.Log("Freeze");
                }
                else if (hit.collider.gameObject.CompareTag("Projectile"))
                {
                    // deflect
					handAnimationController.PlayRewind();
                    hit.transform.GetComponent<Projectile>().Deflect();

                    Debug.Log("Deflect");
                }
            }
        }
    }
}
