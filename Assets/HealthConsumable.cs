using UnityEngine;

public class HealthConsumable : Consumable
{
	public int amount = 50;

    // Start is called before the first frame update

	void OnTriggerEnter(Collider other) {

		Player player = other.transform.GetComponent<Player>();

		if(player != null && player.hp < 100) {
			player.RestoreHealth(amount);
			Destroy(gameObject);
		}

	}

}