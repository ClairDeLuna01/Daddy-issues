using UnityEngine;

public class Consumable : MonoBehaviour 
{
	private Collider objectCollider;
	
	protected virtual void Start() {
		objectCollider = GetComponent<Collider>();
		if(objectCollider != null) {
			objectCollider.isTrigger = true;
		}
	}

}