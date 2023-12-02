using UnityEngine;
using System;

public class ArmorConsumable : Consumable
{
    private float frame = 0f;
	private Vector3 startPos;
	private Player playerToLook;

    // Start is called before the first frame update

	protected override void Start() {
		base.Start();
		startPos = transform.position;
		playerToLook = GameObject.Find("Player").GetComponent<Player>();
	}

	void OnTriggerEnter(Collider other) {

		Player player = other.transform.GetComponent<Player>();

		if(player != null && !player.armor) {
			Debug.Log("Armor restored");
			player.RestoreArmor();
			Destroy(gameObject);
		}

	}

	void Animate() {
		float offset = (float)Math.PI/180f * frame;
		transform.position = startPos + new Vector3 (0f, (float)Math.Sin(offset)/4f + 0.5f, 0f);
		Vector3 rot = transform.rotation.eulerAngles;
        float newRot = Quaternion.LookRotation(playerToLook.transform.position - transform.position).eulerAngles.y;
        rot.y = Mathf.LerpAngle(rot.y, newRot, 10f * Time.deltaTime);
        transform.rotation = Quaternion.Euler(rot);
		frame = (frame + Time.deltaTime*100f) % 360;
	}

	void Update() {
		Animate();
	}

}