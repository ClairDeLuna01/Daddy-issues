using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{
	public bool armor = false;
	public int energy = 50;
	public float Iframes = 20f;
	public bool godMode = true;
	public bool infiniteEnergy = false;
	public bool slowing = false;
	public bool freezing = false;
	public int slowDrainCost = 2;
	public int parryCost = 0;
	public int freezeCost = 20;
	public int freezeDrainCost = 1;
	private float drainFrames = 2f;
	private float nextDrain;
	private float nextDamage;
	private HandAnimationController handAnimationController;
	private GameManager gameManager;
    private Rigidbody rb;
    private PlayerController playerController;
	private RaycastHit target;

	protected void Start() {
		playerController = GetComponent<PlayerController>();
		handAnimationController = GameObject.Find("Hand").GetComponent<HandAnimationController>();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		nextDamage = Time.time;
		nextDrain = Time.time;
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

	public bool EnergyCheck(int cost) {
		return energy - cost > 0;
	}

	public void RemoveEnergy(int amount) {
		energy -= amount;
	}

    void Update()
    {
		if(Time.time >= nextDrain) {
			nextDrain = Time.time + 1f/drainFrames;
			if(slowing && !infiniteEnergy) 
			{
				if(EnergyCheck(slowDrainCost)) 
				{
					RemoveEnergy(slowDrainCost);
				}
				else 
				{
					slowing = false;
					gameManager.DeactivateSlow();
				}
			}
			if(freezing && !infiniteEnergy) 
			{
				if(EnergyCheck(freezeDrainCost))
				{
					RemoveEnergy(freezeDrainCost);
				}
				else
				{
					freezing = false;
					target.transform.GetComponent<Enemy>().Unfreeze();
				}
			}

		}

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 camDir = Camera.main.transform.forward;

            Ray ray = new(Camera.main.transform.position, camDir);

            // draw gizmo
            Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.red, 100.0f);

            if (Physics.Raycast(ray, out target, 100.0f))
            {
                if (target.collider.gameObject.CompareTag("Enemy"))
                {
                    // freeze
					if(freezing)
					{
						freezing = false;
						handAnimationController.PlayPause();
                    	target.transform.GetComponent<Enemy>().toggleFreeze();
                    	Debug.Log("Freeze");
					} 
					else if(EnergyCheck(freezeCost))
					{
						freezing = true;
						RemoveEnergy(freezeCost);
						handAnimationController.PlayPause();
                    	target.transform.GetComponent<Enemy>().toggleFreeze();
                    	Debug.Log("Freeze");
					}
                }
                else if (target.collider.gameObject.CompareTag("Projectile") && EnergyCheck(parryCost))
                {
                    // deflect
					RemoveEnergy(parryCost);
					handAnimationController.PlayRewind();
                    target.transform.GetComponent<Projectile>().Deflect();
                    Debug.Log("Deflect");
                }
            }
        }
    }
}
