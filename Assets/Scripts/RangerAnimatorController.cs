using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerAnimatorController : MonoBehaviour
{
    
    private Animator animator;
	public bool hasLoaded;
    void Start()
    {
        animator = GetComponent<Animator>();
		if(animator != null) {
			hasLoaded = true;
		} else {
			hasLoaded = false;
			Debug.Log("Ranger Animator not loaded");
		}
    }

	public void PlayIdle() {
		if(hasLoaded) {
			animator.SetBool("moving", false);
		}
	}
	public void PlayRun() {
		if(hasLoaded) {
			animator.SetBool("moving", true);
		}
	}
	public void PlayShoot() {
		if(hasLoaded) {
			animator.SetTrigger("trShoot");
		}
	}
}
