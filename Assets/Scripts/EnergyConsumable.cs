using UnityEngine;
using System;

public class EnergyConsumable : Consumable
{
	public int amount = 25;
	private float frame = 0f;
	private Vector3 startPos;

    // Start is called before the first frame update

	protected override void Start() {
		base.Start();
		startPos = transform.position;
	}
	void OnTriggerEnter(Collider other) {

		Player player = other.transform.GetComponent<Player>();

		if(player != null && player.energy < 100) {
			Debug.Log("Energy restored");
			player.RestoreEnergy(amount);
			Destroy(gameObject);
		}

	}

	void Animate() {
		float offset = (float)Math.PI/180f * frame;
		transform.position = startPos + new Vector3 (0f, (float)Math.Sin(offset)/4f + 0.5f, 0f);
		transform.RotateAround(transform.position, new Vector3 (0f, 1f, 0f), Time.deltaTime*100f);
		frame = (frame + Time.deltaTime*100f) % 360;
	}

	void Update() {
		Animate();
	}

}
