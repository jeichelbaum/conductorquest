using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConductorView : MonoBehaviour {

    Animator animator;

    string anim_conductorBounce = "conductor_utzn";

	void Start () {

        animator = GetComponent<Animator>();
        BeatController.instance.OnBeatUpdate += OnBeatUpdate;
	}
	
	void Update () {
		
	}


    void OnBeatUpdate()
    {
        animator.Play(anim_conductorBounce);
    }
}
