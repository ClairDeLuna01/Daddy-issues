using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Consumable : MonoBehaviour 
{
	private float frame = 0f;
	private Vector3 startPos;
	private Collider objectCollider;

	protected void Start() {
		startPos = transform.position;
		objectCollider = GetComponent<Collider>();
		if(objectCollider != null) {
			objectCollider.isTrigger = true;
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