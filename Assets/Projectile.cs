using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Destroy(gameObject, 5.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
		Destroy(gameObject);
        Player player = other.transform.GetComponent<Player>();
		if(player != null) 
		{
			player.Hit();
		}

    }
}
