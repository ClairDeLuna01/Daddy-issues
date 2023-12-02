using UnityEngine;

public class Consumable : MonoBehaviour 
{
	private Collider objectCollider;
	protected GameManager gameManager;
	

	protected virtual void Start() {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		objectCollider = GetComponent<Collider>();
		if(objectCollider != null) {
			objectCollider.isTrigger = true;
		}
	}

}