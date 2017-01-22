﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartView : MonoBehaviour {
    
    public static string anim_idle = "heart_idle";
    public static string anim_idle_dead = "heart_idle_dead";
    public static string anim_bounce = "heart_bounce";

    Animator animator;
    bool alive = true;

    void Start ()
    {
        animator = GetComponent<Animator>();
        BeatController.instance.OnBeatUpdate += OnBeatUpdate;
	}
	

    void OnBeatUpdate()
    {
        if (!alive) return;
        animator.Play(anim_bounce);
    }

    public void SetAlive(bool val)
    {
        alive = val;
        if (alive) animator.Play(anim_idle);
        if (!alive) animator.Play(anim_idle_dead);
    }
}
