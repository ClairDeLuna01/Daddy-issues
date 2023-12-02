using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimationController : MonoBehaviour
{
	private Animator animator;
	public bool hasLoaded;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
		if(animator != null) {
			hasLoaded = true;
		} else {
			hasLoaded = false;
			Debug.Log("Hand Animator not loaded");
		}
    }

    public void PlayPause() {
		if(hasLoaded) {
			animator.SetTrigger("trPause");
		}
	}
	public void PlayRewind() {
		if(hasLoaded) {
			animator.SetTrigger("trRewind");
		}
	}
	public void PlaySlowmo() {
		if(hasLoaded) {
			animator.SetTrigger("trSlowmo");
		}
	}
}
