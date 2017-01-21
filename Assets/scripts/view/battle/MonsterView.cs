using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterView : MonoBehaviour {

    Animator animator;

    string anim_monsterBounce = "monster_bounce";
    string anim_monsterFadeIn = "monster_fadein";
    string anim_monsterfadeOut = "monster_fadeout";

    bool dead = false;
    float t_dead = 0.2f;

    void Awake()
    {
        animator = GetComponent<Animator>();
        BeatController.instance.OnBeatUpdate += OnBeatUpdate;
    }
	
	void Update ()
    {
		if (dead)
        {
            t_dead -= Time.deltaTime;
            if(t_dead < 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnBeatUpdate()
    {
        if (dead) return;
        animator.Play(anim_monsterBounce);
    }


    public void PlayAnimationFadeIn()
    {
        animator.Play(anim_monsterFadeIn);
    }

    public void PlayAnimationFadeOut()
    {
        dead = true;
        animator.Play(anim_monsterfadeOut);
    }
}
