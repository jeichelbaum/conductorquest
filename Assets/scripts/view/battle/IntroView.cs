using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroView : MonoBehaviour {

    Animator animator;

    public delegate void AnimCallback();
    AnimCallback done_cb;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayIntro(AnimCallback cb)
    {
        done_cb = cb;
        animator.Play("intro");
    }

    void TriggerCallback()
    {
        if (done_cb != null) done_cb();
    }
    
}
