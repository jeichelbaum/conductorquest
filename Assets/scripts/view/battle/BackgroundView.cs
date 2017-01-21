using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundView : MonoBehaviour {

    string anim_switch = "background_switch";

    Animator animator;

	void Start ()
    {
        animator = GetComponent<Animator>();
	}
	
    public void PlaySwitchAnimation()
    {
        animator.Play(anim_switch);
    }
}
