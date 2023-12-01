using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{
	public bool armor = false;
	public int energy = 50;
	public float Iframes = 20f;
	public bool godMode = true;
	private float nextDamage;

	protected void Start() {
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
}
