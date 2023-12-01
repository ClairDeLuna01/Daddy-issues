using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public int hp = 100;
	public int energy = 50;

	public void RestoreHealth(int amount) {
		hp += amount;
		if(hp > 100) hp = 100;
	}
	public void RestoreEnergy(int amount) {
		energy += amount;
		if(energy > 100) energy = 100;
	}
}
