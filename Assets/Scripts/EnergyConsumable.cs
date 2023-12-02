using UnityEngine;

public class EnergyConsumable : Consumable
{
	public int amount = 25;

    // Start is called before the first frame update

	void OnTriggerEnter(Collider other) {

		Player player = other.transform.GetComponent<Player>();

		if(player != null && player.energy < 100) {
			Debug.Log("Energy restored");
			player.RestoreEnergy(amount);
			Destroy(gameObject);
		}

	}

}
