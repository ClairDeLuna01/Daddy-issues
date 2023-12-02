using UnityEngine;

public class ArmorConsumable : Consumable
{
    // Start is called before the first frame update

	void OnTriggerEnter(Collider other) {

		Player player = other.transform.GetComponent<Player>();

		if(player != null && !player.armor) {
			Debug.Log("Armor restored");
			player.RestoreArmor();
			Destroy(gameObject);
		}

	}

}